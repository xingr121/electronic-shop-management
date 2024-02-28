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
    /// Interaction logic for EditAddCustomer.xaml
    /// </summary>
    public partial class EditAddCustomer : Window
    {
        Customer currCustomer;
        public EditAddCustomer(Customer currCustomer=null)
        {
            this.currCustomer = currCustomer;

            InitializeComponent();
            
            if (currCustomer != null)
            {
                Title = "Update Customer";
                NameInput.Text = currCustomer.CustName;
                AddressInput.Text = currCustomer.CustAddress;
                PhoneInput.Text = currCustomer.CustPhone;

                BtnSave.Content = "Update";
            }
            else
            {
                Title = "Add New Customer";
                BtnSave.Content = "Add";
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();



                if (currCustomer != null)
                {
                    Customer existingCustomer = db.Customers.FirstOrDefault(c => c.CustId == currCustomer.CustId);

                    if (existingCustomer != null)
                    {
                        existingCustomer.CustName = NameInput.Text;
                        existingCustomer.CustAddress = AddressInput.Text;
                        existingCustomer.CustPhone = PhoneInput.Text;
                       


                    }
                    else
                    {
                        throw new InvalidOperationException("Customer not found in the database.");
                    }
                }
                else
                {

                    Customer newCustomer = new Customer(NameInput.Text, AddressInput.Text, PhoneInput.Text);

                    db.Customers.Add(newCustomer);
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
