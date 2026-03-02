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
    public class OrdersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Order>      Orders      => AppDatabase.Instance.Orders;
        public ObservableCollection<Department> Departments => AppDatabase.Instance.Departments;
        public ObservableCollection<Ingredient> Ingredients => AppDatabase.Instance.Ingredients;

        private Order? _selected;
        public Order? Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                OnPropertyChanged();
                IsEditing  = value != null;
                OrderItems = value != null
                    ? new ObservableCollection<OrderItem>(value.Items)
                    : new();
            }
        }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        private ObservableCollection<OrderItem> _orderItems = new();
        public ObservableCollection<OrderItem> OrderItems
        {
            get => _orderItems;
            set { _orderItems = value; OnPropertyChanged(); }
        }

        public ICommand AddCommand       { get; }
        public ICommand DeleteCommand    { get; }
        public ICommand SaveCommand      { get; }
        public ICommand AddItemCommand   { get; }
        public ICommand RemoveItemCommand{ get; }

        public OrdersViewModel()
        {
            AddCommand        = new RelayCommand(Add);
            DeleteCommand     = new RelayCommand(Delete, () => Selected != null);
            SaveCommand       = new RelayCommand(Save);
            AddItemCommand    = new RelayCommand(AddItem, () => Selected != null);
            RemoveItemCommand = new RelayCommand<OrderItem>(RemoveItem);
        }

        private void Add()
        {
            var o = new Order { Id = AppDatabase.Instance.NextId(), OrderDate = DateTime.Today };
            if (Departments.Count > 0) { o.DepartmentId = Departments[0].Id; o.Department = Departments[0]; }
            Orders.Add(o);
            Selected = o;
        }

        private void Delete()
        {
            if (Selected != null) { Orders.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save()
        {
            if (Selected != null)
            {
                Selected.Items = new System.Collections.Generic.List<OrderItem>(OrderItems);
                if (Selected.Department != null) Selected.DepartmentId = Selected.Department.Id;
                // Deduct from stock
                foreach (var item in OrderItems)
                {
                    foreach (var ing in AppDatabase.Instance.Ingredients)
                    {
                        if (ing.Id == item.IngredientId)
                        {
                            ing.StockBalance -= item.Quantity;
                            break;
                        }
                    }
                }
            }
            AppDatabase.Instance.Save();
            MessageBox.Show("Заказ сохранён. Остатки обновлены.", "Сохранено", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddItem()
        {
            if (Ingredients.Count == 0)
            {
                MessageBox.Show("Нет ингредиентов на складе.", "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }
            var ing = Ingredients[0];
            OrderItems.Add(new OrderItem
            {
                IngredientId   = ing.Id,
                IngredientName = ing.Name,
                Unit           = ing.Unit,
                Quantity       = 1
            });
        }

        private void RemoveItem(OrderItem? item)
        {
            if (item != null) OrderItems.Remove(item);
        }
    }
}
