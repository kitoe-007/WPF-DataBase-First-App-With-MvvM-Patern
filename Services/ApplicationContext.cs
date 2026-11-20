using Microsoft.EntityFrameworkCore;
using DataBaseFirstWpfApp.Data;

namespace SQLiteApp
{
    public class ApplicationContext : WarehouseContext
    {
        private static ApplicationContext _context;

        public ApplicationContext()
            : base(new DbContextOptionsBuilder<WarehouseContext>().UseSqlite("Data Source=warehouse.db").Options)
        {
        }

        public static ApplicationContext GetContext()
        {
            if (_context == null)
                _context = new ApplicationContext();
            return _context;
        }
    }

}
