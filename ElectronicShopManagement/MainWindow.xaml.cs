using System.Windows;

namespace ElectronicShopManagement
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        private void Main_Loaded(object sender, RoutedEventArgs e)
        {
            
            Homepage homepage = new Homepage();

         
            Main.Content = homepage;
        }

        private void BtnP_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Products();
        }

        private void BtnC_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Customers();
        }

        private void BtnE_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Employees();
        }

        private void BtnHome_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Homepage();

        }

        private void BtnO_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new OrderPage3();
        }

        private void BtnReport_Click(object sender, RoutedEventArgs e)
        {
            Main.Content = new Reports();
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            Login newlogin = new Login();
            newlogin.Show();
            this.Hide();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (GlobalPro.userType == "U")
            {
                BtnE.Visibility = Visibility.Collapsed;
            }else if(GlobalPro.userType == "A")
            {
BtnE.Visibility = Visibility.Visible;
            }
        }
    }
}
