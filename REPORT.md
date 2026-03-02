# ОТЧЕТ О ПРОДЕЛАННОЙ РАБОТЕ
## Информационная система предприятия общественного питания

**Проект:** CateringIS  
**Версия:** 1.3  
**Дата:** Февраль 2026  
**Технологии:** C#, WPF, .NET 8, MVVM

---

## 1. ОПИСАНИЕ КЛАССОВ

### 1.1. Класс Supplier (Поставщик)

#### 1.1.1. Назначение класса
Класс Supplier представляет юридическое лицо или предпринимателя, осуществляющего поставки продуктов питания для предприятия общественного питания. Класс инкапсулирует все атрибуты поставщика, включая банковские реквизиты и контактные данные, обеспечивая их целостность в соответствии с бизнес-правилами системы.

#### 1.1.2. Данные-элементы класса (приватные поля)
- `_id` (int) — уникальный идентификатор поставщика
- `_name` (string) — наименование организации
- `_address` (string) — юридический/фактический адрес
- `_managerFullName` (string) — ФИО ответственного лица
- `_managerPhone` (string) — контактный телефон менеджера
- `_bankName` (string) — наименование банка
- `_bankAccount` (string) — расчетный счет
- `_taxId` (string) — ИНН (идентификационный номер налогоплательщика)

#### 1.1.3. Операции и функции-утилиты
Валидация осуществляется через WPF ValidationRules:
- **NameValidationRule** — проверяет наименование на содержание букв (не только цифры)
- **PhoneValidationRule** — проверяет формат телефона (цифры, +, -, (, ))
- **TaxIdValidationRule** — проверяет ИНН (10 или 12 цифр)

#### 1.1.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;) — уникальный идентификатор
- `Name` (string, get; set;) — наименование поставщика
- `Address` (string, get; set;) — адрес
- `ManagerFullName` (string, get; set;) — ФИО менеджера
- `ManagerPhone` (string, get; set;) — телефон менеджера
- `BankName` (string, get; set;) — название банка
- `BankAccount` (string, get; set;) — расчетный счет
- `TaxId` (string, get; set;) — ИНН

**Наследование:**
- Базовый класс: `ObservableObject` (реализует INotifyPropertyChanged)

---

### 1.2. Класс Ingredient (Ингредиент склада)

#### 1.2.1. Назначение класса
Класс Ingredient моделирует продукт питания, хранящийся на складе предприятия и используемый в качестве компонента для приготовления блюд. Класс отвечает за учет остатков, связь с поставщиком и ценообразование через наценку.

#### 1.2.2. Данные-элементы класса
- `_id` (int) — уникальный идентификатор
- `_name` (string) — наименование продукта
- `_unit` (string) — единица измерения (кг, л, шт и т.д.)
- `_priceMarkupPercent` (decimal) — процент наценки на закупочную цену
- `_stockBalance` (decimal) — текущий остаток на складе
- `_supplierId` (int) — ID поставщика
- `_supplier` (Supplier?) — объектная ссылка на поставщика

#### 1.2.3. Операции и функции-утилиты
Валидация через ValidationRules:
- **NameValidationRule** — проверка названия (должно содержать буквы)
- **PercentValidationRule** — проверка наценки (0-100%)
- **PositiveNumberValidationRule** — проверка остатка (≥0, допускается нулевое значение)

#### 1.2.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `Name` (string, get; set;)
- `Unit` (string, get; set;)
- `PriceMarkupPercent` (decimal, get; set;)
- `StockBalance` (decimal, get; set;)
- `SupplierId` (int, get; set;)
- `Supplier` (Supplier?, get; set;) — навигационное свойство

**Наследование:**
- Базовый класс: `ObservableObject`

---

### 1.3. Класс ProductGroup (Группа блюд)

#### 1.3.1. Назначение класса
Класс ProductGroup представляет категорию блюд в меню (закуски, первые блюда, десерты и т.д.). Обеспечивает классификацию и группировку продукции для удобства учета и навигации.

#### 1.3.2. Данные-элементы класса
- `_id` (int) — уникальный идентификатор группы
- `_name` (string) — название группы

#### 1.3.3. Операции и функции-утилиты
- **NameValidationRule** — проверка названия (не только цифры)

#### 1.3.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `Name` (string, get; set;)

**Наследование:**
- Базовый класс: `ObservableObject`

---

### 1.4. Класс RecipeItem (Элемент рецептуры)

#### 1.4.1. Назначение класса
Класс RecipeItem представляет один ингредиент в составе рецепта блюда, с указанием весовых характеристик до и после первичной обработки.

#### 1.4.2. Данные-элементы класса
- `_ingredientId` (int) — ID ингредиента из справочника
- `_ingredientName` (string) — название ингредиента (денормализация для производительности)
- `_grossWeight` (decimal) — масса брутто (до обработки)
- `_netWeight` (decimal) — масса нетто (после обработки)

#### 1.4.3. Операции и функции-утилиты
Валидация весов осуществляется на уровне пользовательского интерфейса.

#### 1.4.4. Интерфейс класса
**Свойства:**
- `IngredientId` (int, get; set;)
- `IngredientName` (string, get; set;)
- `GrossWeight` (decimal, get; set;)
- `NetWeight` (decimal, get; set;)

**Наследование:**
- Базовый класс: `ObservableObject`

---

### 1.5. Класс Product (Блюдо)

#### 1.5.1. Назначение класса
Класс Product моделирует готовое блюдо в меню предприятия. Содержит полную информацию о блюде: стоимость, выход, технологию приготовления, фотографию и детальную рецептуру.

#### 1.5.2. Данные-элементы класса
- `_id` (int) — уникальный идентификатор блюда
- `_name` (string) — название блюда
- `_groupId` (int) — ID группы блюд
- `_group` (ProductGroup?) — навигационное свойство к группе
- `_cost` (decimal) — цена продажи
- `_yield` (string) — выход (например: "1 порция", "250 г")
- `_preparationDescription` (string) — краткое описание технологии приготовления
- `_photoPath` (string) — путь к файлу фотографии блюда
- `_recipe` (List<RecipeItem>) — состав рецепта

#### 1.5.3. Операции и функции-утилиты
Валидация:
- **NameValidationRule** — проверка названия
- **PositiveNumberValidationRule** — проверка цены (>0)
- **YieldValidationRule** — проверка выхода (должен содержать единицу измерения и может содержать числа, но не отрицательные)

Специальные функции:
- Копирование фотографии в папку приложения при добавлении
- Генерация уникального имени файла: `product_{id}_{timestamp}.{ext}`

