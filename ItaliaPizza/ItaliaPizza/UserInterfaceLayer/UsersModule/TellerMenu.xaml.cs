using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
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

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    /// <summary>
    /// Interaction logic for TellerMenu.xaml
    /// </summary>
    public partial class TellerMenu : Page
    {
        private Page pageData;
        public TellerMenu(Page page)
        {
            InitializeComponent();
            this.pageData = page;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSingleton.Instance.Clear();
            LoginView loginView = new LoginView();
            pageData.NavigationService.Navigate(loginView);
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            SearchHomeOrderView searchHomeOrderView = new SearchHomeOrderView();
            pageData.NavigationService.Navigate(searchHomeOrderView);
        }

        private void BtnTransaction_Click(object sender, RoutedEventArgs e)
        {
            CashReconciliationHistory cashReconciliation = new CashReconciliationHistory();
            pageData.NavigationService.Navigate(cashReconciliation);
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            TellerScreenView tellerScreenView = new TellerScreenView();
            pageData.NavigationService.Navigate(tellerScreenView);
        }
    }
}
