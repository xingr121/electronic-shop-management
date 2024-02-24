using System;
using System.Linq;
using System.Windows;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        // Create an instance of Entity Framework DbContext
        private readonly ElectronicShopManagementDBEntities _dbContext = new ElectronicShopManagementDBEntities();

        public Login()
        {
            InitializeComponent();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Query the Employees table using LINQ to Entities
                var employee = _dbContext.Employees
                    .FirstOrDefault(emp => emp.EmpName == txtEmpName.Text && emp.EmpPassword == txtPassword.Password);

                if (employee != null)
                {
                    if (employee.Role == "admin")
                    {
                        GlobalPro.userType = "A";
                    }else if (employee.Role == "user")
                    {
                        GlobalPro.userType = "U";
                    }
                    // Login successful
                    MainWindow dashboard = new MainWindow();
                    dashboard.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Employee Name or Password is incorrect.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}