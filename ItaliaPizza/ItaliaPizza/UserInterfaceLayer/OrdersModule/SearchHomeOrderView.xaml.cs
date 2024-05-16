using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
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
    /// Interaction logic for SearchHomeOrderView.xaml
    /// </summary>
    public partial class SearchHomeOrderView : Page
    {
        public SearchHomeOrderView()
        {
            InitializeComponent();
            menuFrame.Content = new TellerMenu(this);
        }

        private List<InternalOrder> GetAllOrders()
        {
            List<InternalOrder> internalOrders = new List<InternalOrder>();
            OrderDAO internalOrderDAO = new OrderDAO();
            return internalOrders;
        }
        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonPendingPreparation_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonReceived_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonSent_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonPrepared_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
