using System.Windows;
using System.Windows.Controls;
using CateringIS.Helpers;

namespace CateringIS.Views
{
    public partial class ProductGroupsView : UserControl
    {
        public ProductGroupsView() => InitializeComponent();

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

            if (DataContext is ViewModels.ProductGroupsViewModel vm)
                vm.SaveCommand.Execute(null);
        }
    }
}