#### 1.5.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `Name` (string, get; set;)
- `GroupId` (int, get; set;)
- `Group` (ProductGroup?, get; set;)
- `Cost` (decimal, get; set;)
- `Yield` (string, get; set;)
- `PreparationDescription` (string, get; set;)
- `PhotoPath` (string, get; set;)
- `Recipe` (List<RecipeItem>, get; set;)

**Наследование:**
- Базовый класс: `ObservableObject`

---

### 1.6. Класс Delivery (Приход товара)

#### 1.6.1. Назначение класса
Класс Delivery представляет документ прихода товарно-материальных ценностей от поставщика. Обеспечивает регистрацию поступления продуктов и автоматическое обновление складских остатков.

#### 1.6.2. Данные-элементы класса
- `_id` (int) — уникальный номер прихода
- `_supplierId` (int) — ID поставщика
- `_supplier` (Supplier?) — навигационное свойство
- `_deliveryDate` (DateTime) — дата поставки
- `_items` (List<DeliveryItem>) — список товарных позиций

#### 1.6.3. Операции и функции-утилиты
**Алгоритм обновления остатков при сохранении:**
```
FOR EACH item IN DeliveryItems DO
    ingredient = FindIngredient(item.IngredientId)
    ingredient.StockBalance += item.Quantity
END FOR
Save()
```

#### 1.6.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `SupplierId` (int, get; set;)
- `Supplier` (Supplier?, get; set;)
- `DeliveryDate` (DateTime, get; set;)
- `Items` (List<DeliveryItem>, get; set;)

**Наследование:**
- Базовый класс: `ObservableObject`

---

### 1.7. Класс DeliveryItem (Элемент прихода)

#### 1.7.1. Назначение класса
Представляет одну товарную позицию в документе прихода с указанием закупочной цены и количества.

#### 1.7.2. Данные-элементы класса
- `_ingredientId` (int) — ID продукта
- `_ingredientName` (string) — название продукта
- `_unit` (string) — единица измерения
- `_purchasePrice` (decimal) — цена закупки
- `_quantity` (decimal) — количество

#### 1.7.3. Операции и функции-утилиты
Валидация через **PositiveNumberValidationRule** для цены и количества.

#### 1.7.4. Интерфейс класса
**Свойства:**
- `IngredientId` (int, get; set;)
- `IngredientName` (string, get; set;)
- `Unit` (string, get; set;)
- `PurchasePrice` (decimal, get; set;)
- `Quantity` (decimal, get; set;)

---

### 1.8. Класс Department (Подразделение)

#### 1.8.1. Назначение класса
Класс Department моделирует структурное подразделение предприятия (кухня, бар, кондитерский цех), которое может заказывать продукты со склада.

#### 1.8.2. Данные-элементы класса
- `_id` (int) — уникальный идентификатор
- `_name` (string) — название подразделения

#### 1.8.3. Операции и функции-утилиты
- **NameValidationRule** — проверка названия (не только цифры)

#### 1.8.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `Name` (string, get; set;)

---

### 1.9. Класс Order (Заказ подразделения)

#### 1.9.1. Назначение класса
Класс Order представляет заявку подразделения на получение продуктов со склада. Обеспечивает списание товаров при формировании заказа.

#### 1.9.2. Данные-элементы класса
- `_id` (int) — номер заказа
- `_departmentId` (int) — ID подразделения
- `_department` (Department?) — навигационное свойство
- `_orderDate` (DateTime) — дата заказа
- `_items` (List<OrderItem>) — список заказанных позиций

#### 1.9.3. Операции и функции-утилиты
**Алгоритм списания со склада при сохранении:**
```
FOR EACH item IN OrderItems DO
    ingredient = FindIngredient(item.IngredientId)
    ingredient.StockBalance -= item.Quantity
END FOR
Save()
```

#### 1.9.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `DepartmentId` (int, get; set;)
- `Department` (Department?, get; set;)
- `OrderDate` (DateTime, get; set;)
- `Items` (List<OrderItem>, get; set;)

---

### 1.10. Класс SalesRecord (Запись о продаже)

#### 1.10.1. Назначение класса
Класс SalesRecord обеспечивает учет объема продаж готовых блюд в разрезе дат для аналитики и планирования.

#### 1.10.2. Данные-элементы класса
- `_id` (int) — уникальный идентификатор записи
- `_date` (DateTime) — дата продажи
- `_productId` (int) — ID блюда
- `_productName` (string) — название блюда
- `_salesVolume` (decimal) — объем продаж (количество порций)
- `_unit` (string) — единица измерения

#### 1.10.3. Операции и функции-утилиты
Валидация через **PositiveNumberValidationRule** для объема продаж.

#### 1.10.4. Интерфейс класса
**Свойства:**
- `Id` (int, get; set;)
- `Date` (DateTime, get; set;)
- `ProductId` (int, get; set;)
- `ProductName` (string, get; set;)
- `SalesVolume` (decimal, get; set;)
- `Unit` (string, get; set;)

---

### 1.11. Класс ObservableObject (Базовый)

#### 1.11.1. Назначение класса
Абстрактный базовый класс, реализующий паттерн Observable для автоматического уведомления UI об изменениях в данных.

#### 1.11.2. Данные-элементы класса
Нет приватных полей (абстрактный класс).

#### 1.11.3. Операции и функции-утилиты
- `OnPropertyChanged([CallerMemberName] string? name)` — генерирует событие PropertyChanged
- `Set<T>(ref T field, T value, [CallerMemberName] string? name)` — устанавливает значение поля и генерирует событие при изменении

#### 1.11.4. Интерфейс класса
**Реализуемые интерфейсы:**
- `INotifyPropertyChanged` — стандартный интерфейс WPF для data binding

**События:**
- `PropertyChanged` — событие изменения свойства

---

## 2. ВЗАИМОСВЯЗЬ КЛАССОВ

### 2.1. UML-диаграмма классов модели данных

