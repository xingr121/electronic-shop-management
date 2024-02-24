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
    /// Interaction logic for OrderWizard.xaml
    /// </summary>
    public partial class OrderWizard : Window
    {
        private readonly ElectronicShopManagementDBEntities _dbContext = new ElectronicShopManagementDBEntities();
        
        private OrderPage3 orderPage;

        public OrderWizard(OrderPage3 orderPage)
        {
            InitializeComponent();
            this.orderPage = orderPage;
            PopulateCustomerIdComboBox();
            PopulateProductNameComboBox();
            PopulateEmployeeIdComboBox();
        }

        private void PopulateCustomerIdComboBox()
        {
            try
            {
                var customerIds = _dbContext.Customers.Select(c => c.CustId).ToList();
                customerIdComboBox.ItemsSource = customerIds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while populating customer IDs: " + ex.Message);
            }
        }

        private void PopulateProductNameComboBox()
        {
            try
            {
                var productNames = _dbContext.Products.Select(p => p.ProdName).ToList();
                productNameComboBox.ItemsSource = productNames;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while populating product names: " + ex.Message);
            }
        }

        private void PopulateEmployeeIdComboBox()
        {
            try
            {
                var employeeIds = _dbContext.Employees.Select(c => c.EmpId).ToList();
                employeeIdComboBox.ItemsSource = employeeIds;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while populating employee IDs: " + ex.Message);
            }
        }

        private void CustomerIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int customerId;
                if (customerIdComboBox.SelectedItem != null && int.TryParse(customerIdComboBox.SelectedItem.ToString(), out customerId))
                {
                    var customer = _dbContext.Customers.FirstOrDefault(c => c.CustId == customerId);
                    if (customer != null)
                    {
                        customerNameTextBlock.Text = customer.CustName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fetching customer name: " + ex.Message);
            }
        }

        private void ProductNameComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string productName = productNameComboBox.SelectedItem as string;
                if (productName != null)
                {
                    var product = _dbContext.Products.FirstOrDefault(p => p.ProdName == productName);
                    if (product != null)
                    {
                        productPriceTextBlock.Text = product.ProdPrice.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fetching product price: " + ex.Message);
            }
        }

        private void EmployeeIdComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int employeeId;
                if (employeeIdComboBox.SelectedItem != null && int.TryParse(employeeIdComboBox.SelectedItem.ToString(), out employeeId))
                {
                    var employee = _dbContext.Employees.FirstOrDefault(c => c.EmpId == employeeId);
                    if (employee != null)
                    {
                        employeeNameTextBlock.Text = employee.EmpName;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error while fetching employee name: " + ex.Message);
            }
        }

        // In the Wizard_Finish method:
        private void Wizard_Finish(object sender, RoutedEventArgs e)
        {
            // Gather data from wizard pages
            string customerName = customerNameTextBlock.Text;
            string productName = productNameComboBox.SelectedItem.ToString();
            decimal productPrice = decimal.Parse(productPriceTextBlock.Text);
            int orderQuantity = int.Parse(orderQuantityTextBox.Text);
            string employeeName = employeeNameTextBlock.Text;
            decimal orderTotal = productPrice * orderQuantity;

            var orderItem = new OrderDetailsItem(customerName,orderTotal,employeeName,orderQuantity,productName,productPrice);
            if (orderItem != null)
            {
                if (GlobalPro.ordercartlist == null)
                {
                    GlobalPro.ordercartlist = new List<OrderDetailsItem>();
                }
                GlobalPro.ordercartlist.Add(orderItem);
            }
            var result=MessageBox.Show(this, "Continue to add more produt?", "make order", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if(result == MessageBoxResult.Yes)
            {
                OrderWizard orderWizard = new OrderWizard(orderPage);
                orderWizard.Show();
            } else if(result==MessageBoxResult.No) {
                Payment paymentPage=new Payment(GlobalPro.ordercartlist);
                paymentPage.Show();
            }
            {

            }
        }
    }
}
