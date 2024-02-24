using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ElectronicShopManagement
{
    public partial class Employee
    {
        public Employee() { }

        public Employee(string empName, string empPassword, string empAddress, string empPhone)
        {
            EmpName = empName;
            EmpPassword = empPassword;
            EmpAddress = empAddress;
            EmpPhone = empPhone;
        }
    }
}
