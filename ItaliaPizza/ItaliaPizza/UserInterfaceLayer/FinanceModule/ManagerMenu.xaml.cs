using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
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
            UserSingleton.Instance.Clear();
            LoginView loginView = new LoginView();
            page.NavigationService.Navigate(loginView);
        }

        private void BtnInventory_Click(object sender, RoutedEventArgs e)
        {
            InventoryView inventoryView = new InventoryView();
            page.NavigationService.Navigate(inventoryView);

        }

        private void BtnProducts_Click(object sender, RoutedEventArgs e)
        {
            ProductsView productsView = new ProductsView();
            page.NavigationService.Navigate(productsView);
        }

        private void BtnSuppliers_Click(object sender, RoutedEventArgs e)
        {
            SuppliersView suppliersView = new SuppliersView();
            page.NavigationService.Navigate(suppliersView);
        }
    }
}
