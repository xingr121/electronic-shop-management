using LiveCharts;
using LiveCharts.Wpf;
using Stripe.Climate;
using System;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Windows.Controls;
namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for Homepage.xaml
    /// </summary>
    public partial class Homepage : UserControl
    {
        public Homepage()
        {
            InitializeComponent();
            LoadChartData();
            DataContext = this;
            decimal dailyTotal =GetDailyOrderTotal();
            string dailyTotalText = $"{(dailyTotal / 1000):0.##}K";
            TbkDay.Text = dailyTotalText;
            decimal monthlyTotal = GetMonthlyOrderTotal();
            string monthlyTotalText = $"{(monthlyTotal / 1000):0.##}K";
            TbkMonth.Text = monthlyTotalText;
        }

        public SeriesCollection SeriesCollection { get; set; }
        public string[] Labels { get; set; }
        public Func<double, string> Formatter { get; set; }

        private void LoadChartData()
        {
            // Connect to your Products database (assuming you're using Entity Framework)
            using (var context = new ElectronicShopManagementDBEntities())
            {
                // Retrieve data from the database
                var categoryQuantities = context.Products
                    .GroupBy(p => p.ProdCategory)
                    .Select(g => new { Category = g.Key, Quantity = g.Sum(p => p.ProdQty) })
                    .ToList();

                // Populate chart data
                SeriesCollection = new SeriesCollection();
                foreach (var item in categoryQuantities)
                {
                    SeriesCollection.Add(new ColumnSeries
                    {
                        Title = item.Category,
                        Values = new ChartValues<double> { (double)item.Quantity }
                    });
                }

                Labels = categoryQuantities.Select(x => AddSpacesToLabel(x.Category)).ToArray();
                Formatter = value => value.ToString("N");
            }
        }
        private string AddSpacesToLabel(string label)
        {
            // Add spaces between each character in the label
            return string.Join(" ", label.Select(c => c.ToString()));
        }

        public decimal GetMonthlyOrderTotal()
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
            decimal monthlyTotal = db.Orders
                .Where(o => o.OrderDate.HasValue && o.OrderDate.Value.Month == DateTime.Today.Month && o.OrderDate.Value.Year == DateTime.Today.Year)
                .Sum(o => (decimal?)o.OrderTotal) ?? 0;

            return monthlyTotal;
        }

        public decimal GetDailyOrderTotal()
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
            decimal dailyTotal = db.Orders
               .Where(o => DbFunctions.TruncateTime(o.OrderDate) == DateTime.Today)
                .Sum(o => (decimal?)o.OrderTotal) ?? 0;

            return dailyTotal;
        }
    }
}
