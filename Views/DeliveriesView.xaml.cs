using CateringIS.Helpers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
namespace CateringIS.Views {
    public partial class DeliveriesView : UserControl 
    { 
        public DeliveriesView() => InitializeComponent();
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Keyboard.ClearFocus();
            if (ValidationHelper.HasValidationErrors(this))
            {
                var errors = ValidationHelper.GetValidationErrors(this);
                MessageBox.Show("Обнаружены ошибки в заполнении полей:\n\n" +
                               string.Join("\n", errors),
                               "Ошибка валидации",
                               MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            if (DataContext is ViewModels.DeliveriesViewModel vm)
                vm.SaveCommand.Execute(null);
        }

    } 

}
