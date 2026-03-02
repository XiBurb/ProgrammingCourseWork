using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections.Generic;
using System.Windows.Input;
using CateringIS.Helpers;

namespace CateringIS.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        private object? _currentView;
        public object? CurrentView { get => _currentView; set { _currentView = value; OnPropertyChanged(); } }

        private string _currentTitle = "Главная";
        public string CurrentTitle { get => _currentTitle; set { _currentTitle = value; OnPropertyChanged(); } }

        // Child ViewModels
        public SuppliersViewModel   SuppliersVM   { get; } = new();
        public IngredientsViewModel IngredientsVM { get; } = new();
        public ProductGroupsViewModel GroupsVM    { get; } = new();
        public ProductsViewModel    ProductsVM    { get; } = new();
        public DeliveriesViewModel  DeliveriesVM  { get; } = new();
        public DepartmentsViewModel DepartmentsVM { get; } = new();
        public OrdersViewModel      OrdersVM      { get; } = new();
        public SalesViewModel       SalesVM       { get; } = new();

        public ICommand NavSuppliersCommand   { get; }
        public ICommand NavIngredientsCommand { get; }
        public ICommand NavGroupsCommand      { get; }
        public ICommand NavProductsCommand    { get; }
        public ICommand NavDeliveriesCommand  { get; }
        public ICommand NavDepartmentsCommand { get; }
        public ICommand NavOrdersCommand      { get; }
        public ICommand NavSalesCommand       { get; }

        public MainViewModel()
        {
            NavSuppliersCommand   = new RelayCommand(() => Navigate(SuppliersVM,   "Поставщики"));
            NavIngredientsCommand = new RelayCommand(() => Navigate(IngredientsVM, "Склад (Ингредиенты)"));
            NavGroupsCommand      = new RelayCommand(() => Navigate(GroupsVM,      "Группы блюд "));
            NavProductsCommand    = new RelayCommand(() => Navigate(ProductsVM,    "Меню (Блюда)"));
            NavDeliveriesCommand  = new RelayCommand(() => Navigate(DeliveriesVM,  "Приход товара"));
            NavDepartmentsCommand = new RelayCommand(() => Navigate(DepartmentsVM, "Подразделения"));
            NavOrdersCommand      = new RelayCommand(() => Navigate(OrdersVM,      "Заказы"));
            NavSalesCommand       = new RelayCommand(() => Navigate(SalesVM,       "Учёт продаж"));

            Navigate(ProductsVM, "Меню (Блюда)");
        }

        private void Navigate(object vm, string title)
        {
            CurrentView  = vm;
            CurrentTitle = title;
        }
    }
}