```
┌─────────────────────────┐
│   ObservableObject      │  ◄─── Базовый класс
│   (abstract)            │       для всех моделей
├─────────────────────────┤
│ + PropertyChanged       │
│ # OnPropertyChanged()   │
│ # Set<T>()              │
└─────────────────────────┘
           △
           │ наследование
           │
    ┌──────┴───────────────────────────────────────────────┐
    │                                                       │
┌───┴──────────────┐                            ┌──────────┴────────┐
│   Supplier       │                            │   Ingredient      │
├──────────────────┤                            ├───────────────────┤
│ - _id: int       │                            │ - _id: int        │
│ - _name: string  │◄───────────────────────────┤ - _supplierId     │
│ - _address       │        связь 1:N           │ - _supplier       │
│ - _taxId         │     (один поставщик —      │ - _name: string   │
│ - _bankName      │      много ингредиентов)   │ - _unit: string   │
│ - _managerPhone  │                            │ - _stockBalance   │
└──────────────────┘                            │ - _priceMarkup%   │
                                                └───────────────────┘
                                                         △
                                                         │ использование
                                                         │
                    ┌────────────────────────────────────┤
                    │                                    │
         ┌──────────┴─────────┐              ┌──────────┴────────┐
         │  DeliveryItem      │              │  OrderItem        │
         ├────────────────────┤              ├───────────────────┤
         │ - _ingredientId    │              │ - _ingredientId   │
         │ - _ingredientName  │              │ - _quantity       │
         │ - _purchasePrice   │              └───────────────────┘
         │ - _quantity        │                        △
         └────────────────────┘                        │
                  △                                    │
                  │                                    │
                  │ композиция                         │
                  │                                    │
         ┌────────┴────────┐                  ┌────────┴─────────┐
         │   Delivery      │                  │   Order          │
         ├─────────────────┤                  ├──────────────────┤
         │ - _id: int      │                  │ - _id: int       │
         │ - _supplierId   │                  │ - _departmentId  │
         │ - _deliveryDate │                  │ - _orderDate     │
         │ - _items: List  │                  │ - _items: List   │
         └─────────────────┘                  └──────────────────┘
                  │                                    │
                  │ ассоциация                         │ ассоциация
                  ▼                                    ▼
         ┌─────────────────┐                  ┌──────────────────┐
         │   Supplier      │                  │   Department     │
         │ (ссылка выше)   │                  ├──────────────────┤
         └─────────────────┘                  │ - _id: int       │
                                              │ - _name: string  │
                                              └──────────────────┘


┌──────────────────────┐
│   ProductGroup       │
├──────────────────────┤
│ - _id: int           │
│ - _name: string      │
└──────────────────────┘
         △
         │ связь 1:N
         │ (одна группа — много блюд)
         │
┌────────┴──────────────┐
│   Product             │
├───────────────────────┤
│ - _id: int            │
│ - _name: string       │
│ - _groupId: int       │
│ - _group: ProdGroup   │
│ - _cost: decimal      │
│ - _yield: string      │
│ - _prepDescription    │
│ - _photoPath: string  │
│ - _recipe: List       │◄──────┐
└───────────────────────┘       │ композиция
                                │ (рецепт принадлежит блюду)
                                │
                      ┌─────────┴─────────┐
                      │  RecipeItem       │
                      ├───────────────────┤
                      │ - _ingredientId   │
                      │ - _ingredientName │
                      │ - _grossWeight    │
                      │ - _netWeight      │
                      └───────────────────┘
                                │
                                │ ссылка на Ingredient
                                ▼
                      ┌───────────────────┐
                      │   Ingredient      │
                      │ (ссылка выше)     │
                      └───────────────────┘


┌──────────────────────┐
│   SalesRecord        │
├──────────────────────┤
│ - _id: int           │
│ - _date: DateTime    │
│ - _productId: int    │─────────► Product
│ - _productName       │           (ссылка на блюдо)
│ - _salesVolume       │
│ - _unit: string      │
└──────────────────────┘
```

### 2.2. UML-диаграмма архитектуры MVVM

```
┌─────────────────────────────────────────────────────────────────┐
│                         PRESENTATION LAYER                       │
│                                                                  │
│  ┌──────────────┐   ┌──────────────┐   ┌──────────────┐        │
│  │ MainWindow   │   │SuppliersView │   │ ProductsView │ ...    │
│  │   (XAML)     │   │   (XAML)     │   │   (XAML)     │        │
│  └──────┬───────┘   └──────┬───────┘   └──────┬───────┘        │
│         │                  │                   │                 │
│         │ DataBinding      │ DataBinding       │ DataBinding     │
│         ▼                  ▼                   ▼                 │
└─────────┼──────────────────┼───────────────────┼─────────────────┘
          │                  │                   │
┌─────────┼──────────────────┼───────────────────┼─────────────────┐
│         │            VIEW MODEL LAYER          │                 │
│         │                  │                   │                 │
│  ┌──────▼────────┐  ┌──────▼────────┐  ┌──────▼────────┐        │
│  │MainViewModel  │  │SuppliersVM    │  │ProductsVM     │        │
│  ├───────────────┤  ├───────────────┤  ├───────────────┤        │
│  │- CurrentView  │  │- Suppliers    │  │- Products     │        │
│  │- CurrentTitle │  │- Selected     │  │- Selected     │        │
│  │               │  │- AddCommand   │  │- RecipeItems  │        │
│  │- NavCommands  │  │- SaveCommand  │  │- SaveCommand  │        │
│  └───────────────┘  └───────────────┘  └───────────────┘        │
│         │                  │                   │                 │
│         │ использует       │                   │                 │
│         ▼                  ▼                   ▼                 │
└─────────┼──────────────────┼───────────────────┼─────────────────┘
          │                  │                   │
┌─────────┼──────────────────┼───────────────────┼─────────────────┐
│         │              MODEL LAYER             │                 │
│         │                  │                   │                 │
│  ┌──────▼──────────────────▼───────────────────▼──────┐          │
│  │           AppDatabase (Singleton)                  │          │
│  ├────────────────────────────────────────────────────┤          │
│  │ - Suppliers: ObservableCollection<Supplier>        │          │
│  │ - Ingredients: ObservableCollection<Ingredient>    │          │
│  │ - Products: ObservableCollection<Product>          │          │
│  │ - Orders, Deliveries, SalesRecords...              │          │
│  │                                                     │          │
│  │ + Save() : void                                    │          │
│  │ + Load() : void                                    │          │
│  │ + NextId() : int                                   │          │
│  └─────────────────────────────────────────────────────┘         │
│                            │                                     │
│                            │ работает с                          │
│                            ▼                                     │
│         ┌──────────────────────────────────┐                    │
│         │  JSON Files (Data/)              │                    │
│         ├──────────────────────────────────┤                    │
│         │ suppliers.json                   │                    │
│         │ ingredients.json                 │                    │
│         │ products.json                    │                    │
│         │ deliveries.json                  │                    │
│         │ ...                               │                    │
│         └──────────────────────────────────┘                    │
└─────────────────────────────────────────────────────────────────┘
```

