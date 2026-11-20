using DataBaseFirstWpfApp.Models;
using Microsoft.EntityFrameworkCore;
using SQLiteApp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;


namespace DataBaseFirstWpfApp.Services
{
    public class Load_service
    {
        public static List<T> Load<T>() where T : class
        {
            var db = ApplicationContext.GetContext();
            return db.Set<T>().ToList();
        }

        public static List<Warehouse> LoadWarehouses()
        {
            var db = ApplicationContext.GetContext();
            return db.Warehouses.Include(w => w.Product).Include(w => w.Supplier).ToList();
        }

        public static bool Add(object item)
        {
            var db = ApplicationContext.GetContext();
            db.Add(item);
            if (Save()) return true;
            db.Entry(item).State = EntityState.Detached;
            return false;
        }

        public static bool Update(object item)
        {
            if (Save()) return true;
            Cancel(item);
            return false;
        }

        public static void Cancel(object item)
        {
            ApplicationContext.GetContext().Entry(item).Reload();
        }

        public static void Delete(object item)
        {
            var db = ApplicationContext.GetContext();
            if (item is Product p)
                db.Warehouses.RemoveRange(db.Warehouses.Where(w => w.ProductId == p.Id));
            if (item is Supplier s)
                db.Warehouses.RemoveRange(db.Warehouses.Where(w => w.SupplierId == s.Id));
            db.Remove(item);
            Save();
        }

        private static bool Save()
        {
            try
            {
                ApplicationContext.GetContext().SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                System.Windows.MessageBox.Show("Ошибка сохранения: " + (ex.InnerException?.Message ?? ex.Message));
                return false;
            }
        }

        public static List<Product> Get_Bakal(string t)
        {
                var db = ApplicationContext.GetContext();
                return db.Set<Product>().Where(p => p.Type == t).ToList();
        }

        public static List<Product> Get_Dolgosrok()
        {
            var db = ApplicationContext.GetContext();
            return db.Set<Product>().Where(p => p.ShelfLife >= 60).ToList();
        }

        public static IEnumerable Get_PriceWarehouse()
        {
            var db = ApplicationContext.GetContext();
            return db.Warehouses
                .GroupBy(w => w.Supplier.Name)
                .Select(g => new
                {
                    Поставщик = g.Key,
                    Поставок = g.Count(),
                    ОбщийОбъём = g.Sum(w => w.Volume),
                    СредняяЦена = g.Average(w => w.Price)
                }).ToList();
        }

        public static IEnumerable Get_Join()
        {
            var db = ApplicationContext.GetContext();
            return db.Warehouses
                .Join(db.Products, w => w.ProductId, p => p.Id, (w, p) => new { w, p })
                .Join(db.Suppliers, wp => wp.w.SupplierId, s => s.Id, (wp, s) => new
                {
                    Дата = wp.w.ReceiptDate,
                    Товар = wp.p.Name,
                    Тип = wp.p.Type,
                    Поставщик = s.Name,
                    Телефон = s.Phone,
                    Объём = wp.w.Volume,
                    Цена = wp.w.Price
                }).ToList();
        }

        public static List<Supplier> Get_ContractsBetween(DateTime from, DateTime to)
        {
            var db = ApplicationContext.GetContext();
            return db.Suppliers.Where(s => s.ContractDate >= from && s.ContractDate <= to).ToList();
        }    }
}
