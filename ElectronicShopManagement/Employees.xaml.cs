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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for Employees.xaml
    /// </summary>
    public partial class Employees : Page
    {
        public Employees()
        {
            InitializeComponent();
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
            var emps = from e in db.Employees select e;
            this.LvEmployees.ItemsSource = emps.ToList();
        }

        private void CmUpdade_Click(object sender, RoutedEventArgs e)
        {
            Employee currSelectedEmployee = LvEmployees.SelectedItem as Employee;
            if (currSelectedEmployee == null) return;
            AddEditEmployee dialog = new AddEditEmployee(currSelectedEmployee);
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var emps = from em in db.Employees select em;
                this.LvEmployees.ItemsSource = emps.ToList();
            }
        }

        private void CmDelete_Click(object sender, RoutedEventArgs e)
        {
            Employee currSelectedEmployee = LvEmployees.SelectedItem as Employee;
            if (currSelectedEmployee == null) return;
            Window parentWindow = Window.GetWindow(this); // Get the parent window
            var result = MessageBox.Show(parentWindow, "are you sure to delete this item", "confirm deletion", MessageBoxButton.YesNo, MessageBoxImage.Question
                );
            if (result == MessageBoxResult.No) return;
            try
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var existingEmployee = db.Employees.Find(currSelectedEmployee.EmpId);
                if (existingEmployee == null)
                {
                    MessageBox.Show(parentWindow, "The selected employee no longer exists.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                db.Employees.Remove(existingEmployee);
                db.SaveChanges();

                var emps = from em in db.Employees select em;
                this.LvEmployees.ItemsSource = emps.ToList();
            }
            catch (SystemException ex)
            {
                MessageBox.Show(parentWindow, ex.Message, "Database Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void TxbFilter_TextChanged(object sender, TextChangedEventArgs e)
        {
            ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();

            if (FilterBy.Text == "Name")
            {
                var emps = from em in db.Employees where em.EmpName.Contains(TxbFilter.Text) select em;
                this.LvEmployees.ItemsSource = emps.ToList();
            }
            else if (FilterBy.Text == "Phone")
            {
                var emps = from em in db.Employees where em.EmpPhone.Contains(TxbFilter.Text) select em;
                this.LvEmployees.ItemsSource = emps.ToList();
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            AddEditEmployee dialog = new AddEditEmployee();
            Window parentWindow = Window.GetWindow(this);
            dialog.Owner = parentWindow;
            if (dialog.ShowDialog() == true)
            {
                ElectronicShopManagementDBEntities db = new ElectronicShopManagementDBEntities();
                var emps = from em in db.Employees select em;
                this.LvEmployees.ItemsSource = emps.ToList();
            }
        }
    }
}
