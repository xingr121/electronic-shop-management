using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public partial class Order
    {
     

        public Order(string custName, string empName, decimal? orderTotal, string paymentId)
        {
            CustName = custName;
            EmpName = empName;
            OrderTotal = orderTotal;
            PaymentId = paymentId;
        }
    }

}
