using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public partial class Customer
    {
        public Customer()
        {
        }

        public Customer(string custName, string custAddress, string custPhone)
        {
            CustName = custName;
            CustAddress = custAddress;
            CustPhone = custPhone;
        }
    }
}