### 2.3. Диаграмма последовательности: Сохранение блюда

```
Пользователь    ProductsView    ProductsViewModel    ValidationHelper    AppDatabase    JSON File
     │               │                  │                    │               │              │
     │  Нажать       │                  │                    │               │              │
     │ "Сохранить"   │                  │                    │               │              │
     ├──────────────►│                  │                    │               │              │
     │               │  SaveButton_     │                    │               │              │
     │               │    Click()       │                    │               │              │
     │               ├─────────────────►│                    │               │              │
     │               │                  │  HasValidation     │               │              │
     │               │                  │    Errors(this)    │               │              │
     │               │                  ├───────────────────►│               │              │
     │               │                  │                    │               │              │
     │               │                  │  [проверка полей]  │               │              │
     │               │                  │                    │               │              │
     │               │                  │◄───────────────────┤               │              │
     │               │                  │   true/false       │               │              │
     │               │                  │                    │               │              │
     │               │  [если ошибки]   │                    │               │              │
     │               │  MessageBox      │                    │               │              │
     │               │◄─────────────────┤                    │               │              │
     │◄──────────────┤                  │                    │               │              │
     │  Окно с       │                  │                    │               │              │
     │  ошибками     │                  │                    │               │              │
     │               │                  │                    │               │              │
     │               │  [если OK]       │                    │               │              │
     │               │  Save()          │                    │               │              │
     │               │                  ├───────────────────────────────────►│              │
     │               │                  │                    │               │  Save()      │
     │               │                  │                    │               ├─────────────►│
     │               │                  │                    │               │              │
     │               │                  │                    │               │◄─────────────┤
     │               │                  │◄───────────────────────────────────┤              │
     │               │◄─────────────────┤                    │               │              │
     │◄──────────────┤                  │                    │               │              │
     │  Успех        │                  │                    │               │              │
```

### 2.4. Типы связей между классами

| Связь | От класса | К классу | Тип | Множественность |
|-------|-----------|----------|-----|-----------------|
| Ассоциация | Ingredient | Supplier | Однонаправленная | N:1 |
| Ассоциация | Product | ProductGroup | Однонаправленная | N:1 |
| Композиция | Product | RecipeItem | Двунаправленная | 1:N |
| Композиция | Delivery | DeliveryItem | Двунаправленная | 1:N |
| Композиция | Order | OrderItem | Двунаправленная | 1:N |
| Ассоциация | Delivery | Supplier | Однонаправленная | N:1 |
| Ассоциация | Order | Department | Однонаправленная | N:1 |
| Ассоциация | SalesRecord | Product | Однонаправленная | N:1 |
| Наследование | Все модели | ObservableObject | Наследование | - |

---

## 3. ОПИСАНИЕ АЛГОРИТМОВ, ОТНОСЯЩИХСЯ К ПРЕДМЕТНОЙ ОБЛАСТИ

### 3.1. Алгоритмы валидации данных

#### 3.1.1. Валидация текстовых полей (названий)

**Назначение:** Обеспечение корректности наименований сущностей

**Класс:** `NameValidationRule`

**Алгоритм:**
```
FUNCTION ValidateName(value):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Error("Поле не может быть пустым")
    
    3. IF MatchRegex(str, "только цифры") THEN
         RETURN Error("Название не может состоять только из цифр")
    
    4. IF NOT ContainsLetters(str) THEN
         RETURN Error("Название должно содержать буквы")
    
    5. RETURN Success
END FUNCTION
```

**Применяется к полям:**
- Название поставщика
- Название ингредиента
- Название блюда
- Название группы блюд
- Название подразделения
- ФИО менеджера
- Адрес
- Банк

**Особенности:**
- Использует регулярные выражения для проверки наличия букв
- Проверяет как русские, так и латинские буквы: `[а-яА-ЯёЁa-zA-Z]`

---

#### 3.1.2. Валидация числовых полей

**Назначение:** Контроль корректности числовых значений (цены, количества)

**Класс:** `PositiveNumberValidationRule`

**Алгоритм:**
```
FUNCTION ValidatePositiveNumber(value, allowZero):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Error("Поле не может быть пустым")
    
    3. IF ContainsLettersOrSpecialChars(str) THEN
         RETURN Error("Поле должно содержать только числа")
    
    4. number = ParseDecimal(str)
    
    5. IF ParseFailed THEN
         RETURN Error("Некорректное числовое значение")
    
    6. IF number < 0 THEN
         RETURN Error("Значение не может быть отрицательным")
    
    7. IF NOT allowZero AND number == 0 THEN
         RETURN Error("Значение должно быть больше нуля")
    
    8. RETURN Success
END FUNCTION
```

**Применяется к полям:**
- Цена блюда (allowZero = false)
- Цена закупки (allowZero = false)
- Количество товара (allowZero = false)
- Остаток на складе (allowZero = true)
- Объем продаж (allowZero = false)

---

#### 3.1.3. Валидация процентов

**Назначение:** Проверка значений процентной наценки

**Класс:** `PercentValidationRule`

**Алгоритм:**
```
FUNCTION ValidatePercent(value):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Error("Поле не может быть пустым")
    
    3. number = ParseDecimal(str)
    
    4. IF ParseFailed THEN
         RETURN Error("Некорректное числовое значение")
    
    5. IF number < 0 OR number > 100 THEN
         RETURN Error("Процент должен быть от 0 до 100")
    
    6. RETURN Success
END FUNCTION
```

**Применяется к полям:**
- Наценка на ингредиент

---

#### 3.1.4. Валидация поля "Выход"

**Назначение:** Проверка корректности указания выхода блюда

**Класс:** `YieldValidationRule`

**Алгоритм:**
```
FUNCTION ValidateYield(value):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Error("Поле не может быть пустым")
    
    3. IF NOT ContainsLetters(str) THEN
         RETURN Error("Выход должен содержать единицу измерения (порция, г, мл и т.д.)")
    
    4. IF ContainsNegativeNumber(str) THEN
         RETURN Error("Значение не может быть отрицательным")
    
    5. RETURN Success
END FUNCTION
```

**Примеры корректных значений:**
- "1 порция"
- "250 г"
- "300 мл"
- "порция"

**Примеры некорректных значений:**
- "1" (нет единицы измерения)
- "-1 порция" (отрицательное)
- "" (пусто)

---

#### 3.1.5. Валидация ИНН

**Назначение:** Проверка идентификационного номера налогоплательщика

**Класс:** `TaxIdValidationRule`

