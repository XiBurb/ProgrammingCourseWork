using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace CateringIS.Helpers
{
    /// <summary>
    /// Проверка текстовых полей: должны содержать буквы, могут содержать цифры, но не только цифры
    /// </summary>
    public class NameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Поле не может быть пустым");

            // Только цифры - недопустимо
            if (Regex.IsMatch(str, @"^\d+$"))
                return new ValidationResult(false, "Название не может состоять только из цифр");

            // Должна быть хотя бы одна буква
            if (!Regex.IsMatch(str, @"[а-яА-ЯёЁa-zA-Z]"))
                return new ValidationResult(false, "Название должно содержать буквы");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Проверка положительных чисел: > 0, без букв и спецсимволов
    /// </summary>
    public class PositiveNumberValidationRule : ValidationRule
    {
        public bool AllowZero { get; set; } = false;

        public string FieldName { get; set; } = "Значение";

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Поле не может быть пустым");

            // Проверка на недопустимые символы
            if (Regex.IsMatch(str, @"[а-яА-ЯёЁa-zA-Z!@#$%^&*()_+=\[\]{};:'""|\\<>?/]"))
                return new ValidationResult(false, $"{FieldName} должно быть больше нуля");

            // Попытка преобразовать в decimal
            if (!decimal.TryParse(str, NumberStyles.Any, cultureInfo, out decimal number))
                return new ValidationResult(false, "Некорректное числовое значение");

            // Проверка на отрицательное
            if (number < 0)
                return new ValidationResult(false, "Значение не может быть отрицательным");

            // Проверка на ноль
            if (!AllowZero && number == 0)
                return new ValidationResult(false, "Значение должно быть больше нуля");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Проверка процентов: 0 до 100, без букв
    /// </summary>
    public class PercentValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Поле не может быть пустым");

            if (!decimal.TryParse(str, NumberStyles.Any, cultureInfo, out decimal number))
                return new ValidationResult(false, "Некорректное числовое значение");

            if (number < 0 || number > 100)
                return new ValidationResult(false, "Процент должен быть от 0 до 100");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Проверка телефона: только цифры, +, -, (), пробелы
    /// </summary>
    public class PhoneValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return ValidationResult.ValidResult; // Телефон может быть пустым

            // Допустимые символы: цифры, +, -, (, ), пробелы
            if (!Regex.IsMatch(str, @"^[\d\s\+\-\(\)]+$"))
                return new ValidationResult(false, "Телефон может содержать только цифры, +, -, ( )");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Проверка поля "Выход": должно содержать текст и может содержать числа
    /// Например: "1 порция", "250 г", "300 мл"
    /// </summary>
    public class YieldValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return new ValidationResult(false, "Поле не может быть пустым");

            // Должна быть хотя бы одна буква
            if (!Regex.IsMatch(str, @"[а-яА-ЯёЁa-zA-Z]"))
                return new ValidationResult(false, "Выход должен содержать единицу измерения (порция, г, мл и т.д.)");

            // Проверяем на отрицательные числа
            if (Regex.IsMatch(str, @"-\d"))
                return new ValidationResult(false, "Значение не может быть отрицательным");

            return ValidationResult.ValidResult;
        }
    }

    /// <summary>
    /// Проверка ИНН: только цифры, 10 или 12 символов
    /// </summary>
    public class TaxIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string str = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(str))
                return ValidationResult.ValidResult; // ИНН может быть пустым

            // Только цифры
            if (!Regex.IsMatch(str, @"^\d+$"))
                return new ValidationResult(false, "ИНН должен содержать только цифры");

            // Длина 10 или 12
            if (str.Length != 10 && str.Length != 12)
                return new ValidationResult(false, "ИНН должен содержать 10 или 12 цифр");

            return ValidationResult.ValidResult;
        }
    }
    public class UnitValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value?.ToString()?.Trim() ?? "";

            if (string.IsNullOrWhiteSpace(input))
                return new ValidationResult(false, "Введите единицу измерения");

            // Только буквы (русские + английские)
            if (!Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ\.]+$"))
                return new ValidationResult(false,
                    "Единица измерения должна содержать только буквы (и \\ или .)");

            return ValidationResult.ValidResult;
        }
    }
}
