using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class SalesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<SalesRecord> SalesRecords => AppDatabase.Instance.SalesRecords;
        public ObservableCollection<Product>     Products     => AppDatabase.Instance.Products;

        private SalesRecord? _selected;
        public SalesRecord? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); IsEditing = value != null; } }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public ICommand AddCommand    { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand   { get; }

        public SalesViewModel()
        {
            AddCommand    = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
            SaveCommand   = new RelayCommand(Save);
        }

        private void Add()
        {
            var sr = new SalesRecord { Id = AppDatabase.Instance.NextId(), Date = DateTime.Today, Unit = "порций" };
            if (Products.Count > 0)
            {
                sr.ProductId   = Products[0].Id;
                sr.ProductName = Products[0].Name;
            }
            SalesRecords.Add(sr);
            Selected = sr;
        }

        private void Delete()
        {
            if (Selected != null) { SalesRecords.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save() => AppDatabase.Instance.Save();
    }
}
