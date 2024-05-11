using ItaliaPizza.UserInterfaceLayer.UsersModule;
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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for ManagerMenu.xaml
    /// </summary>
    public partial class ManagerMenu : Page
    {
        private Page page;
        public ManagerMenu(Page page)
        {
            InitializeComponent();
            this.page = page;
        }

        private void BtnEmployees_Click(object sender, RoutedEventArgs e)
        {
            EmployeesView employeesView = new EmployeesView();
            page.NavigationService.Navigate(employeesView);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
