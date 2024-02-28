using PdfSharpCore.Drawing;
using PdfSharpCore.Pdf;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for InvoicePage2.xaml
    /// </summary>
    public partial class InvoicePage2 : Window
    {
        private OrderDetailsItem SelectedOrder { get; set; }

        public InvoicePage2(OrderDetailsItem selectedOrder)
        {
            InitializeComponent();
            SelectedOrder = selectedOrder;

            PopulateInvoiceData();
        }

        private void PopulateInvoiceData()
        {
            txtCustomerName.Text = SelectedOrder.CustName;
            txtOrderDate.Text = SelectedOrder.OrderDate.ToString();
            txtEmployeeName.Text = SelectedOrder.EmpName;
            txtProductName.Text = SelectedOrder.ProdName;
            txtPrice.Text = SelectedOrder.PriceAtPurchase.ToString();
            txtQty.Text = SelectedOrder.OrderQty.ToString();

            // Calculate subtotal
            decimal subtotal = (SelectedOrder.PriceAtPurchase ?? 0) * (SelectedOrder.OrderQty ?? 0);
            txtSubTotal.Text = subtotal.ToString();

            // Calculate tax (15% of subtotal)
            decimal tax = subtotal * 0.15m; // Assuming tax is 10%
            txtTax.Text = tax.ToString();

            // Calculate grand total (subtotal + tax)
            decimal grandTotal = subtotal + tax;
            txtGrandTotal.Text = grandTotal.ToString();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
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
            Close(); // Close the window
        }
    }
}
