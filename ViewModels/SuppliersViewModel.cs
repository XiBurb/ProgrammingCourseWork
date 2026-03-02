using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class SuppliersViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Supplier> Suppliers => AppDatabase.Instance.Suppliers;

        private Supplier? _selected;
        public Supplier? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); IsEditing = value != null; } }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public ICommand AddCommand    { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand   { get; }

        public SuppliersViewModel()
        {
            AddCommand    = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
            SaveCommand   = new RelayCommand(Save);
        }

        private void Add()
        {
            var s = new Supplier { Id = AppDatabase.Instance.NextId(), Name = "Новый поставщик" };
            Suppliers.Add(s);
            Selected = s;
        }

        private void Delete()
        {
            if (Selected != null)
            {
                Suppliers.Remove(Selected);
                Selected = null;
                AppDatabase.Instance.Save();
            }
        }

        private void Save()
        {
            AppDatabase.Instance.Save();
        }
    }
}
