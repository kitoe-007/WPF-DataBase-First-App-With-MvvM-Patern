using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using CommunityToolkit.Mvvm;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DataBaseFirstWpfApp.Models;
using DataBaseFirstWpfApp.Services;
using DataBaseFirstWpfApp.Views;

namespace DataBaseFirstWpfApp.ViewModels
{
    public partial class MainViewModel : ObservableObject
    {
        public List<string> Tables { get; } = new() { "Товары", "Поставщики", "Склад" };

        [ObservableProperty]
        string _selectedTable = "Товары";

        [ObservableProperty]
        IEnumerable _items;

        [ObservableProperty]
        object _selectedItem;

        [ObservableProperty]
        string _selectType;

        public MainViewModel()
        {
            LoadTable();
        }

        partial void OnSelectedTableChanged(string value) => LoadTable();

        private void LoadTable()
        {
            Items = SelectedTable switch
            {
                "Поставщики" => Load_service.Load<Supplier>(),
                "Склад" => Load_service.LoadWarehouses(),
                _ => Load_service.Load<Product>()
            };
        }

        [RelayCommand]
        private void Add()
        {
            object item = SelectedTable switch
            {
                "Поставщики" => new Supplier(),
                "Склад" => new Warehouse { ReceiptDate = DateTime.Today },
                _ => new Product()
            };
            if (EditView.Open(item))
                Load_service.Add(item);
            LoadTable();
        }

        [RelayCommand]
        private void Edit()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите строку в таблице");
                return;
            }
            var item = SelectedItem;
            if (EditView.Open(item))
                Load_service.Update(item);
            else
                Load_service.Cancel(item);
            LoadTable();
        }

        [RelayCommand]
        private void Delete()
        {
            if (SelectedItem == null)
            {
                MessageBox.Show("Выберите строку в таблице");
                return;
            }
            if (MessageBox.Show("Удалить выбранную запись?", "Подтверждение", MessageBoxButton.YesNo) != MessageBoxResult.Yes)
                return;
            Load_service.Delete(SelectedItem);
            LoadTable();
        }
        [RelayCommand]
        private void Command1() { Items = Load_service.Get_Bakal(_selectType); }

        [RelayCommand]
        private void Command2() { Items = Load_service.Get_Dolgosrok(); }
        [RelayCommand]
        private void Command3() { Items = Load_service.Get_PriceWarehouse(); }
        [RelayCommand]
        private void Command4() { Items = Load_service.Get_Join(); }

        [ObservableProperty]
        DateTime _dateFrom = DateTime.Today.AddYears(-1);

        [ObservableProperty]
        DateTime _dateTo = DateTime.Today;

        [RelayCommand]
        private void Command5() { Items = Load_service.Get_ContractsBetween(DateFrom, DateTo); }    }
}
