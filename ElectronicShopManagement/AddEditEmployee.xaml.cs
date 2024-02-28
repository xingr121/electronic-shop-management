using System;
using System.Collections.Generic;
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
    /// Interaction logic for AddEditEmployee.xaml
    /// </summary>
    public partial class AddEditEmployee : Window
    {
        Employee currEmployee;
        public AddEditEmployee(Employee currEmployee=null)
        {
           this.currEmployee = currEmployee;
            InitializeComponent();
            if (currEmployee != null)
            {
                Title = "Update Employee";
                NameInput.Text = currEmployee.EmpName;
                Password.Text = currEmployee.EmpPassword;
                AddressInput.Text = currEmployee.EmpAddress;
                PhoneInput.Text = currEmployee.EmpPhone;

                BtnSave.Content = "Update";
            }
            else
            {
                Title = "Add New Employee";
                BtnSave.Content = "Add";
            }
        }
        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();



                if (currEmployee != null)
                {
                    Employee existingEmployee = db.Employees.FirstOrDefault(c => c.EmpId == currEmployee.EmpId);

                    if (existingEmployee != null)
                    {
                        existingEmployee.EmpName = NameInput.Text;
                        existingEmployee.EmpPassword = Password.Text;
                        existingEmployee.EmpAddress = AddressInput.Text;
                        existingEmployee.EmpPhone = PhoneInput.Text;



                    }
                    else
                    {
                        throw new InvalidOperationException("Employee not found in the database.");
                    }
                }
                else
                {

                    Employee newEmp = new Employee(NameInput.Text, Password.Text,AddressInput.Text, PhoneInput.Text);

                    db.Employees.Add(newEmp);
                }

                db.SaveChanges();
                this.DialogResult = true;
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show(this, ex.Message, "Input Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (SystemException ex)
            {
                MessageBox.Show(this, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    
    }
}

