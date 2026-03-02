using System.Windows;
using System.Windows.Controls;
using CateringIS.Helpers;

namespace CateringIS.Views
{
    public partial class SuppliersView : UserControl
    {
        public SuppliersView() => InitializeComponent();

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Проверяем наличие ошибок валидации
            if (ValidationHelper.HasValidationErrors(this))
            {
                var errors = ValidationHelper.GetValidationErrors(this);
                var errorMessage = "Обнаружены ошибки в заполнении полей:\n\n" + 
                                  string.Join("\n", errors.GetRange(0, System.Math.Min(5, errors.Count)));
                
                if (errors.Count > 5)
                    errorMessage += $"\n\n... и ещё {errors.Count - 5} ошибок";

                MessageBox.Show(errorMessage, "Ошибка валидации", 
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // Если ошибок нет, вызываем команду сохранения
            if (DataContext is ViewModels.SuppliersViewModel vm)
                vm.SaveCommand.Execute(null);
        }
    }
}
