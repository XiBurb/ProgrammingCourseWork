using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class ProductsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Product>      Products    => AppDatabase.Instance.Products;
        public ObservableCollection<ProductGroup> Groups      => AppDatabase.Instance.ProductGroups;
        public ObservableCollection<Ingredient>   Ingredients => AppDatabase.Instance.Ingredients;

        private Product? _selected;
        public Product? Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
                IsEditing = value != null;
                if (value != null)
                    RecipeItems = new ObservableCollection<RecipeItem>(value.Recipe);
            }
        }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        private ObservableCollection<RecipeItem> _recipeItems = new();
        public ObservableCollection<RecipeItem> RecipeItems
        {
            get => _recipeItems;
            set { _recipeItems = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand          { get; }
        public ICommand DeleteCommand       { get; }
        public ICommand SaveCommand         { get; }
        public ICommand AddRecipeItemCommand{ get; }
        public ICommand RemoveRecipeItemCommand { get; }
        public ICommand BrowsePhotoCommand  { get; }

        public ProductsViewModel()
        {
            AddCommand             = new RelayCommand(Add);
            DeleteCommand          = new RelayCommand(Delete, () => Selected != null);
            SaveCommand            = new RelayCommand(Save);
            AddRecipeItemCommand   = new RelayCommand(AddRecipeItem, () => Selected != null);
            RemoveRecipeItemCommand= new RelayCommand<RecipeItem>(RemoveRecipeItem);
            BrowsePhotoCommand     = new RelayCommand(BrowsePhoto, () => Selected != null);
        }

        private void Add()
        {
            var p = new Product { Id = AppDatabase.Instance.NextId(), Name = "Новое блюдо", Yield = "1 порция" };
            if (Groups.Count > 0) { p.GroupId = Groups[0].Id; p.Group = Groups[0]; }
            Products.Add(p);
            Selected = p;
        }

        private void Delete()
        {
            if (Selected != null) { Products.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save()
        {
            if (Selected != null)
            {
                Selected.Recipe = new System.Collections.Generic.List<RecipeItem>(RecipeItems);
                if (Selected.Group != null) Selected.GroupId = Selected.Group.Id;
            }
            AppDatabase.Instance.Save();
        }

        private void AddRecipeItem()
        {
            if (Ingredients.Count == 0)
            {
                MessageBox.Show("Сначала добавьте ингредиенты на склад.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var ing = Ingredients[0];
            RecipeItems.Add(new RecipeItem
            {
                IngredientId   = ing.Id,
                IngredientName = ing.Name,
                GrossWeight    = 100,
                NetWeight      = 90
            });
        }

        private void RemoveRecipeItem(RecipeItem? item)
        {
            if (item != null) RecipeItems.Remove(item);
        }

        private void BrowsePhoto()
        {
            var dlg = new Microsoft.Win32.OpenFileDialog
            {
                Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif|All files|*.*",
                Title  = "Выберите фото блюда"
            };
            if (dlg.ShowDialog() == true && Selected != null)
            {
                try
                {
                    // Copy photo to app's Photos folder
                    var photosDir = System.IO.Path.Combine(
                        System.Environment.GetFolderPath(System.Environment.SpecialFolder.ApplicationData),
                        "CateringIS", "Photos");
                    System.IO.Directory.CreateDirectory(photosDir);

                    var ext = System.IO.Path.GetExtension(dlg.FileName);
                    var newName = $"product_{Selected.Id}_{System.DateTime.Now:yyyyMMdd_HHmmss}{ext}";
                    var destPath = System.IO.Path.Combine(photosDir, newName);

                    System.IO.File.Copy(dlg.FileName, destPath, true);
                    Selected.PhotoPath = destPath;
                }
                catch (System.Exception ex)
                {
                    MessageBox.Show($"Ошибка сохранения фото: {ex.Message}", "Ошибка", 
                        MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }

    // Generic relay command helper
    public class RelayCommand<T> : ICommand
    {
        private readonly System.Action<T?> _execute;
        private readonly System.Func<T?, bool>? _can;
        public RelayCommand(System.Action<T?> execute, System.Func<T?, bool>? can = null) { _execute = execute; _can = can; }
        public event System.EventHandler? CanExecuteChanged { add => CommandManager.RequerySuggested += value; remove => CommandManager.RequerySuggested -= value; }
        public bool CanExecute(object? p) => _can?.Invoke(p is T t ? t : default) ?? true;
        public void Execute(object? p) => _execute(p is T t ? t : default);
    }
}
