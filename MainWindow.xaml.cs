using Microsoft.EntityFrameworkCore;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WpfFirstEFApp.DB;

namespace WpfFirstEFApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        _1135Start2025Context db;
        private Product insertProduct = new Product();
        private List<Product> products;

        public event PropertyChangedEventHandler? PropertyChanged;

        void Signal([CallerMemberName] string prop = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        public List<Product> Products
        {
            get => products;
            set
            {
                products = value;
                Signal();
            }
        }
        public Product InsertProduct
        {
            get => insertProduct;
            set
            {
                insertProduct = value;
                Signal();
            }
        }
        public List<Category> Categories { get; set; }
        public List<Supplier> Suppliers { get; set; }
        public MainWindow()
        {
            InitializeComponent();
            db = new _1135Start2025Context();
            Categories = db.Categories.ToList();
            Suppliers = db.Suppliers.ToList();
            //db.Suppliers.ToList(); // поставщики сохранятся в dbcontext
            SelectProducts();
            DataContext = this;
        }

        private void UpdateProduct(object sender, RoutedEventArgs e)
        {
            db.SaveChanges();
        }
        private void InsertProductMethod(object sender, RoutedEventArgs e)
        {
            db = new _1135Start2025Context();
            // костыль для исключения добавления уже существующих объектов
            // в связанные таблицы
            InsertProduct.SupplierId = InsertProduct.Supplier.Id;
            InsertProduct.Supplier = null;
            InsertProduct.CategoryId = InsertProduct.Category.Id;
            InsertProduct.Category = null;
            // чаще должен быть вариант, что DbContext уже содержит существующие данные
            // метод Add добавляет всю иерархию объекта в отслеживание
            db.Products.Add(InsertProduct);
            db.SaveChanges();
            InsertProduct = new Product();
            SelectProducts();
        }

        private void SelectProducts()
        {
            var query = db.Products
                        .Include(s => s.Category)
                        .Include(s => s.Supplier)
                        .Where(s => s.Count == 1) // условие
                        .Skip(1)   // пропуск кол-ва записей
                        .Take(1);   // ограничение кол-ва записей
            
            Products = query.ToList();
        }

        private void RemoveProduct(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Точно????", "Заголовок", MessageBoxButton.YesNoCancel) == MessageBoxResult.Yes)
            {
                var product = (sender as Button).CommandParameter as Product;

                db.Products.Remove(product);
                db.SaveChanges();

                SelectProducts();
            }
        }
    }
}