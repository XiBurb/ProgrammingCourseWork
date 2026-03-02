using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace CateringIS.Models
{
    public abstract class ObservableObject : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        protected bool Set<T>(ref T field, T value, [CallerMemberName] string? name = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value)) return false;
            field = value;
            OnPropertyChanged(name);
            return true;
        }
    }

    public class Supplier : ObservableObject
    {
        private int _id;
        private string _name = "";
        private string _address = "";
        private string _managerFullName = "";
        private string _managerPhone = "";
        private string _bankName = "";
        private string _bankAccount = "";
        private string _taxId = "";

        public int Id { get => _id; set => Set(ref _id, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Address { get => _address; set => Set(ref _address, value); }
        public string ManagerFullName { get => _managerFullName; set => Set(ref _managerFullName, value); }
        public string ManagerPhone { get => _managerPhone; set => Set(ref _managerPhone, value); }
        public string BankName { get => _bankName; set => Set(ref _bankName, value); }
        public string BankAccount { get => _bankAccount; set => Set(ref _bankAccount, value); }
        public string TaxId { get => _taxId; set => Set(ref _taxId, value); }

        public override string ToString() => Name;
    }

    public class Ingredient : ObservableObject
    {
        private int _id;
        private string _name = "";
        private string _unit = "";
        private decimal _priceMarkupPercent;
        private decimal _stockBalance;
        private int _supplierId;
        private Supplier? _supplier;

        public int Id { get => _id; set => Set(ref _id, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public string Unit { get => _unit; set => Set(ref _unit, value); }
        public decimal PriceMarkupPercent { get => _priceMarkupPercent; set => Set(ref _priceMarkupPercent, value); }
        public decimal StockBalance { get => _stockBalance; set => Set(ref _stockBalance, value); }
        public int SupplierId { get => _supplierId; set => Set(ref _supplierId, value); }
        public Supplier? Supplier { get => _supplier; set => Set(ref _supplier, value); }

        public override string ToString() => Name;
    }

    public class ProductGroup : ObservableObject
    {
        private int _id;
        private string _name = "";

        public int Id { get => _id; set => Set(ref _id, value); }
        public string Name { get => _name; set => Set(ref _name, value); }

        public override string ToString() => Name;
    }

    public class RecipeItem : ObservableObject
    {
        private int _ingredientId;
        private string _ingredientName = "";
        private decimal _grossWeight;
        private decimal _netWeight;

        public int IngredientId { get => _ingredientId; set => Set(ref _ingredientId, value); }
        public string IngredientName { get => _ingredientName; set => Set(ref _ingredientName, value); }
        public decimal GrossWeight { get => _grossWeight; set => Set(ref _grossWeight, value); }
        public decimal NetWeight { get => _netWeight; set => Set(ref _netWeight, value); }
    }

    public class Product : ObservableObject
    {
        private int _id;
        private string _name = "";
        private int _groupId;
        private ProductGroup? _group;
        private decimal _cost;
        private string _yield = "";
        private string _preparationDescription = "";
        private string _photoPath = "";
        private List<RecipeItem> _recipe = new();

        public int Id { get => _id; set => Set(ref _id, value); }
        public string Name { get => _name; set => Set(ref _name, value); }
        public int GroupId { get => _groupId; set => Set(ref _groupId, value); }
        public ProductGroup? Group { get => _group; set => Set(ref _group, value); }
        public decimal Cost { get => _cost; set => Set(ref _cost, value); }
        public string Yield { get => _yield; set => Set(ref _yield, value); }
        public string PreparationDescription { get => _preparationDescription; set => Set(ref _preparationDescription, value); }
        public string PhotoPath { get => _photoPath; set => Set(ref _photoPath, value); }
        public List<RecipeItem> Recipe { get => _recipe; set => Set(ref _recipe, value); }

        public override string ToString() => Name;
    }

    public class DeliveryItem : ObservableObject
    {
        private int _ingredientId;
        private string _ingredientName = "";
        private string _unit = "";
        private decimal _purchasePrice;
        private decimal _quantity;

        public int IngredientId { get => _ingredientId; set => Set(ref _ingredientId, value); }
        public string IngredientName { get => _ingredientName; set => Set(ref _ingredientName, value); }
        public string Unit { get => _unit; set => Set(ref _unit, value); }
        public decimal PurchasePrice { get => _purchasePrice; set => Set(ref _purchasePrice, value); }
        public decimal Quantity { get => _quantity; set => Set(ref _quantity, value); }
    }

    public class Delivery : ObservableObject
    {
        private int _id;
        private int _supplierId;
        private Supplier? _supplier;
        private DateTime _deliveryDate = DateTime.Today;
        private List<DeliveryItem> _items = new();

        public int Id { get => _id; set => Set(ref _id, value); }
        public int SupplierId { get => _supplierId; set => Set(ref _supplierId, value); }
        public Supplier? Supplier { get => _supplier; set => Set(ref _supplier, value); }
        public DateTime DeliveryDate { get => _deliveryDate; set => Set(ref _deliveryDate, value); }
        public List<DeliveryItem> Items { get => _items; set => Set(ref _items, value); }
    }

    public class Department : ObservableObject
    {
        private int _id;
        private string _name = "";

        public int Id { get => _id; set => Set(ref _id, value); }
        public string Name { get => _name; set => Set(ref _name, value); }

        public override string ToString() => Name;
    }

    public class OrderItem : ObservableObject
    {
        private int _ingredientId;
        private string _ingredientName = "";
        private string _unit = "";
        private decimal _quantity;

        public int IngredientId { get => _ingredientId; set => Set(ref _ingredientId, value); }
        public string IngredientName { get => _ingredientName; set => Set(ref _ingredientName, value); }
        public string Unit { get => _unit; set => Set(ref _unit, value); }
        public decimal Quantity { get => _quantity; set => Set(ref _quantity, value); }
    }

    public class Order : ObservableObject
    {
        private int _id;
        private int _departmentId;
        private Department? _department;
        private DateTime _orderDate = DateTime.Today;
        private List<OrderItem> _items = new();

        public int Id { get => _id; set => Set(ref _id, value); }
        public int DepartmentId { get => _departmentId; set => Set(ref _departmentId, value); }
        public Department? Department { get => _department; set => Set(ref _department, value); }
        public DateTime OrderDate { get => _orderDate; set => Set(ref _orderDate, value); }
        public List<OrderItem> Items { get => _items; set => Set(ref _items, value); }
    }

    public class SalesRecord : ObservableObject
    {
        private int _id;
        private DateTime _date = DateTime.Today;
        private int _productId;
        private string _productName = "";
        private decimal _salesVolume;
        private string _unit = "";

        public int Id { get => _id; set => Set(ref _id, value); }
        public DateTime Date { get => _date; set => Set(ref _date, value); }
        public int ProductId { get => _productId; set => Set(ref _productId, value); }
        public string ProductName { get => _productName; set => Set(ref _productName, value); }
        public decimal SalesVolume { get => _salesVolume; set => Set(ref _salesVolume, value); }
        public string Unit { get => _unit; set => Set(ref _unit, value); }
    }
}
