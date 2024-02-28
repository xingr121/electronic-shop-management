using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for Products.xaml
    /// </summary>
    public partial class Products : Page
    {
        public Products()
        {
            InitializeComponent();
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
            var pros=from p in db.Products select p;
           this.LvProducts.ItemsSource = pros.ToList();
        }

        private void TxbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();

            if (FilterBy.Text == "Name")
            {
                var pros = from p in db.Products where p.ProdName.Contains(TxbFilter.Text) select p;
                this.LvProducts.ItemsSource = pros.ToList();
            }
            else if (FilterBy.Text == "Category")
            {
                var pros = from p in db.Products where p.ProdCategory.Contains(TxbFilter.Text) select p;
                this.LvProducts.ItemsSource = pros.ToList();
            }

        }
        private void CmUpdade_Click(object sender, RoutedEventArgs e)
        {
            Product currSelectedProduct = LvProducts.SelectedItem as Product;
            if (currSelectedProduct == null) return;
            AddProduct dialog = new AddProduct(currSelectedProduct);
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var pros = from p in db.Products select p;
                this.LvProducts.ItemsSource = pros.ToList();
            }
        }

        private void CmDelete_Click(object sender, RoutedEventArgs e)
        {
            Product currSelectedProduct = LvProducts.SelectedItem as Product;
            if (currSelectedProduct == null) return;
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            var result = MessageBox.Show(parentWindow, "are you sure to delete this item", "confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question
                );
            if (result == MessageBoxResult.No) return;
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var existingProduct = db.Products.Find(currSelectedProduct.ProdId);
                if (existingProduct == null)
                {
                    MessageBox.Show(parentWindow, "The selected product no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Products.Remove(existingProduct);
                db.SaveChanges();

                var pros = from p in db.Products select p;
                this.LvProducts.ItemsSource = pros.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(parentWindow, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    

        private void BtnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddProduct dialog= new AddProduct();
            Window parentWindow = Window.GetWindow(this); 
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var pros = from p in db.Products select p;
                this.LvProducts.ItemsSource = pros.ToList();
            }
            }

     
    }
}
