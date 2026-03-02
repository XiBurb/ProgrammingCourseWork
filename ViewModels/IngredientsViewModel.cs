using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class IngredientsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Ingredient> Ingredients => AppDatabase.Instance.Ingredients;
        public ObservableCollection<Supplier>   Suppliers   => AppDatabase.Instance.Suppliers;

        private Ingredient? _selected;
        public Ingredient? Selected
        {
            get => _selected;
            set { _selected = value; OnPropertyChanged(); IsEditing = value != null; }
        }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public ICommand AddCommand    { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand   { get; }

        public IngredientsViewModel()
        {
            AddCommand    = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
            SaveCommand   = new RelayCommand(Save);
        }

        private void Add()
        {
            var i = new Ingredient { Id = AppDatabase.Instance.NextId(), Name = "Новый продукт", Unit = "кг" };
            Ingredients.Add(i);
            Selected = i;
        }

        private void Delete()
        {
            if (Selected != null)
            {
                Ingredients.Remove(Selected);
                Selected = null;
                AppDatabase.Instance.Save();
            }
        }

        private void Save() => AppDatabase.Instance.Save();
    }
}
