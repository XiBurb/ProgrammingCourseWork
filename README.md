# CateringIS — Информационная система предприятия общественного питания

Полнофункциональная система управления для кафе, столовых и ресторанов. Разработана на **C#** с использованием **WPF**, паттерна **MVVM** и **OOP** принципов.

---

## ✨ Основные возможности

### 📋 Меню (Блюда)
- Полная карточка блюда с **фотографией**
- Цена, выход (порция), группа блюда
- Описание технологии приготовления
- **Рецептура**: ингредиенты с брутто/нетто весом
- **Фото автоматически копируются** в папку приложения и сохраняются

### 🥦 Склад
- Учёт ингредиентов с остатками
- Наценка на каждый продукт
- Связь с поставщиками

### 🚚 Приход товара
- Регистрация поставок от поставщиков
- **Автоматическое обновление остатков** на складе при сохранении

### 📦 Заказы подразделений
- Заказы от кухни, бара, кондитерского цеха
- **Автоматическое списание** со склада

### 🏢 Справочники
- **Поставщики**: полная информация (банк, счёт, ИНН, менеджер)
- **Подразделения**: кухня, бар и др.
- **Группы блюд**: закуски, первые, вторые, десерты и т.д.

### 📊 Учёт продаж
- Ежедневная запись объёмов продаж по блюдам
- Аналитика востребованности позиций

---

## 💾 Хранение данных

### Автоматическое сохранение
Все данные **автоматически сохраняются в JSON** при каждом изменении и при закрытии приложения:

**Расположение данных:**
```
<папка_с_программой>/Data/
  ├── suppliers.json       ← Поставщики
  ├── ingredients.json     ← Склад
  ├── groups.json          ← Группы блюд
  ├── products.json        ← Меню с рецептами
  ├── deliveries.json      ← Приходы
  ├── departments.json     ← Подразделения
  ├── orders.json          ← Заказы
  ├── sales.json           ← Продажи
  ├── meta.json            ← Служебные данные
  └── Photos/              ← Фотографии блюд
       ├── product_1_20250220_143052.jpg
       ├── product_2_20250220_143125.png
       └── ...
```

**Где находится папка Data:**
- При разработке: `CateringIS/bin/Debug/net8.0-windows/Data/`
- При публикации: рядом с `CateringIS.exe`

### Фотографии блюд
При добавлении фото:
1. Файл **копируется** в `%AppData%\CateringIS\Photos\`
2. Получает уникальное имя: `product_{id}_{дата-время}.{расширение}`
3. Путь сохраняется в базу данных
4. **Фото не теряются** при перемещении исходных файлов

### Первый запуск
При первом запуске автоматически создаётся папка `Data/` рядом с программой и в ней:
- Стандартные группы блюд (Закуски, Первые блюда, Вторые блюда, Десерты, Напитки, Кондитерские изделия, Выпечка)
- Стандартные подразделения (Кухня, Бар, Кондитерский цех)

---

## 🛠 Технические детали

### Архитектура
```
┌─────────────────────────────────────────┐
│  Views (XAML)                           │  ← Пользовательский интерфейс
│  • MainWindow                           │
│  • SuppliersView, ProductsView, etc.    │
└──────────────┬──────────────────────────┘
               │ DataBinding
┌──────────────▼──────────────────────────┐
│  ViewModels                             │  ← Логика представления
│  • MainViewModel (навигация)            │
│  • Каждый раздел имеет свой ViewModel   │
└──────────────┬──────────────────────────┘
               │ Commands & Properties
┌──────────────▼──────────────────────────┐
│  Models                                 │  ← Модели данных
│  • Supplier, Ingredient, Product...     │
│  • ObservableObject (INotifyPropertyChanged)
└──────────────┬──────────────────────────┘
               │
┌──────────────▼──────────────────────────┐
│  Data Layer                             │  ← Персистентность
│  • AppDatabase (Singleton)              │
│  • JSON сериализация                    │
│  • Автосохранение                       │
└─────────────────────────────────────────┘
```

### Используемые паттерны
- **MVVM** (Model-View-ViewModel) — полное разделение UI и логики
- **Singleton** — единственный экземпляр базы данных
- **Repository** — централизованный доступ к данным
- **Observer** — реактивное обновление UI через `INotifyPropertyChanged`
- **Command** — команды для действий пользователя

### Ключевые компоненты
- **RelayCommand**: универсальная реализация `ICommand`
- **Converters**: преобразования данных для XAML (bool→Visibility, decimal→string и др.)
- **DataTemplates**: автоматическая маршрутизация View для каждого ViewModel

---

## 🚀 Запуск проекта

### Требования
- **.NET 8 SDK** или новее
- Windows 10/11
- Visual Studio 2022 (опционально) или VS Code

### Компиляция и запуск

**Через командную строку:**
```bash
cd CateringIS
dotnet restore
dotnet run
```

**Через Visual Studio:**
1. Открыть `CateringIS.csproj`
2. Нажать F5 или "Запуск"

**Публикация exe-файла:**
```bash
dotnet publish -c Release -r win-x64 --self-contained
```
Готовый exe будет в `bin\Release\net8.0-windows\win-x64\publish\`

---

## 📦 Структура проекта

```
CateringIS/
├── CateringIS.csproj      ← Файл проекта
├── app.ico                ← Иконка приложения
│
├── App.xaml               ← Глобальные стили и ресурсы
├── App.xaml.cs            ← Точка входа
│
├── Models/
│   └── Models.cs          ← Все модели (Supplier, Product, Order и др.)
│
├── Data/
│   └── AppDatabase.cs     ← Репозиторий + JSON-персистентность
│
├── ViewModels/
│   ├── MainViewModel.cs         ← Навигация
│   ├── SuppliersViewModel.cs
│   ├── IngredientsViewModel.cs
│   ├── ProductsViewModel.cs     ← Работа с фото
│   ├── DeliveriesViewModel.cs   ← Обновление остатков
│   ├── OrdersViewModel.cs       ← Списание со склада
│   └── ...
│
├── Views/
│   ├── MainWindow.xaml          ← Главное окно с меню
│   ├── SuppliersView.xaml
│   ├── ProductsView.xaml        ← Карточка блюда + рецептура
│   └── ...
│
├── Helpers/
│   └── RelayCommand.cs          ← ICommand реализация
│
└── Converters/
    └── Converters.cs            ← Value converters
