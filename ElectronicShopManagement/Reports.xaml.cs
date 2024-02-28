using LiveCharts;
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
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
            // Get the selected start and end dates from the DatePickers
            DateTime startDate = StartDatePicker.SelectedDate ?? DateTime.MinValue;
            DateTime endDate = EndDatePicker.SelectedDate ?? DateTime.MaxValue;

            // Verify if the start date is earlier than the end date
            if (startDate > endDate)
            {
                MessageBox.Show("Please select a valid date range. The start date cannot be later than the end date.");
                return;
            }

            // Update the charts with data based on the selected date range
            UpdateCharts(startDate, endDate);
        }

        private void UpdateCharts(DateTime startDate, DateTime endDate)
        {
            // Initialize SeriesCollection and DoughnutSeriesCollection
            SeriesCollection = new SeriesCollection();
            DoughnutSeriesCollection = new SeriesCollection();

            // Fetch data from the database based on the selected date range
            using (var dbContext = new ElectronicShopManagementDBEntities())
            {
                // Query the database to get the sum of OrderQty and OrderTotal for each product category within the selected date range
                var categoryQuantities = dbContext.OrderItems
                    .Where(oi => oi.Order.OrderDate >= startDate && oi.Order.OrderDate <= endDate) // Filter by date range
                    .GroupBy(oi => oi.Product.ProdCategory)
                    .Select(g => new { Category = g.Key, TotalQuantity = g.Sum(oi => oi.OrderQty), TotalOrderAmount = g.Sum(oi => oi.Order.OrderTotal) })
                    .ToList();

                // Populate SeriesCollection and DoughnutSeriesCollection with the filtered data
                foreach (var categoryQuantity in categoryQuantities)
                {
                    // Populate SeriesCollection
                    SeriesCollection.Add(new PieSeries
                    {
                        Title = categoryQuantity.Category,
                        Values = new ChartValues<ObservableValue> { new ObservableValue((double)categoryQuantity.TotalQuantity) },
                        DataLabels = true
                    });

                    // Populate DoughnutSeriesCollection
                    DoughnutSeriesCollection.Add(new PieSeries
                    {
                        Title = categoryQuantity.Category,
                        Values = new ChartValues<ObservableValue> { new ObservableValue((double)categoryQuantity.TotalOrderAmount) },
                        DataLabels = true
                    });
                }
            }

            // Update DataContext to refresh the binding
            DataContext = null;
            DataContext = this;
        }

        private void ExportToCSVButton_Click(object sender, RoutedEventArgs e)
        {
            // Gather data from the pie chart
            var pieChartData = SeriesCollection.Select(series =>
            {
                // Sum the values
                double total = 0;
                foreach (var value in series.Values)
                {
                    var numericValue = value.GetType().GetProperty("Value").GetValue(value);
                    total += Convert.ToDouble(numericValue);
                }

                return new string[] { series.Title, total.ToString(), "" }; // Empty string for Total Sale
            }).ToList();

            // Gather data from the doughnut chart for Total Sale
            var doughnutTotalSaleData = DoughnutSeriesCollection.Select(series =>
            {
                // Sum the values
                double total = 0;
                foreach (var value in series.Values)
                {
                    var numericValue = value.GetType().GetProperty("Value").GetValue(value);
                    total += Convert.ToDouble(numericValue);
                }

                return total.ToString(); // Only return the total sale amount
            }).ToList();

            // Combine pie chart data with doughnut chart's total sale data
            for (int i = 0; i < pieChartData.Count; i++)
            {
                pieChartData[i][2] = doughnutTotalSaleData[i]; // Set the third column (index 2) to the corresponding total sale from doughnut chart
            }

            // Add column titles
            string[] columnTitles = new string[] { "Category", "Quantity", "Total Sale" };

            // Add title to the CSV file
            string title = "Sales Report"; // Change this to your desired title
            string filePath = "report_data.csv";
            ExportToCSV(filePath, pieChartData, columnTitles, title);
        }

        private void ExportToCSV(string filePath, List<string[]> data, string[] columnTitles, string title)
        {
            try
            {
                // Write data to CSV file
                using (StreamWriter sw = new StreamWriter(filePath, false, Encoding.UTF8))
                {
                    // Write column titles to the CSV file
                    sw.WriteLine(string.Join(",", columnTitles));

                    // Write the data rows to the CSV file
                    foreach (var row in data)
                    {
                        sw.WriteLine(string.Join(",", row));
                    }
                }

                MessageBox.Show("Data exported successfully to CSV file.");
                Process.Start(filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data to CSV file: {ex.Message}");
            }
        }
    }
}

