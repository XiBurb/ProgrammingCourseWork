using System.Windows;
using CateringIS.Helpers;

namespace CateringIS.Views
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            
            // Применяем настройки из конфига
            var config = AppConfig.Instance;
            Title = config.WindowTitle;
            
            // Пытаемся загрузить иконку из конфига
            try
            {
                var iconPath = System.IO.Path.Combine(
                    System.AppDomain.CurrentDomain.BaseDirectory,
                    config.WindowIcon);
                    
                if (System.IO.File.Exists(iconPath))
                {
                    Icon = new System.Windows.Media.Imaging.BitmapImage(
                        new System.Uri(iconPath, System.UriKind.Absolute));
                }
            }
            catch
            {
                // Если не удалось загрузить, используем дефолтную иконку из ресурсов
            }
        }
    }
}
