using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
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
using ItaliaPizza.ApplicationLayer;

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
            radioButtonAll.IsChecked = true;
        }

        private List<InternalOrder> GetAllOrders()
        {
            List<InternalOrder> orders = new List<InternalOrder>();
            OrderDAO orderDAO = new OrderDAO();
            orders = orderDAO.GetHomeOrdersWithNonZeroStatus();
            return orders;
        }

        private void SearchHomeOrderByStatus(int status)
        {
            OrderDAO orderDAO = new OrderDAO();
            List<InternalOrder> orders = orderDAO.GetHomeOrdersByStatus(status);
            ShowOrders(orders);
        }

        private void ShowOrders(List<InternalOrder> orders)
        {
            ordersListBox.Items.Clear();

            if (orders.Any())
            {
                foreach (InternalOrder order in orders)
                {
                    InternalOrdersUC orderCard = new InternalOrdersUC();
                    orderCard.PageView = this;
                    orderCard.ShowHomeOrderData(order);
                    ordersListBox.Items.Add(orderCard);
                }
            } else
            {
                ShowNoItemsMessage();
            }
        }

        private void ShowNoItemsMessage()
        {
            ordersListBox.Items.Clear();
            Label lblNoItems = new Label();
            lblNoItems.Style = (Style)FindResource("NoItemsLabelStyle");
            lblNoItems.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoItems.VerticalAlignment = VerticalAlignment.Center;
            ordersListBox.Items.Add(lblNoItems);
        }

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonPendingPreparation.IsChecked = false;
            radioButtonPrepared.IsChecked = false;
            radioButtonPreparing.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            radioButtonSent.IsChecked = false;
            ShowOrders(GetAllOrders());
        }

        private void RadioButtonPendingPreparation_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonAll.IsChecked = false;
            radioButtonPrepared.IsChecked = false;
            radioButtonPreparing.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            radioButtonSent.IsChecked = false;
            SearchHomeOrderByStatus(Constants.ORDER_STATUS_PENDING_PREPARATION);
        }

        private void RadioButtonReceived_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonAll.IsChecked = false;
            radioButtonPrepared.IsChecked = false;
            radioButtonPreparing.IsChecked = false;
            radioButtonPendingPreparation.IsChecked = false;
            radioButtonSent.IsChecked = false;
            SearchHomeOrderByStatus(Constants.ORDER_STATUS_DELIVERED);
        }

        private void RadioButtonSent_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonAll.IsChecked = false;
            radioButtonPrepared.IsChecked = false;
            radioButtonPreparing.IsChecked = false;
            radioButtonPendingPreparation.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            SearchHomeOrderByStatus(Constants.ORDER_STATUS_SENT);
        }

        private void RadioButtonPrepared_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonAll.IsChecked = false;
            radioButtonSent.IsChecked = false;
            radioButtonPreparing.IsChecked = false;
            radioButtonPendingPreparation.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            SearchHomeOrderByStatus(Constants.ORDER_STATUS_PREPARED);
        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchBar.Text;
            if (searchText.Length > 3 && searchText.Length < 100)
            {
                SearchOrderByClientName(searchText);

            }
            else
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    radioButtonAll.IsChecked = true;
                }
            }
        }

        private void SearchOrderByClientName(string searchText)
        {
            OrderDAO orderDAO = new OrderDAO();
            List<InternalOrder> orders = orderDAO.SearchOrderByClientName(searchText);
            ShowOrders(orders);
        }

        private void RadioButtonPreparing_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonAll.IsChecked = false;
            radioButtonSent.IsChecked = false;
            radioButtonPrepared.IsChecked = false;
            radioButtonPendingPreparation.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            SearchHomeOrderByStatus(Constants.ORDER_STATUS_PREPARING);
        }
    }
}
