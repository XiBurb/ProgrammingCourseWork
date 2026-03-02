using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using CateringIS.Models;

namespace CateringIS.Data
{
    public class AppDatabase
    {
        private static AppDatabase? _instance;
        public static AppDatabase Instance => _instance ??= new AppDatabase();

        // Папка Data/ рядом с исполняемым файлом
        private static readonly string DataFolder = Path.Combine(
            AppDomain.CurrentDomain.BaseDirectory, "Data");
        
        public static readonly string PhotosFolder = Path.Combine(DataFolder, "Photos");

        public ObservableCollection<Supplier> Suppliers { get; set; } = new();
        public ObservableCollection<Ingredient> Ingredients { get; set; } = new();
        public ObservableCollection<ProductGroup> ProductGroups { get; set; } = new();
        public ObservableCollection<Product> Products { get; set; } = new();
        public ObservableCollection<Delivery> Deliveries { get; set; } = new();
        public ObservableCollection<Department> Departments { get; set; } = new();
        public ObservableCollection<Order> Orders { get; set; } = new();
        public ObservableCollection<SalesRecord> SalesRecords { get; set; } = new();

        public static ObservableCollection<Models.Ingredient> InstanceIngredients => Instance.Ingredients;

        private int _nextId = 1;
        
        public int NextId()
        {
            var usedIds = new HashSet<int>();
            foreach (var s in Suppliers) usedIds.Add(s.Id);
            foreach (var i in Ingredients) usedIds.Add(i.Id);
            foreach (var g in ProductGroups) usedIds.Add(g.Id);
            foreach (var p in Products) usedIds.Add(p.Id);
            foreach (var d in Deliveries) usedIds.Add(d.Id);
            foreach (var dept in Departments) usedIds.Add(dept.Id);
            foreach (var o in Orders) usedIds.Add(o.Id);
            foreach (var sr in SalesRecords) usedIds.Add(sr.Id);

            int id = 1;
            while (usedIds.Contains(id))
                id++;

            _nextId = id + 1;
            return id;
        }

        private AppDatabase() { Load(); }

        private string FilePath(string name) => Path.Combine(DataFolder, name + ".json");

        private static JsonSerializerSettings Settings => new()
        {
            Formatting = Formatting.Indented,
            NullValueHandling = NullValueHandling.Ignore
        };

        public void Save()
        {
            Directory.CreateDirectory(DataFolder);
            File.WriteAllText(FilePath("suppliers"),    JsonConvert.SerializeObject(Suppliers.ToList(), Settings));
            File.WriteAllText(FilePath("ingredients"),  JsonConvert.SerializeObject(Ingredients.ToList(), Settings));
            File.WriteAllText(FilePath("groups"),       JsonConvert.SerializeObject(ProductGroups.ToList(), Settings));
            File.WriteAllText(FilePath("products"),     JsonConvert.SerializeObject(Products.ToList(), Settings));
            File.WriteAllText(FilePath("deliveries"),   JsonConvert.SerializeObject(Deliveries.ToList(), Settings));
            File.WriteAllText(FilePath("departments"),  JsonConvert.SerializeObject(Departments.ToList(), Settings));
            File.WriteAllText(FilePath("orders"),       JsonConvert.SerializeObject(Orders.ToList(), Settings));
            File.WriteAllText(FilePath("sales"),        JsonConvert.SerializeObject(SalesRecords.ToList(), Settings));
            File.WriteAllText(FilePath("meta"),         JsonConvert.SerializeObject(_nextId));
        }

        private void Load()
        {
            Directory.CreateDirectory(DataFolder);
            Directory.CreateDirectory(PhotosFolder);

            Suppliers    = Load<Supplier>("suppliers");
            Ingredients  = Load<Ingredient>("ingredients");
            ProductGroups= Load<ProductGroup>("groups");
            Products     = Load<Product>("products");
            Deliveries   = Load<Delivery>("deliveries");
            Departments  = Load<Department>("departments");
            Orders       = Load<Order>("orders");
            SalesRecords = Load<SalesRecord>("sales");

            var metaFile = FilePath("meta");
            _nextId = File.Exists(metaFile)
                ? JsonConvert.DeserializeObject<int>(File.ReadAllText(metaFile))
                : 1;

            LinkReferences();

            if (ProductGroups.Count == 0) SeedDefaults();
        }

        private ObservableCollection<T> Load<T>(string name)
        {
            var file = FilePath(name);
            if (!File.Exists(file)) return new();
            try
            {
                var list = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(file)) ?? new();
                return new ObservableCollection<T>(list);
            }
            catch { return new(); }
        }

        private void LinkReferences()
        {
            var supMap   = Suppliers.ToDictionary(s => s.Id);
            var ingMap   = Ingredients.ToDictionary(i => i.Id);
            var grpMap   = ProductGroups.ToDictionary(g => g.Id);
            var deptMap  = Departments.ToDictionary(d => d.Id);
            var prodMap  = Products.ToDictionary(p => p.Id);

            foreach (var ing in Ingredients)
                if (supMap.TryGetValue(ing.SupplierId, out var s)) ing.Supplier = s;

            foreach (var prod in Products)
            {
                if (grpMap.TryGetValue(prod.GroupId, out var g)) prod.Group = g;
                foreach (var ri in prod.Recipe)
                    if (ingMap.TryGetValue(ri.IngredientId, out var i)) ri.IngredientName = i.Name;
            }

            foreach (var del in Deliveries)
                if (supMap.TryGetValue(del.SupplierId, out var s)) del.Supplier = s;

            foreach (var ord in Orders)
            {
                if (deptMap.TryGetValue(ord.DepartmentId, out var d)) ord.Department = d;
            }

            foreach (var sr in SalesRecords)
                if (prodMap.TryGetValue(sr.ProductId, out var p)) sr.ProductName = p.Name;
        }

        private void SeedDefaults()
        {
            var groups = new[] { "Закуски", "Первые блюда", "Вторые блюда", "Десерты", "Напитки", "Кондитерские изделия", "Выпечка" };
            foreach (var g in groups)
                ProductGroups.Add(new ProductGroup { Id = NextId(), Name = g });

            var depts = new[] { "Кухня", "Бар", "Кондитерский цех" };
            foreach (var d in depts)
                Departments.Add(new Department { Id = NextId(), Name = d });

            Save();
        }
    }
}
