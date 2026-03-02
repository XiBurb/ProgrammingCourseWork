using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using CateringIS.Data;
using CateringIS.Helpers;
using CateringIS.Models;

namespace CateringIS.ViewModels
{
    public class DepartmentsViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string? n = null)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(n));

        public ObservableCollection<Department> Departments => AppDatabase.Instance.Departments;

        private Department? _selected;
        public Department? Selected { get => _selected; set { _selected = value; OnPropertyChanged(); IsEditing = value != null; } }

        private bool _isEditing;
        public bool IsEditing { get => _isEditing; set { _isEditing = value; OnPropertyChanged(); } }

        public ICommand AddCommand    { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SaveCommand   { get; }

        public DepartmentsViewModel()
        {
            AddCommand    = new RelayCommand(Add);
            DeleteCommand = new RelayCommand(Delete, () => Selected != null);
            SaveCommand   = new RelayCommand(Save);
        }

        private void Add()
        {
            var d = new Department { Id = AppDatabase.Instance.NextId(), Name = "Новое подразделение" };
            Departments.Add(d);
            Selected = d;
        }

        private void Delete()
        {
            if (Selected != null) { Departments.Remove(Selected); Selected = null; AppDatabase.Instance.Save(); }
        }

        private void Save() => AppDatabase.Instance.Save();
    }
}