**Алгоритм:**
```
FUNCTION ValidateTaxId(value):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Success  // ИНН может быть пустым
    
    3. IF NOT MatchRegex(str, "только цифры") THEN
         RETURN Error("ИНН должен содержать только цифры")
    
    4. IF Length(str) != 10 AND Length(str) != 12 THEN
         RETURN Error("ИНН должен содержать 10 или 12 цифр")
    
    5. RETURN Success
END FUNCTION
```

**Особенности:**
- ИНН юридического лица: 10 цифр
- ИНН физического лица: 12 цифр
- Поле может быть пустым

---

#### 3.1.6. Валидация телефона

**Назначение:** Проверка формата телефонного номера

**Класс:** `PhoneValidationRule`

**Алгоритм:**
```
FUNCTION ValidatePhone(value):
    1. str = Trim(value)
    
    2. IF IsNullOrWhiteSpace(str) THEN
         RETURN Success  // Телефон может быть пустым
    
    3. allowedChars = ["0-9", "+", "-", "(", ")", " "]
    
    4. FOR EACH char IN str DO
         IF char NOT IN allowedChars THEN
            RETURN Error("Телефон может содержать только цифры, +, -, ( )")
         END IF
       END FOR
    
    5. RETURN Success
END FUNCTION
```

**Примеры корректных значений:**
- "+7 (495) 123-45-67"
- "89161234567"
- "123-456"

---

### 3.2. Алгоритм умной генерации ID

**Назначение:** Генерация уникальных идентификаторов с переиспользованием освобожденных номеров

**Класс:** `AppDatabase`

**Метод:** `NextId()`

**Алгоритм:**
```
FUNCTION NextId():
    1. usedIds = CreateEmptySet()
    
    2. // Собрать все существующие ID из всех коллекций
    FOR EACH supplier IN Suppliers DO
        usedIds.Add(supplier.Id)
    END FOR
    
    FOR EACH ingredient IN Ingredients DO
        usedIds.Add(ingredient.Id)
    END FOR
    
    FOR EACH group IN ProductGroups DO
        usedIds.Add(group.Id)
    END FOR
    
    // ... аналогично для Products, Deliveries, Departments, Orders, SalesRecords
    
    3. // Найти первый свободный ID начиная с 1
    id = 1
    
    4. WHILE usedIds.Contains(id) DO
         id = id + 1
       END WHILE
    
    5. _nextId = id + 1  // Сохраняем следующий кандидат
    
    6. RETURN id
END FUNCTION
```

**Пример работы:**
```
Состояние: IDs = {1, 2, 4, 5, 6, 7}  (ID 3 удален)
Вызов NextId() → возвращает 3

Состояние: IDs = {1, 2, 3, 4, 5, 6, 7}
Вызов NextId() → возвращает 8
```

**Преимущества:**
- Нет пропусков в нумерации
- Компактные ID
- Переиспользование освободившихся номеров
- Предсказуемое поведение

---

### 3.3. Алгоритм автоматического обновления складских остатков

#### 3.3.1. При приходе товара

**Назначение:** Увеличение остатков при поступлении товара от поставщика

**Класс:** `DeliveriesViewModel`

**Метод:** `Save()`

**Алгоритм:**
```
FUNCTION SaveDelivery():
    1. IF Selected == null THEN
         RETURN
    
    2. // Сохранить структуру документа
    Selected.Items = List(DeliveryItems)
    Selected.SupplierId = Selected.Supplier.Id
    
    3. // Обновить остатки на складе
    FOR EACH item IN DeliveryItems DO
        ingredient = FindIngredient(item.IngredientId)
        
        IF ingredient != null THEN
           ingredient.StockBalance = ingredient.StockBalance + item.Quantity
        END IF
    END FOR
    
    4. AppDatabase.Save()
    
    5. ShowMessage("Приход сохранён. Остатки обновлены.")
END FUNCTION
```

**Пример:**
```
Было: Картофель - 50 кг
Приход: 30 кг
Стало: Картофель - 80 кг
```

---

#### 3.3.2. При оформлении заказа подразделения

**Назначение:** Списание продуктов со склада при формировании заказа

**Класс:** `OrdersViewModel`

**Метод:** `Save()`

**Алгоритм:**
```
FUNCTION SaveOrder():
    1. IF Selected == null THEN
         RETURN
    
    2. // Сохранить структуру документа
    Selected.Items = List(OrderItems)
    Selected.DepartmentId = Selected.Department.Id
    
    3. // Списать со склада
    FOR EACH item IN OrderItems DO
        ingredient = FindIngredient(item.IngredientId)
        
        IF ingredient != null THEN
           ingredient.StockBalance = ingredient.StockBalance - item.Quantity
        END IF
    END FOR
    
    4. AppDatabase.Save()
    
    5. ShowMessage("Заказ сохранён. Остатки обновлены.")
END FUNCTION
```

**Пример:**
```
Было: Мука - 100 кг
Заказ кухни: 25 кг
Стало: Мука - 75 кг
```

**Особенность:** Система не проверяет отрицательные остатки (может уйти в минус). Это бизнес-правило для отслеживания дефицита.

---

### 3.4. Алгоритм работы с фотографиями блюд

**Назначение:** Копирование фотографий в папку приложения с уникальным именованием

**Класс:** `ProductsViewModel`

**Метод:** `BrowsePhoto()`

**Алгоритм:**
```
FUNCTION BrowsePhoto():
    1. dialog = CreateOpenFileDialog()
    dialog.Filter = "Image files|*.jpg;*.jpeg;*.png;*.bmp;*.gif"
    
    2. IF dialog.ShowDialog() == true AND Selected != null THEN
       TRY
           // Создать папку Photos если её нет
           photosDir = ApplicationDataFolder + "/Photos"
           CreateDirectory(photosDir)
           
           // Сгенерировать уникальное имя
           ext = GetExtension(dialog.FileName)
           timestamp = CurrentDateTime.ToString("yyyyMMdd_HHmmss")
           newName = "product_" + Selected.Id + "_" + timestamp + ext
           
           // Полный путь назначения
           destPath = photosDir + "/" + newName
           
           // Копировать файл
           File.Copy(dialog.FileName, destPath, overwrite: true)
           
           // Сохранить путь в модели
           Selected.PhotoPath = destPath
           
       CATCH exception
           ShowError("Ошибка при копировании фото: " + exception.Message)
       END TRY
    END IF
END FUNCTION
```

**Пример:**
```
Исходный файл: C:\Users\User\Pictures\borsch.jpg
Скопирован в:  Data\Photos\product_15_20260220_143052.jpg
```

