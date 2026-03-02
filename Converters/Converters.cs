using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace CateringIS.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type t, object? p, CultureInfo c)
            => value is null || (value is string s && string.IsNullOrEmpty(s))
               ? Visibility.Collapsed : Visibility.Visible;
        public object ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotImplementedException();
    }

    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object? value, Type t, object? p, CultureInfo c)
            => value is true ? Visibility.Visible : Visibility.Collapsed;
        public object ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotImplementedException();
    }

    public class PathToImageConverter : IValueConverter
    {
        public object? Convert(object? value, Type t, object? p, CultureInfo c)
        {
            if (value is string path && !string.IsNullOrEmpty(path))
            {
                try
                {
                    var img = new BitmapImage();
                    img.BeginInit();
                    img.CacheOption = BitmapCacheOption.OnLoad;
                    img.UriSource   = new Uri(path, UriKind.Absolute);
                    img.EndInit();
                    return img;
                }
                catch { }
            }
            return null;
        }
        public object ConvertBack(object? v, Type t, object? p, CultureInfo c) => throw new NotImplementedException();
    }

    public class DecimalFormatConverter : IValueConverter
    {
        public object Convert(object? value, Type t, object? p, CultureInfo c)
            => value is decimal d ? d.ToString("N2", c) : "0.00";
        public object ConvertBack(object? v, Type t, object? p, CultureInfo c)
            => decimal.TryParse(v?.ToString(), NumberStyles.Any, c, out var d) ? d : 0m;
    }
}
