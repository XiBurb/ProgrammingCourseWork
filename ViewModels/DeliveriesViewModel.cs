using System;
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
    public class DeliveriesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Delivery>   Deliveries  => AppDatabase.Instance.Deliveries;
        public ObservableCollection<Supplier>   Suppliers   => AppDatabase.Instance.Suppliers;
        public ObservableCollection<Ingredient> Ingredients => AppDatabase.Instance.Ingredients;

        private Delivery? _selected;
        public Delivery? Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
                IsEditing = value != null;
                DeliveryItems = value != null
                    ? new ObservableCollection<DeliveryItem>(value.Items)
                    : new();
            }
        }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        private ObservableCollection<DeliveryItem> _deliveryItems = new();
        public ObservableCollection<DeliveryItem> DeliveryItems
        {
            get => _deliveryItems;
            set { _deliveryItems = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand       { get; }
        public ICommand DeleteCommand    { get; }
        public ICommand SaveCommand      { get; }
        public ICommand AddItemCommand   { get; }
        public ICommand RemoveItemCommand{ get; }

        public DeliveriesViewModel()
        {
            AddCommand        = new RelayCommand(Add);
            DeleteCommand     = new RelayCommand(Delete, () => Selected != null);
            SaveCommand       = new RelayCommand(Save);
            AddItemCommand    = new RelayCommand(AddItem, () => Selected != null);
            RemoveItemCommand = new RelayCommand<DeliveryItem>(RemoveItem);
        }

        private void Add()
        {
            var d = new Delivery { Id = AppDatabase.Instance.NextId(), DeliveryDate = DateTime.Today };
            if (Suppliers.Count > 0) { d.SupplierId = Suppliers[0].Id; d.Supplier = Suppliers[0]; }
            Deliveries.Add(d);
            Selected = d;
        }

        private void Delete()
        {
            if (Selected != null) { Deliveries.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save()
        {
            if (Selected != null)
            {
                Selected.Items = new System.Collections.Generic.List<DeliveryItem>(DeliveryItems);
                if (Selected.Supplier != null) Selected.SupplierId = Selected.Supplier.Id;
                // Update stock balances
                foreach (var item in DeliveryItems)
                {
                    foreach (var i in AppDatabase.Instance.Ingredients)
                    {
                        if (i.Id == item.IngredientId)
                        {
                            i.StockBalance += item.Quantity;
                            break;
                        }
                    }
                }
            }
            AppDatabase.Instance.Save();
            MessageBox.Show("Приход сохранён. Остатки обновлены.", "Сохранено", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddItem()
        {
            if (Ingredients.Count == 0)
            {
                MessageBox.Show("Добавьте ингредиенты на склад.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var ing = Ingredients[0];
            DeliveryItems.Add(new DeliveryItem
            {
                IngredientId   = ing.Id,
                IngredientName = ing.Name,
                Unit           = ing.Unit,
                PurchasePrice  = 0,
                Quantity       = 1
            });
        }

        private void RemoveItem(DeliveryItem? item)
        {
            if (item != null) DeliveryItems.Remove(item);
        }
    }
}
