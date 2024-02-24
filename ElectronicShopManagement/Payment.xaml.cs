using Stripe;
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
    /// Interaction logic for Payment.xaml
    /// </summary>
    public partial class Payment : Window
    {
        Order currOrder;
        decimal OrderAmount;
        public Payment(Order currOrder)
        {
           this.currOrder = currOrder;
            InitializeComponent();
            decimal orderTotal = currOrder.OrderTotal.HasValue ? currOrder.OrderTotal.Value : 0;
            SubTotal.Text = orderTotal.ToString();
            decimal taxRate = 0.15m;
            decimal taxAmount = orderTotal * taxRate;
            Tax.Text = taxAmount.ToString();
            OrderAmount = orderTotal + taxAmount;
            OrderTotal.Text = OrderAmount.ToString();
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

                    string custName = currOrder.CustName;
                    string empName = "name";
                    decimal orderTotal = OrderAmount;
                    string paymentId = charge.Id;
                    Order newOrder = new Order(custName, empName, orderTotal, paymentId);
                    ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                    MessageBox.Show("pay successfully");
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
    }

}
