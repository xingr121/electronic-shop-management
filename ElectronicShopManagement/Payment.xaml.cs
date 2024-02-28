using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using Stripe;
using Stripe.Climate;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
        int currOrderId;
        DateTime currOrderDate;
        public Payment(List<OrderDetailsItem> currOrder)
        {
           this.currOrder = currOrder;
            InitializeComponent();
            decimal orderTotal = currOrder.Sum(item => item.OrderTotal ?? 0);
            SubTotal.Text = $"SubTotal: {orderTotal.ToString()}";
            decimal taxRate = 0.15m;
            decimal taxAmount = orderTotal * taxRate;
            Tax.Text =  $"Estimated Tax: {taxAmount.ToString()}";
            OrderAmount = orderTotal + taxAmount;
            OrderTotal.Text = $"Order Total: {OrderAmount.ToString()}";
            CustomerName= currOrder.FirstOrDefault()?.CustName;
            this.lvOrders.ItemsSource = currOrder;
            BtnInvoice.Visibility = Visibility.Hidden;
            TbInvoice.Visibility = Visibility.Hidden;
            information.Visibility = Visibility.Hidden;
        }

        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StripeConfiguration.ApiKey = "sk_test_51Odw1mCo376H9Bv8276tO6VdA7psgzGuIDgvKmzZwYiZwAkM8QlcBhcGJNI9vRQsgOmiFQkqcfVQLt3wyFDsSPkF00N8c22pPN";

               
                var options = new ChargeCreateOptions
                {
                    Amount = Convert.ToInt32(OrderAmount * 100),
                    Currency = "usd",
                    Description = $"{CustomerName}" + "transaction",
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
                     currOrderDate = DateTime.Now;
                    Order newOrder = new Order(custName, empName, orderTotal, paymentId,currOrderDate);
                    ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                    db.Orders.Add(newOrder);
                    db.SaveChanges();
                     currOrderId = newOrder.OrderId;
                    foreach (var item in currOrder)
                    {
                        var product = db.Products.FirstOrDefault(p => p.ProdName == item.ProdName);
                        if (product != null)
                        {
                            int productId = product.ProdId;

                            var orderItem = new OrderItem
                            {
                                OrderId = currOrderId,
                                ProdId = productId,
                                PriceAtPurchase = item.PriceAtPurchase,
                                OrderQty = item.OrderQty
                            };
                            db.OrderItems.Add(orderItem);
                            product.ProdQty -= item.OrderQty;
                        }
                    }
                    db.SaveChanges();
                    MessageBox.Show(this,"pay successfully","Pay information",MessageBoxButton.OK,MessageBoxImage.Information);
                    GlobalPro.ordercartlist.Clear();
                   
                }
                else
                {
                    MessageBox.Show(this,"payment failed","Pay information",MessageBoxButton.OK,MessageBoxImage.Information);
                }
               

            }
            catch (Exception ex)
            {

                MessageBox.Show("failed: " + ex.Message);
            }
            BtnCancel.Visibility = Visibility.Hidden;
            BtnPlace.Visibility = Visibility.Hidden;
            BtnInvoice.Visibility = Visibility.Visible;
            TbInvoice.Visibility = Visibility.Visible;
            information.Visibility = Visibility.Visible;
            TxtcusName.Text = $"Customer Name: {CustomerName}";
            TxtOrderId.Text = currOrderId.ToString();
            TxtorderDate.Text = currOrderDate.ToString();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            GlobalPro.ordercartlist.Clear();
        }

        private void BtnInvoice_Click(object sender, RoutedEventArgs e)
        {
            // Create a PDF document
            using (var doc = new PdfDocument())
            {
                // Create a page
                var page = doc.AddPage();

                // Get the size of the WPF page
                var pageSize = new PdfSharpCore.Drawing.XSize(this.ActualWidth, this.ActualHeight);

                // Draw the content of the current page onto the PDF page
                using (var gfx = XGraphics.FromPdfPage(page))
                {
                    var wpfVisual = VisualTreeHelper.GetChild(this, 0) as Visual;
                    var drawingVisual = new DrawingVisual();
                    using (var drawingContext = drawingVisual.RenderOpen())
                    {
                        var wpfVisualBrush = new VisualBrush(wpfVisual);
                        drawingContext.DrawRectangle(wpfVisualBrush, null, new Rect(new Point(0, 0), new System.Windows.Size(pageSize.Width, pageSize.Height)));
                    }
                    var renderTargetBitmap = new RenderTargetBitmap((int)pageSize.Width, (int)pageSize.Height, 96, 96, PixelFormats.Pbgra32);
                    renderTargetBitmap.Render(drawingVisual);

                    // Convert BitmapSource to XImage
                    using (var ms = new MemoryStream())
                    {
                        var encoder = new PngBitmapEncoder();
                        encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
                        encoder.Save(ms);
                        ms.Seek(0, SeekOrigin.Begin);
                        var xImage = XImage.FromStream(() => ms);

                        // Draw the image onto the PDF page
                        gfx.DrawImage(xImage, 0, 0, pageSize.Width, pageSize.Height);
                    }
                }

                // Get the path to the Downloads folder
                string downloadsFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                // Specify the file path where the PDF will be saved
                var filePath = System.IO.Path.Combine(downloadsFolder, "output.pdf");

                // Save the PDF document to the specified file
                doc.Save(filePath);

                MessageBox.Show("PDF saved successfully.");

                // Open the saved PDF file
                System.Diagnostics.Process.Start(filePath);
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }

}
