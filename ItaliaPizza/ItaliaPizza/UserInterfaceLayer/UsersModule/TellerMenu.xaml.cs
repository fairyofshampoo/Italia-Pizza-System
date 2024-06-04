using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
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
