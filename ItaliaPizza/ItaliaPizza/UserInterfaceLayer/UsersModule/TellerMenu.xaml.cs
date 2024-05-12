using ItaliaPizza.ApplicationLayer;
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
        private Page page;
        public TellerMenu(Page page)
        {
            InitializeComponent();
            this.page = page;
        }

        private void BtnLogout_Click(object sender, RoutedEventArgs e)
        {
            UserSingleton.Instance.Clear();
            LoginView loginView = new LoginView();
            page.NavigationService.Navigate(loginView);
        }

        private void BtnOrders_Click(object sender, RoutedEventArgs e)
        {
            //Aún no existe jiji
        }

        private void BtnTransaction_Click(object sender, RoutedEventArgs e)
        {
            //cortes de caja
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            TellerScreenView tellerScreenView = new TellerScreenView();
            page.NavigationService.Navigate(tellerScreenView);
        }
    }
}