```

---

## 💡 Особенности реализации

### 🛡️ Валидация данных
**Автоматическая проверка вводимых данных:**
- **Текстовые поля** (названия): должны содержать буквы, не могут быть только цифрами
- **Числовые поля** (цены, количество): только положительные числа, без букв
- **Проценты**: от 0 до 100
- **Телефоны**: только цифры и символы +, -, (, )
- **Визуальная индикация**: красная рамка + текст ошибки

Подробнее см. [`VALIDATION.md`](VALIDATION.md)

### ⚙️ Конфигурационный файл
**Файл `app.conf` позволяет настроить:**
- Название главного окна
- Путь к иконке приложения

**Без перекомпиляции!** Просто отредактируйте `app.conf` и перезапустите программу.

Пример:
```ini
WindowTitle=Кафе "Солнышко" - Система учёта
WindowIcon=my_icon.ico
```

### Автосохранение
```csharp
// В App.xaml.cs
protected override void OnExit(ExitEventArgs e)
{
    AppDatabase.Instance.Save();  // Сохранение при выходе
    base.OnExit(e);
}

// В каждом ViewModel
private void Save() => AppDatabase.Instance.Save();
```

### Управление фото
```csharp
// ProductsViewModel.cs
private void BrowsePhoto()
{
    // 1. Выбор файла
    // 2. Копирование в Photos/
    // 3. Уникальное имя: product_{id}_{timestamp}
    // 4. Сохранение пути
}
```

### Обновление остатков при приходе
```csharp
// DeliveriesViewModel.cs
private void Save()
{
    foreach (var item in DeliveryItems)
    {
        var ing = FindIngredient(item.IngredientId);
        ing.StockBalance += item.Quantity;  // Прибавляем
    }
    AppDatabase.Instance.Save();
}
```

### Списание при заказе
```csharp
// OrdersViewModel.cs
private void Save()
{
    foreach (var item in OrderItems)
    {
        var ing = FindIngredient(item.IngredientId);
        ing.StockBalance -= item.Quantity;  // Вычитаем
    }
    AppDatabase.Instance.Save();
}
```

---

## 📝 Соответствие требованиям ТЗ

| № | Требование | Статус | Реализация |
|---|-----------|--------|-----------|
| 1 | Группы блюд | ✅ | `ProductGroup` + `ProductGroupsView` |
| 2 | Полная карточка блюда | ✅ | `Product` с фото, описанием, выходом, ценой |
| 2 | Фото блюда | ✅ | Копирование в Photos/, сохранение пути |
| 2 | Рецептура (брутто/нетто) | ✅ | `List<RecipeItem>` в каждом `Product` |
| 3 | Склад с наценкой | ✅ | `Ingredient` + `PriceMarkupPercent` |
| 3 | Остатки на складе | ✅ | `StockBalance` + авто-обновление |
| 4 | Поставщики | ✅ | `Supplier` (банк, счёт, ИНН, менеджер) |
| 5 | Приход товара | ✅ | `Delivery` + `DeliveryItem` + обновление остатков |
| 6 | Заказы подразделений | ✅ | `Order` + `OrderItem` + списание остатков |
| 7 | Учёт продаж | ✅ | `SalesRecord` по дням и блюдам |
| - | **C#** | ✅ | .NET 8 / C# 12 |
| - | **OOP** | ✅ | Классы, наследование, инкапсуляция |
| - | **MVVM** | ✅ | Полное разделение View-ViewModel-Model |
| - | **WPF** | ✅ | XAML + DataBinding + Commands |
| - | **Персистентность** | ✅ | JSON-файлы в AppData |
| - | **Иконка приложения** | ✅ | app.ico |

---

## 🔄 Расширение системы

### Добавление новой сущности
1. **Модель** в `Models/Models.cs`:
   ```csharp
   public class NewEntity : ObservableObject
   {
       private string _name = "";
       public string Name { get => _name; set => Set(ref _name, value); }
   }
   ```

2. **Коллекция** в `AppDatabase.cs`:
   ```csharp
   public ObservableCollection<NewEntity> NewEntities { get; set; } = new();
   ```

3. **ViewModel** в `ViewModels/`:
   ```csharp
   public class NewEntityViewModel : INotifyPropertyChanged { ... }
   ```

4. **View** в `Views/`:
   ```xaml
   <UserControl x:Class="...NewEntityView">
     <DataGrid ItemsSource="{Binding NewEntities}" .../>
   </UserControl>
   ```

5. **DataTemplate** в `App.xaml`:
   ```xaml
   <DataTemplate DataType="{x:Type vm:NewEntityViewModel}">
     <v:NewEntityView/>
   </DataTemplate>
   ```

---

## 📧 Контакты и поддержка

Проект создан как учебная работа по дисциплине "Программирование".

**Автор**: [Ваше имя]  
**Группа**: [Номер группы]  
**Год**: 2025

---

## 📄 Лицензия

Образовательный проект. Свободное использование в учебных целях.
