using System.Collections.Generic;
using DataBaseFirstWpfApp.Models;
using DataBaseFirstWpfApp.Services;

namespace DataBaseFirstWpfApp.ViewModels
{
    public class EditViewModel
    {
        public object Item { get; }
        public List<Product> Products { get; } = Load_service.Load<Product>();
        public List<Supplier> Suppliers { get; } = Load_service.Load<Supplier>();

        public EditViewModel(object item)
        {
            Item = item;
        }
    }
}
