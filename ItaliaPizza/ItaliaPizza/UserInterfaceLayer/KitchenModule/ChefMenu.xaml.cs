using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
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

namespace ItaliaPizza.UserInterfaceLayer.KitchenModule
{
    /// <summary>
    /// Interaction logic for ChefMenu.xaml
    /// </summary>
    public partial class ChefMenu : Page
    {
        Page page;
        public ChefMenu(Page page)
        {
            InitializeComponent();
            this.page = page;
        }

        private void BtnRecipes_Click(object sender, RoutedEventArgs e)
        {
            RecipesView recipesView = new RecipesView();
            page.NavigationService.Navigate(recipesView);
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            bool isWaiter = false;
            SearchInternalOrderView orderView = new SearchInternalOrderView(isWaiter);
            page.NavigationService.Navigate(orderView);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSingleton.Instance.Clear();
            LoginView loginView = new LoginView();
            page.NavigationService.Navigate(loginView);
        }

        private void BtnAddInternalProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductRegisterView productRegisterView = new ProductRegisterView();
            productRegisterView.SetRegisterForInternalProduct();
            page.NavigationService.Navigate(productRegisterView);
        }
    }
}
