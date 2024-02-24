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
    /// Interaction logic for OrderPage3.xaml
    /// </summary>
    public partial class OrderPage3 : Page
    {
        private OrderDetailsItem selectedOrderItem;
        public OrderPage3()
        {
            InitializeComponent();
            LoadOrderDetails();
        }

        private void LoadOrderDetails()
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();

            var orderDetails = from order in db.Orders
                               join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
                               join product in db.Products on orderItem.ProdId equals product.ProdId
                               select new OrderDetailsItem // Create an OrderDetailsItem object
                               {
                                   OrderId = order.OrderId,
                                   CustName = order.CustName,
                                   OrderTotal = order.OrderTotal,
                                   OrderDate = order.OrderDate,
                                   EmpName = order.EmpName,
                                   OrderQty = orderItem.OrderQty,
                                   ProdName = product.ProdName,
                                   PriceAtPurchase = orderItem.PriceAtPurchase
                               };

            LvOrders.ItemsSource = orderDetails.ToList();
        }

        private void BtnOrdWiz_Click(object sender, RoutedEventArgs e)
        {
            OrderWizard orderWizard = new OrderWizard(this);
            orderWizard.Show();
        }

        public void RefreshListView()
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();

            var orderDetails = from order in db.Orders
                               join orderItem in db.OrderItems on order.OrderId equals orderItem.OrderId
                               join product in db.Products on orderItem.ProdId equals product.ProdId
                               select new
                               {
                                   order.OrderId,
                                   order.CustName,
                                   order.OrderTotal,
                                   order.OrderDate,
                                   order.EmpName,
                                   orderItem.OrderQty,
                                   product.ProdName,
                                   orderItem.PriceAtPurchase
                               };

            LvOrders.ItemsSource = orderDetails.ToList();
        }

        private void BtnInvoice_Click(object sender, RoutedEventArgs e)
        {
            if (selectedOrderItem != null) // Check if an order item is selected
            {
                // Pass the selected item data to the InvoicePage
                InvoicePage invoicePage = new InvoicePage(selectedOrderItem);
                invoicePage.Show();
            }
            else
            {
                MessageBox.Show("Please select an order to create an invoice.");
            }
        }

        private void LvOrders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (LvOrders.SelectedItem != null)
            {
                // Ensure correct type casting
                var selectedOrder = LvOrders.SelectedItem as dynamic;

                // Create an OrderDetailsItem object using the selected item data
                selectedOrderItem = new OrderDetailsItem
                {
                    OrderId = selectedOrder.OrderId,
                    CustName = selectedOrder.CustName,
                    OrderTotal = selectedOrder.OrderTotal,
                    OrderDate = selectedOrder.OrderDate,
                    EmpName = selectedOrder.EmpName,
                    OrderQty = selectedOrder.OrderQty,
                    ProdName = selectedOrder.ProdName,
                    PriceAtPurchase = selectedOrder.PriceAtPurchase
                };
            }
        }
    }
}