**Преимущества:**
- Фото не теряются при удалении исходного файла
- Уникальные имена исключают конфликты
- Все фото в одной папке для удобства

---

### 3.5. Алгоритм проверки валидации перед сохранением

**Назначение:** Предотвращение сохранения данных с ошибками валидации

**Класс:** `ValidationHelper`

**Метод:** `HasValidationErrors()`

**Алгоритм:**
```
FUNCTION HasValidationErrors(visualElement):
    1. // Проверить текущий элемент
    IF visualElement is TextBox THEN
       IF HasError(visualElement) THEN
          RETURN true
       END IF
    END IF
    
    2. // Рекурсивно проверить все дочерние элементы
    childCount = GetChildrenCount(visualElement)
    
    3. FOR i = 0 TO childCount - 1 DO
         child = GetChild(visualElement, i)
         
         IF HasValidationErrors(child) THEN
            RETURN true
         END IF
       END FOR
    
    4. RETURN false
END FUNCTION
```

**Использование:**
```
FUNCTION SaveButton_Click():
    1. IF HasValidationErrors(this) THEN
         errors = GetValidationErrors(this)
         errorMessage = "Обнаружены ошибки:\n" + Join(errors, "\n")
         ShowMessageBox(errorMessage, "Ошибка валидации", Information)
         RETURN  // Не сохранять!
    END IF
    
    2. // Валидация пройдена - сохранить
    ViewModel.SaveCommand.Execute()
END FUNCTION
```

---

### 3.6. Алгоритм сохранения и загрузки данных (JSON)

#### 3.6.1. Сохранение

**Назначение:** Сериализация всех данных в JSON-файлы

**Класс:** `AppDatabase`

**Метод:** `Save()`

**Алгоритм:**
```
FUNCTION Save():
    1. // Создать папку Data если её нет
    CreateDirectory(DataFolder)
    
    2. // Настройки сериализации
    settings.Formatting = Indented
    settings.NullValueHandling = Ignore
    
    3. // Сохранить каждую коллекцию в отдельный файл
    WriteFile("suppliers.json",    Serialize(Suppliers, settings))
    WriteFile("ingredients.json",  Serialize(Ingredients, settings))
    WriteFile("groups.json",       Serialize(ProductGroups, settings))
    WriteFile("products.json",     Serialize(Products, settings))
    WriteFile("deliveries.json",   Serialize(Deliveries, settings))
    WriteFile("departments.json",  Serialize(Departments, settings))
    WriteFile("orders.json",       Serialize(Orders, settings))
    WriteFile("sales.json",        Serialize(SalesRecords, settings))
    
    4. // Сохранить метаданные (счётчик ID)
    WriteFile("meta.json",         Serialize(_nextId))
END FUNCTION
```

---

#### 3.6.2. Загрузка

**Назначение:** Десериализация данных из JSON при запуске приложения

**Класс:** `AppDatabase`

**Метод:** `Load()`

**Алгоритм:**
```
FUNCTION Load():
    1. // Создать папки
    CreateDirectory(DataFolder)
    CreateDirectory(PhotosFolder)
    
    2. // Загрузить каждую коллекцию
    Suppliers     = LoadCollection<Supplier>("suppliers")
    Ingredients   = LoadCollection<Ingredient>("ingredients")
    ProductGroups = LoadCollection<ProductGroup>("groups")
    Products      = LoadCollection<Product>("products")
    Deliveries    = LoadCollection<Delivery>("deliveries")
    Departments   = LoadCollection<Department>("departments")
    Orders        = LoadCollection<Order>("orders")
    SalesRecords  = LoadCollection<SalesRecord>("sales")
    
    3. // Загрузить метаданные
    IF FileExists("meta.json") THEN
       _nextId = Deserialize<int>("meta.json")
    ELSE
       _nextId = 1
    END IF
    
    4. // Восстановить связи между объектами
    LinkReferences()
    
    5. // Создать начальные данные если база пуста
    IF ProductGroups.Count == 0 THEN
       SeedDefaults()
    END IF
END FUNCTION

FUNCTION LoadCollection<T>(fileName):
    filePath = DataFolder + "/" + fileName + ".json"
    
    IF NOT FileExists(filePath) THEN
       RETURN EmptyCollection<T>()
    END IF
    
    TRY
       json = ReadFile(filePath)
       list = Deserialize<List<T>>(json)
       RETURN ObservableCollection<T>(list)
    CATCH
       RETURN EmptyCollection<T>()
    END TRY
END FUNCTION
```

---

#### 3.6.3. Восстановление связей

**Назначение:** Восстановление навигационных свойств после десериализации

**Метод:** `LinkReferences()`

**Алгоритм:**
```
FUNCTION LinkReferences():
    1. // Создать словари для быстрого поиска
    supplierMap   = CreateDictionary(Suppliers, by: Id)
    ingredientMap = CreateDictionary(Ingredients, by: Id)
    groupMap      = CreateDictionary(ProductGroups, by: Id)
    departmentMap = CreateDictionary(Departments, by: Id)
    productMap    = CreateDictionary(Products, by: Id)
    
    2. // Восстановить связи Ingredient → Supplier
    FOR EACH ingredient IN Ingredients DO
        IF supplierMap.ContainsKey(ingredient.SupplierId) THEN
           ingredient.Supplier = supplierMap[ingredient.SupplierId]
        END IF
    END FOR
    
    3. // Восстановить связи Product → ProductGroup
    FOR EACH product IN Products DO
        IF groupMap.ContainsKey(product.GroupId) THEN
           product.Group = groupMap[product.GroupId]
        END IF
        
        // Восстановить названия ингредиентов в рецепте
        FOR EACH recipeItem IN product.Recipe DO
            IF ingredientMap.ContainsKey(recipeItem.IngredientId) THEN
               recipeItem.IngredientName = ingredientMap[recipeItem.IngredientId].Name
            END IF
        END FOR
    END FOR
    
    4. // Восстановить связи Delivery → Supplier
    FOR EACH delivery IN Deliveries DO
        IF supplierMap.ContainsKey(delivery.SupplierId) THEN
           delivery.Supplier = supplierMap[delivery.SupplierId]
        END IF
    END FOR
    
    5. // Восстановить связи Order → Department
    FOR EACH order IN Orders DO
        IF departmentMap.ContainsKey(order.DepartmentId) THEN
           order.Department = departmentMap[order.DepartmentId]
        END IF
    END FOR
    
    6. // Восстановить названия блюд в продажах
    FOR EACH salesRecord IN SalesRecords DO
        IF productMap.ContainsKey(salesRecord.ProductId) THEN
           salesRecord.ProductName = productMap[salesRecord.ProductId].Name
        END IF
    END FOR
END FUNCTION
```

