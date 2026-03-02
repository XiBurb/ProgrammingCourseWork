using System.Windows;
using System.Windows.Controls;
using CateringIS.Helpers;

namespace CateringIS.Views
{
    public partial class DepartmentsView : UserControl
    {
        public DepartmentsView() => InitializeComponent();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (ValidationHelper.HasValidationErrors(this))
            {
                var errors = ValidationHelper.GetValidationErrors(this);
                MessageBox.Show("Обнаружены ошибки в заполнении полей:\n\n" + 
                               string.Join("\n", errors), 
                               "Ошибка валидации", 
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (DataContext is ViewModels.DepartmentsViewModel vm)
                vm.SaveCommand.Execute(null);
        }
    }
}
