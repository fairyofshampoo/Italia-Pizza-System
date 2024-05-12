using ItaliaPizza.ApplicationLayer;
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

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    /// <summary>
    /// Interaction logic for WaiterMenu.xaml
    /// </summary>
    public partial class WaiterMenu : Page
    {
        private Page page;
        public WaiterMenu(Page page)
        {
            InitializeComponent();
            this.page = page;
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            bool isWaiter = true;
            SearchInternalOrderView orderView = new SearchInternalOrderView(isWaiter);
            page.NavigationService.Navigate(orderView);
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSingleton.Instance.Clear();
            LoginView loginView = new LoginView();
            page.NavigationService.Navigate(loginView);
        }
    }
}