---

## 4. ОПИСАНИЕ РАЗРАБОТАННОГО ПРИЛОЖЕНИЯ

### 4.1. Общие сведения

**Название:** CateringIS (Catering Information System)

**Назначение:** Автоматизация учета и управления на предприятиях общественного питания (кафе, столовые, рестораны, буфеты)

**Платформа:** Windows 10/11

**Технологический стек:**
- Язык: C# 12
- Framework: .NET 8
- UI: WPF (Windows Presentation Foundation)
- Паттерн: MVVM (Model-View-ViewModel)
- Сериализация: JSON (Newtonsoft.Json)
- Конфигурация: INI-подобный текстовый формат

**Размер проекта:**
- Файлов: 52
- Строк кода: ~3500+
- Классов моделей: 10
- ViewModels: 9
- Views: 8

---

### 4.2. Функциональные возможности

#### 4.2.1. Управление справочниками

**Поставщики:**
- Добавление, редактирование, удаление
- Полная информация: наименование, адрес, ИНН, банковские реквизиты
- Контактные данные менеджера
- Валидация всех полей

**Группы блюд:**
- Классификация блюд по категориям
- Стандартные группы: Закуски, Первые блюда, Вторые блюда, Десерты, Напитки, Кондитерские изделия, Выпечка
- Возможность создания собственных групп

**Подразделения:**
- Кухня, бар, кондитерский цех
- Любые пользовательские подразделения

---

#### 4.2.2. Управление складом

**Ингредиенты:**
- Учет продуктов питания
- Связь с поставщиками
- Единицы измерения (кг, л, шт и т.д.)
- Процент наценки
- Автоматический контроль остатков

**Приход товара:**
- Регистрация поставок от поставщиков
- Указание закупочной цены
- **Автоматическое обновление остатков** при сохранении
- История всех поставок

**Заказы подразделений:**
- Оформление заявок на получение продуктов со склада
- **Автоматическое списание** при сохранении
- Учет потребностей каждого подразделения

---

#### 4.2.3. Управление меню

**Карточка блюда:**
- Название блюда
- Группа (категория)
- Цена продажи
- Выход (порция, вес)
- Описание технологии приготовления
- **Фотография** (с автоматическим копированием в папку приложения)

**Рецептура:**
- Детальный состав ингредиентов
- Масса брутто (до обработки)
- Масса нетто (после обработки)
- Связь с складом ингредиентов

---

#### 4.2.4. Аналитика

**Учет продаж:**
- Ежедневная регистрация объемов продаж
- Учет по каждому блюду
- Количество проданных порций
- База для анализа спроса

---

### 4.3. Технические особенности

#### 4.3.1. Валидация данных

**Автоматическая проверка полей ввода:**

| Тип поля | Правила | Визуализация ошибки |
|----------|---------|---------------------|
| Текстовые (названия) | Должны содержать буквы, не только цифры | Красная рамка + текст |
| Числовые (цены) | > 0, без букв и спецсимволов | Красная рамка + текст |
| Проценты | 0-100 | Красная рамка + текст |
| Выход блюда | Должен содержать единицу измерения | Красная рамка + текст |
| ИНН | 10 или 12 цифр | Красная рамка + текст |
| Телефон | Цифры, +, -, (, ) | Красная рамка + текст |

**При попытке сохранить с ошибками:**
- Появляется информационное окно MessageBox
- Перечисляются все найденные ошибки
- Данные не сохраняются до исправления

---

#### 4.3.2. Умная система ID

**Особенность:** ID переиспользуются без пропусков

**Принцип работы:**
```
Создано: 1, 2, 3, 4, 5, 6, 7
Удалено: 3
Создано новое → ID = 3 (переиспользование!)
Создано еще → ID = 8
```

**Преимущества:**
- Компактная нумерация
- Нет пропусков
- Предсказуемое поведение

---

#### 4.3.3. Автоматическое управление остатками

**Приход товара:**
```
Было: Картофель - 50 кг
Приход: +30 кг
Стало: 80 кг (автоматически)
```

**Заказ подразделения:**
```
Было: Мука - 100 кг
Заказ: -25 кг
Стало: 75 кг (автоматически)
```

**Особенность:** Система допускает отрицательные остатки для отслеживания дефицита

---

#### 4.3.4. Работа с фотографиями

**Алгоритм:**
1. Пользователь выбирает фото через диалог
2. Файл копируется в `Data/Photos/`
3. Генерируется уникальное имя: `product_{id}_{timestamp}.jpg`
4. Путь сохраняется в JSON
5. Фото не теряется при удалении исходного файла

---

#### 4.3.5. Персистентность данных

**Формат:** JSON

**Расположение:** `Data/` (рядом с программой)

**Структура:**
```
Data/
├── suppliers.json
├── ingredients.json
├── products.json
├── deliveries.json
├── orders.json
├── sales.json
├── departments.json
├── groups.json
├── meta.json
└── Photos/
    └── product_*.jpg
```

**Автосохранение:**
- При нажатии кнопки "Сохранить"
- При удалении записи
- При закрытии приложения

**Автозагрузка:**
- При запуске приложения
- Восстановление всех связей между объектами

---

#### 4.3.6. Настройка через конфигурационный файл

**Файл:** `app.conf`

**Возможности:**
- Изменение названия окна
- Изменение пути к иконке

**Пример:**
```ini
# Конфигурация CateringIS

WindowTitle=Кафе "Солнышко" - Система учёта
WindowIcon=custom_icon.ico
```

**Применение:** Без перекомпиляции — просто отредактировать и перезапустить

---

### 4.4. Архитектура приложения

#### 4.4.1. Паттерн MVVM

**Model (Модель):**
- Классы предметной области: `Supplier`, `Ingredient`, `Product` и т.д.
- Базовый класс `ObservableObject` с INotifyPropertyChanged
- `AppDatabase` — Singleton для доступа к данным

**View (Представление):**
- XAML-файлы: `MainWindow.xaml`, `SuppliersView.xaml` и т.д.
- Минимальный code-behind (только обработчики валидации)
- DataBinding для связи с ViewModel

**ViewModel (Модель представления):**
- Логика представления: `MainViewModel`, `SuppliersViewModel` и т.д.
- Commands для действий пользователя
- ObservableCollections для списков

**Преимущества MVVM:**
- Полное разделение UI и логики
- Легкое тестирование
- Переиспользование кода
- Поддерживаемость

