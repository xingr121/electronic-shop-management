using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for Reports.xaml
    /// </summary>
    public partial class Reports : UserControl
    {
        public Reports()
        {
            InitializeComponent();
            // Fetch data from the database and populate the chart
            PopulateChart();
            PopulateDoughnutChart();
            // Set DataContext to this instance to enable binding
            DataContext = this;
        }

        // SeriesCollection to store data for the PieChart
        public SeriesCollection SeriesCollection { get; set; }

        // DoughnutSeriesCollection to store data for the DoughnutChart
        public SeriesCollection DoughnutSeriesCollection { get; set; }

        // Function to populate the chart with data from the database
        private void PopulateChart()
        {
            // Initialize SeriesCollection
            SeriesCollection = new SeriesCollection();

            // Assume you have a DbContext instance called dbContext for interacting with the database
            // You need to replace dbContext with your actual DbContext instance
            using (var dbContext = new ElectronicShopManagementDBEntities())
            {
                // Query the database to get the sum of OrderQty for each product category
                var categoryQuantities = dbContext.OrderItems
                    .GroupBy(oi => oi.Product.ProdCategory) // Group by product category
                    .Select(g => new { Category = g.Key, TotalQuantity = g.Sum(oi => oi.OrderQty) }) // Select category and total quantity
                    .ToList();

                // Iterate through the categoryQuantities and add them to SeriesCollection
                foreach (var categoryQuantity in categoryQuantities)
                {
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = categoryQuantity.Category, // Use category name as title
                        Values = new ChartValues<ObservableValue> { new ObservableValue((double)categoryQuantity.TotalQuantity) },
                        DataLabels = true
                    });
                }
            }
        }
        // Function to populate the DoughnutChart with data from the database
        private void PopulateDoughnutChart()
        {
            // Initialize DoughnutSeriesCollection
            DoughnutSeriesCollection = new SeriesCollection();

            // Assume you have a DbContext instance called dbContext for interacting with the database
            // You need to replace dbContext with your actual DbContext instance
            using (var dbContext = new ElectronicShopManagementDBEntities())
            {
                // Query the database to get the sum of OrderTotal for each product category
                var categoryOrderTotals = dbContext.OrderItems
                    .GroupBy(oi => oi.Product.ProdCategory) // Group by product category
                    .Select(g => new { Category = g.Key, TotalOrderAmount = g.Sum(oi => oi.Order.OrderTotal) }) // Select category and total order amount
                    .ToList();

                // Iterate through the categoryOrderTotals and add them to DoughnutSeriesCollection
                foreach (var categoryOrderTotal in categoryOrderTotals)
                {
                    DoughnutSeriesCollection.Add(new PieSeries
                    {
                        Title = categoryOrderTotal.Category, // Use category name as title
                        Values = new ChartValues<ObservableValue> { new ObservableValue((double)categoryOrderTotal.TotalOrderAmount) },
                        DataLabels = true
                    });
                }
            }
        }
        // Event handler for data click on the PieChart
        private void PieChart_DataClick(object sender, LiveCharts.ChartPoint chartPoint)
        {
            // Show a message box with the clicked category and its quantity
            MessageBox.Show($"Category: {chartPoint.SeriesView.Title}, Quantity: {chartPoint.Y}");
        }

        private void Filter_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}

