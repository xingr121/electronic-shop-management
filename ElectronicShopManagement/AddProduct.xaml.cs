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
using System.Windows.Shapes;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        Product currProduct;
        public AddProduct(Product currProduct = null)
        {
            this.currProduct = currProduct;
            InitializeComponent();
            if (currProduct != null)
            {
                Title = "Update Product";
                NameInput.Text = currProduct.ProdName;
                QtyInput.Text = currProduct.ProdQty.ToString();
                PriceInput.Text = currProduct.ProdPrice.ToString();

                foreach (ComboBoxItem item in Category.Items)
                {
                    if (item.Content.ToString() == currProduct.ProdCategory)
                    {
                        Category.SelectedItem = item;
                        break;
                    }
                }

                BtnSave.Content = "Update";
            }
            else
            {
                Title = "Add New Product";
                BtnSave.Content = "Add";
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();



                if (currProduct != null)
                {
                    Product existingProduct = db.Products.FirstOrDefault(p => p.ProdId == currProduct.ProdId);

                    if (existingProduct != null)
                    {
                        existingProduct.ProdName = NameInput.Text;
                        existingProduct.ProdCategory = ((ComboBoxItem)Category.SelectedItem).Content.ToString();
                        existingProduct.ProdQty = int.Parse(QtyInput.Text);
                        existingProduct.ProdPrice = decimal.Parse(PriceInput.Text);

                     
                    }
                    else
                    {
                        throw new InvalidOperationException("Product not found in the database.");
                    }
                }
                else
                {

                    if (!int.TryParse(QtyInput.Text, out int quantity) || !decimal.TryParse(PriceInput.Text, out decimal price))
                    {
                        throw new ArgumentException("Invalid quantity or price format.");
                    }

                    string category = ((ComboBoxItem)Category.SelectedItem).Content.ToString();
                    Product newProduct = new Product(NameInput.Text, category, quantity, price);

                    db.Products.Add(newProduct);
                }

                db.SaveChanges();
                this.DialogResult = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
