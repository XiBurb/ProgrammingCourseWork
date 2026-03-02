using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class ProductGroupsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<ProductGroup> Groups => AppDatabase.Instance.ProductGroups;

        private ProductGroup? _selected;
        public ProductGroup? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); IsEditing = value != null; } }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public ICommand AddCommand    { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand   { get; }

        public ProductGroupsViewModel()
        {
            AddCommand    = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
            SaveCommand   = new RelayCommand(Save);
        }

        private void Add()
        {
            var g = new ProductGroup { Id = AppDatabase.Instance.NextId(), Name = "Новая группа" };
            Groups.Add(g);
            Selected = g;
        }

        private void Delete()
        {
            if (Selected != null) { Groups.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save() => AppDatabase.Instance.Save();
    }
}
