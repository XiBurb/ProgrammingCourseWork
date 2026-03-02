using System;
using System.Windows.Input;

namespace CateringIS.Helpers
{
    public class RelayCommand : ICommand
    {
        private readonly Action<object?> _execute;
        private readonly Func<object?, bool>? _canExecute;

        public RelayCommand(Action<object?> execute, Func<object?, bool>? canExecute = null)
        {
            _execute    = execute;
            _canExecute = canExecute;
        }

        public RelayCommand(Action execute, Func<bool>? canExecute = null)
            : this(_ => execute(), canExecute is null ? null : _ => canExecute()) { }

        public event EventHandler? CanExecuteChanged
        {
            add    => CommandManager.RequerySuggested += value;
            remove => CommandManager.RequerySuggested -= value;
        }

        public bool CanExecute(object? parameter) => _canExecute?.Invoke(parameter) ?? true;
        public void Execute(object? parameter) => _execute(parameter);
    }

    /// <summary>
    /// Хелпер для проверки валидации в окне/контроле
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Проверяет наличие ошибок валидации в визуальном дереве
        /// </summary>
        public static bool HasValidationErrors(System.Windows.DependencyObject obj)
        {
            // Проверяем текущий элемент
            if (obj is System.Windows.Controls.TextBox textBox)
            {
                if (System.Windows.Controls.Validation.GetHasError(textBox))
                    return true;
            }

            // Рекурсивно проверяем дочерние элементы
            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                if (HasValidationErrors(child))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Получает все сообщения об ошибках валидации
        /// </summary>
        public static System.Collections.Generic.List<string> GetValidationErrors(System.Windows.DependencyObject obj)
        {
            var errors = new System.Collections.Generic.List<string>();

            if (obj is System.Windows.Controls.TextBox textBox)
            {
                if (System.Windows.Controls.Validation.GetHasError(textBox))
                {
                    var validationErrors = System.Windows.Controls.Validation.GetErrors(textBox);
                    foreach (var error in validationErrors)
                    {
                        if (error.ErrorContent != null)
                            errors.Add(error.ErrorContent.ToString() ?? "");
                    }
                }
            }

            for (int i = 0; i < System.Windows.Media.VisualTreeHelper.GetChildrenCount(obj); i++)
            {
                var child = System.Windows.Media.VisualTreeHelper.GetChild(obj, i);
                errors.AddRange(GetValidationErrors(child));
            }

            return errors;
        }
    }
}
