using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public class OrderInfo
    {
        public string CustomerName { get; set; }
        public string ProductName { get; set; }
        public int OrderQuantity { get; set; }

        public decimal PriceAtPurchase { get; set; }

        public DateTime OrderDate { get; set; }
        public string EmployeeName { get; set; }

        public decimal OrderTotal { get; set; }

        // Constructor to initialize the OrderDate property
        public OrderInfo()
        {
            OrderDate = DateTime.Now; // Set default to current date/time
        }
    }
}
