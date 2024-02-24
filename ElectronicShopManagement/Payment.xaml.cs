using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        List<OrderDetailsItem> currOrder;
        decimal OrderAmount;
        string CustomerName;
        public Payment(List<OrderDetailsItem> currOrder)
        {
           this.currOrder = currOrder;
            InitializeComponent();
            decimal orderTotal = currOrder.Sum(item => item.OrderTotal ?? 0);
            SubTotal.Text = orderTotal.ToString();
            decimal taxRate = 0.15m;
            decimal taxAmount = orderTotal * taxRate;
            Tax.Text = taxAmount.ToString();
            OrderAmount = orderTotal + taxAmount;
            OrderTotal.Text = OrderAmount.ToString();
            CustomerName= currOrder.FirstOrDefault()?.CustName;
            this.lvOrders.ItemsSource = currOrder;
        }

        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51Odw1mCo376H9Bv8TuARUgGGNJAB9SBYNct5yGC60qB9Txz1BHp74r1FMDLdQ1wN52bbojFmbdYKdkgcGCyV8LGH00hyXc2oOp";

               
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderAmount * 100),
                    Currency = "usd",
                    Description = "${ currOrder.CustName}" + "transaction",
                    Source = "tok_visa",
                };
                
                var service = new ChargeService();
                Charge charge = service.Create(options);
                if (charge.Paid)
                {

                    string custName = CustomerName;
                    string empName = currOrder.FirstOrDefault()?.EmpName;
                    decimal orderTotal = OrderAmount;
                    string paymentId = charge.Id;
                    DateTime orderDate = DateTime.Now;
                    Order newOrder = new Order(custName, empName, orderTotal, paymentId,orderDate);
                    ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                    int newOrderId = newOrder.OrderId;
                    foreach (var item in currOrder)
                    {
                        var product = db.Products.FirstOrDefault(p => p.ProdName == item.ProdName);
                        if (product != null)
                        {
                            int productId = product.ProdId;

                            var orderItem = new OrderItem
                            {
                                OrderId = newOrderId,
                                ProdId = productId,
                                PriceAtPurchase = item.PriceAtPurchase,
                                OrderQty = item.OrderQty
                            };
                            db.OrderItems.Add(orderItem);
                        }
                    }
                    db.SaveChanges();
                    MessageBox.Show("pay successfully");
                    this.Close();
                }
                else
                {
                    MessageBox.Show("payment failed");
                }

            }
            catch (Exception ex)
            {

                MessageBox.Show("failed: " + ex.Message);
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            GlobalPro.ordercartlist.Clear();
        }
    }

}
