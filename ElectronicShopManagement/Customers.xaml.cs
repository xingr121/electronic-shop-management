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
    /// Interaction logic for Customers.xaml
    /// </summary>
    public partial class Customers : Page
    {
        public Customers()
        {
           
            InitializeComponent();
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
            var cus = from c in db.Customers select c;
            this.LvCustomers.ItemsSource = cus.ToList();
        }

        private void CmUpdade_Click(object sender, RoutedEventArgs e)
        {
            Customer currSelectedCustomer = LvCustomers.SelectedItem as Customer;
            if (currSelectedCustomer == null) return;
            EditAddCustomer dialog = new EditAddCustomer(currSelectedCustomer);
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var cus = from c in db.Customers select c;
                this.LvCustomers.ItemsSource = cus.ToList();
            }
        }

        private void CmDelete_Click(object sender, RoutedEventArgs e)
        {

            Customer currSelectedCustomer = LvCustomers.SelectedItem as Customer;
            if (currSelectedCustomer == null) return;
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            var result = MessageBox.Show(parentWindow, "are you sure to delete this item", "confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question
                );
            if (result == MessageBoxResult.No) return;
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var existingCustomer = db.Customers.Find(currSelectedCustomer.CustId);
                if (existingCustomer == null)
                {
                    MessageBox.Show(parentWindow, "The selected product no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Customers.Remove(existingCustomer);
                db.SaveChanges();

                var cus = from c in db.Customers select c;
                this.LvCustomers.ItemsSource = cus.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(parentWindow, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();

            if (FilterBy.Text == "Name")
            {
                var cus = from c in db.Customers where c.CustName.Contains(TxbFilter.Text) select c;
                this.LvCustomers.ItemsSource = cus.ToList();
            }
            else if (FilterBy.Text == "Phone")
            {
                var cus = from c in db.Customers where c.CustPhone.Contains(TxbFilter.Text) select c;
                this.LvCustomers.ItemsSource = cus.ToList();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            EditAddCustomer dialog = new EditAddCustomer();
            Window parentWindow = Window.GetWindow(this);
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var cus = from c in db.Customers select c;
                this.LvCustomers.ItemsSource =cus.ToList();
            }
        }
    }
    
}
