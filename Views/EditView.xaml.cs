using System.Windows;
using DataBaseFirstWpfApp.ViewModels;
using SQLiteApp;
using Microsoft.EntityFrameworkCore;

namespace DataBaseFirstWpfApp.Views
{
    public partial class EditView : Window
    {
        public EditView(object item)
        {
            InitializeComponent();
            DataContext = new EditViewModel(item);
            bool isNew = ApplicationContext.GetContext().Entry(item).State == EntityState.Detached;
            SaveButton.Content = isNew ? "Добавить" : "Сохранить";
        }

        public static bool Open(object item)
        {
            var window = new EditView(item) { Owner = Application.Current.MainWindow };
            return window.ShowDialog() == true;
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }
    }
}
