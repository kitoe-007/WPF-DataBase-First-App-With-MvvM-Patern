# DataBaseFirstWpfApp

WPF-приложение для учёта **оптового склада**: товары, поставщики и поступления.  
Стек построен на **MVVM**, **Entity Framework Core** и подходе **Database First** с SQLite.

---

## Возможности

- Просмотр таблиц: товары, поставщики, склад
- CRUD: добавление, изменение и удаление записей
- Фильтры и аналитические запросы (LINQ / EF Core):
  - товары по типу
  - товары с длительным сроком хранения (≥ 60 дней)
  - агрегация поставок по поставщику
  - join товаров, поставщиков и поступлений
  - поставщики с договором в выбранном диапазоне дат

---

## Стек технологий

| Технология | Назначение |
|---|---|
| **.NET 10 / WPF** | десктопный UI |
| **MVVM** | разделение UI и логики |
| **CommunityToolkit.Mvvm** | `ObservableObject`, `[RelayCommand]`, `[ObservableProperty]` |
| **Entity Framework Core 10** | ORM и доступ к данным |
| **SQLite** | локальная БД (`warehouse.db`) |
| **Database First** | модель генерируется из существующей схемы |

---

## Архитектура: MVVM

```
┌─────────────┐     binding / commands     ┌──────────────────┐
│    View     │ ◄────────────────────────► │    ViewModel     │
│  (XAML)     │                            │  (логика UI)     │
└─────────────┘                            └────────┬─────────┘
                                                    │
                                                    ▼
                                           ┌──────────────────┐
                                           │     Service      │
                                           │  (работа с БД)   │
                                           └────────┬─────────┘
                                                    │
                                                    ▼
                                           ┌──────────────────┐
                                           │  Model + DbContext│
                                           │   (EF Core)       │
                                           └──────────────────┘
```

| Слой | Папка | Роль |
|---|---|---|
| **View** | `Views/` | XAML-окна, привязки, без бизнес-логики |
| **ViewModel** | `ViewModels/` | состояние UI, команды, реакция на действия пользователя |
| **Model** | `Models/` | сущности предметной области |
| **Data** | `Data/` | `WarehouseContext` — EF Core DbContext |
| **Service** | `Services/` | загрузка, сохранение, запросы к БД |

**View** знает только о **ViewModel** (через `DataContext` и binding).  
**ViewModel** не обращается к UI напрямую для данных — работает через сервисы и модели.

### Как это выглядит в коде

```xml
<!-- Views/MainView.xaml -->
<Window.DataContext>
    <vm:MainViewModel/>
</Window.DataContext>

<ComboBox ItemsSource="{Binding Tables}" SelectedItem="{Binding SelectedTable}"/>
<Button Content="Добавить" Command="{Binding AddCommand}"/>
<DataGrid ItemsSource="{Binding Items}" SelectedItem="{Binding SelectedItem}"/>
```

```csharp
// ViewModels/MainViewModel.cs
public partial class MainViewModel : ObservableObject
{
    [ObservableProperty] string _selectedTable = "Товары";
    [ObservableProperty] IEnumerable _items;

    [RelayCommand]
    private void Add() { /* ... */ }
}
```

---

## Подход Database First

Схема БД создаётся **сначала** (SQL / готовый файл `warehouse.db`), затем по ней строится C#-модель.

```
warehouse.sql  →  warehouse.db  →  scaffold (EF Core)  →  Models + WarehouseContext
```

Типичная команда scaffold:

```bash
dotnet ef dbcontext scaffold "Data Source=warehouse.db" Microsoft.EntityFrameworkCore.Sqlite ^
  --output-dir Models ^
  --context-dir Data ^
  --context WarehouseContext ^
  --force
```

В проекте:

- `warehouse.sql` — исходная схема и демо-данные
- `warehouse.db` — рабочая SQLite-база (копируется в output)
- `Models/*` — сущности (`Product`, `Supplier`, `Warehouse`)
- `Data/WarehouseContext.cs` — DbContext с маппингом таблиц и связей
- `Services/ApplicationContext.cs` — singleton-контекст с connection string

---

## Модель данных

```
suppliers 1 ─────── * warehouse * ─────── 1 products
```

| Таблица | Сущность | Описание |
|---|---|---|
| `products` | `Product` | наименование, тип, производитель, срок годности… |
| `suppliers` | `Supplier` | название, адрес, телефон, дата договора |
| `warehouse` | `Warehouse` | поступление: дата, товар, поставщик, объём, цена |

Связи: `Warehouse` → `Product`, `Warehouse` → `Supplier` (FK + navigation properties).

---

## Структура проекта

```
DataBaseFirstWpfApp/
├── Views/
│   ├── MainView.xaml      # главное окно (список + команды)
│   └── EditView.xaml      # диалог добавления / редактирования
├── ViewModels/
│   ├── MainViewModel.cs   # таблицы, CRUD, запросы
│   └── EditViewModel.cs   # данные для формы редактирования
├── Models/
│   ├── Product.cs
│   ├── Supplier.cs
│   └── Warehouse.cs
├── Data/
│   └── WarehouseContext.cs
├── Services/
│   ├── ApplicationContext.cs
│   └── Load_service.cs
├── warehouse.db
├── warehouse.sql
└── DataBaseFirstWpfApp.csproj
```

---

## Запуск

**Требования:** .NET 10 SDK, Windows.

```bash
dotnet restore
dotnet run
```

Или откройте решение в Visual Studio / Rider и запустите проект.

> База `warehouse.db` копируется в каталог сборки (`CopyToOutputDirectory`).  
> При первом запуске уже есть демо-данные из `warehouse.sql`.

---

## Основные зависимости

```xml
<PackageReference Include="CommunityToolkit.Mvvm" Version="8.4.2" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Sqlite" Version="10.0.13" />
<PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="10.0.13" />
```

---

## Кратко

| Принцип | Реализация в проекте |
|---|---|
| **MVVM** | Views ↔ ViewModels, binding + RelayCommand |
| **Database First** | схема SQLite → scaffold → модели и DbContext |
| **Разделение ответственности** | UI / логика / доступ к данным в разных слоях |
| **LINQ to Entities** | фильтры, join, группировка через EF Core |
