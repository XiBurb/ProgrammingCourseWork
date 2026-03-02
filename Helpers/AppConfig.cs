using System;
using System.Collections.Generic;
using System.IO;

namespace CateringIS.Helpers
{
    public class AppConfig
    {
        private static AppConfig? _instance;
        public static AppConfig Instance => _instance ??= new AppConfig();

        private Dictionary<string, string> _settings = new();

        // Настройки окна
        public string WindowTitle { get; private set; } = "Информационная система предприятия общественного питания";
        public string WindowIcon { get; private set; } = "app.ico";

        private AppConfig()
        {
            Load();
        }

        private void Load()
        {
            try
            {
                var configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.conf");
                
                if (!File.Exists(configPath))
                {
                    CreateDefaultConfig(configPath);
                    return;
                }

                var lines = File.ReadAllLines(configPath);
                foreach (var line in lines)
                {
                    // Пропускаем комментарии и пустые строки
                    if (string.IsNullOrWhiteSpace(line) || line.TrimStart().StartsWith("#"))
                        continue;

                    var parts = line.Split('=', 2);
                    if (parts.Length == 2)
                    {
                        var key = parts[0].Trim();
                        var value = parts[1].Trim();
                        _settings[key] = value;
                    }
                }

                // Применяем настройки
                if (_settings.TryGetValue("WindowTitle", out var title))
                    WindowTitle = title;

                if (_settings.TryGetValue("WindowIcon", out var icon))
                    WindowIcon = icon;
            }
            catch
            {
                // При ошибке используем значения по умолчанию
            }
        }

        private void CreateDefaultConfig(string path)
        {
            var defaultConfig = @"# Конфигурационный файл CateringIS
# Изменяйте значения после знака '='

# Название главного окна
WindowTitle=Информационная система предприятия общественного питания

# Путь к иконке (относительно папки с программой)
WindowIcon=app.ico

# Примеры других названий:
# WindowTitle=Система учёта для кафе ""Солнышко""
# WindowTitle=Столовая №5 - Учётная система
# WindowIcon=custom_icon.ico
";
            try
            {
                File.WriteAllText(path, defaultConfig);
            }
            catch
            {
                // Не критично если не удалось создать
            }
        }
    }
}
