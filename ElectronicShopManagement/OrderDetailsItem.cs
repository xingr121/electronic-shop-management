using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public class OrderDetailsItem
    {
        public int OrderId { get; set; }
        public string CustName { get; set; }
        public decimal? OrderTotal { get; set; }
        public DateTime? OrderDate { get; set; }
        public string EmpName { get; set; }
        public int? OrderQty { get; set; }
        public string ProdName { get; set; }
        public decimal? PriceAtPurchase { get; set; }
    }
}