---

#### 4.4.2. Паттерны проектирования

| Паттерн | Применение | Класс |
|---------|------------|-------|
| Singleton | Единственный экземпляр БД | `AppDatabase` |
| Repository | Централизованный доступ к данным | `AppDatabase` |
| Observer | Автообновление UI | `ObservableObject` |
| Command | Действия пользователя | `RelayCommand` |
| Strategy | Различные правила валидации | `ValidationRule` классы |

---

#### 4.4.3. Структура проекта

```
CateringIS/
├── Models/              ← Модели предметной области
│   └── Models.cs
├── ViewModels/          ← Логика представления
│   ├── MainViewModel.cs
│   ├── SuppliersViewModel.cs
│   └── ...
├── Views/               ← XAML интерфейсы
│   ├── MainWindow.xaml
│   ├── SuppliersView.xaml
│   └── ...
├── Data/                ← Доступ к данным
│   └── AppDatabase.cs
├── Helpers/             ← Вспомогательные классы
│   ├── RelayCommand.cs
│   ├── ValidationRules.cs
│   └── AppConfig.cs
├── Converters/          ← Value converters для XAML
│   └── Converters.cs
├── App.xaml             ← Точка входа, ресурсы, стили
├── app.conf             ← Конфигурация
└── app.ico              ← Иконка
```

---

### 4.5. Пользовательский интерфейс

#### 4.5.1. Главное окно

**Компоненты:**
- Боковое меню навигации (слева)
- Область контента (справа)
- Заголовок текущего раздела

**Навигация:**
```
МЕНЮ:
  📋 Блюда (Меню)
  🗂 Группы блюд

СКЛАД:
  🥦 Ингредиенты
  🚚 Приход товара
  📦 Заказы подразделений

СПРАВОЧНИКИ:
  🏢 Поставщики
  🏬 Подразделения

АНАЛИТИКА:
  📊 Учёт продаж
```

---

#### 4.5.2. Типовой интерфейс раздела

**Левая часть:**
- Таблица со списком записей (DataGrid)
- Кнопки: "+ Добавить", "🗑 Удалить", "💾 Сохранить"

**Правая часть:**
- Панель редактирования выбранной записи
- Поля ввода с валидацией
- Кнопка "💾 Сохранить"

**Особенность:** Master-Detail интерфейс (список слева, детали справа)

---

#### 4.5.3. Визуализация ошибок валидации

**В реальном времени:**
- Красная рамка вокруг поля с ошибкой
- Текст ошибки справа от поля
- Tooltip при наведении

**При сохранении:**
```
┌────────────────────────────────────┐
│  ℹ  Ошибка валидации      │  [X]  │
├────────────────────────────────────┤
│  Обнаружены ошибки:                │
│                                    │
│  • Название не может состоять      │
│    только из цифр                  │
│  • Значение должно быть больше 0   │
│                                    │
│                   ┌──────┐         │
│                   │  OK  │         │
│                   └──────┘         │
└────────────────────────────────────┘
```

---

### 4.6. Требования к системе

**Операционная система:**
- Windows 10 (версия 1809 или новее)
- Windows 11

**Программное обеспечение:**
- .NET 8 Runtime (или SDK для разработки)

**Оборудование:**
- Процессор: 1 ГГц или быстрее
- ОЗУ: 2 ГБ (минимум), 4 ГБ (рекомендуется)
- Дисковое пространство: 100 МБ для приложения + место для данных
- Разрешение экрана: минимум 900×600

---

### 4.7. Установка и развертывание

#### 4.7.1. Для разработки

```bash
# Клонирование/распаковка проекта
cd CateringIS

# Восстановление зависимостей
dotnet restore

# Запуск
dotnet run
```

#### 4.7.2. Для конечных пользователей

```bash
# Публикация автономного приложения
dotnet publish -c Release -r win-x64 --self-contained

# Результат в:
bin/Release/net8.0-windows/win-x64/publish/
```

**Содержимое дистрибутива:**
- CateringIS.exe
- app.ico
- app.conf
- Необходимые DLL
- Папка Data/ создастся автоматически при первом запуске

---

### 4.8. Безопасность и надежность

**Целостность данных:**
- Валидация на уровне UI
- Автосохранение при критических операциях
- JSON с читаемым форматом для ручной корректировки при необходимости

**Резервное копирование:**
- Пользователь может копировать папку `Data/` целиком
- Восстановление: скопировать `Data/` обратно

**Обработка ошибок:**
- Try-catch блоки в критических местах
- Информативные сообщения об ошибках
- Graceful degradation (при ошибке загрузки файла — создается пустая коллекция)

---

### 4.9. Расширяемость

**Добавление новой сущности:**
1. Создать класс модели (наследник `ObservableObject`)
2. Добавить коллекцию в `AppDatabase`
3. Создать ViewModel
4. Создать View (XAML)
5. Добавить DataTemplate в `App.xaml`
6. Добавить кнопку навигации в главное окно

**Время: ~30-60 минут для простой сущности**

---

## 5. ЗАКЛЮЧЕНИЕ

Разработанная информационная система предприятия общественного питания полностью соответствует требованиям технического задания и демонстрирует применение современных технологий и лучших практик разработки:

**Ключевые достижения:**

1. **Полная реализация функциональных требований:**
   - Управление поставщиками с полными реквизитами
   - Учет ингредиентов с автоматическим контролем остатков
   - Карточки блюд с фотографиями и детальной рецептурой
   - Автоматизация документооборота (приход, заказы)
   - Аналитика продаж

2. **Применение ООП принципов:**
   - Инкапсуляция (приватные поля с валидацией)
   - Наследование (базовый класс ObservableObject)
   - Полиморфизм (ValidationRule классы)

3. **Паттерн MVVM:**
   - Четкое разделение Model-View-ViewModel
   - Двусторонний DataBinding
   - Commands для действий пользователя

4. **Качество кода:**
   - Валидация всех пользовательских вводов
   - Информативные сообщения об ошибках
   - Автоматическое управление данными
   - Умная генерация ID

5. **Удобство использования:**
   - Интуитивный интерфейс
   - Визуальная индикация ошибок
   - Автосохранение данных
   - Настройка через конфигурационный файл

Система готова к практическому использованию на предприятиях общественного питания и может быть легко расширена дополнительным функционалом.

---

**Дата составления отчета:** 21 февраля 2026  
**Версия системы:** 1.3  
**Автор:** [Ваше ФИО]  
**Группа:** [Номер группы]
