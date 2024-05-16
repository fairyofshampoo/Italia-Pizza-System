using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    public partial class InternalOrdersUC : UserControl
    {
        public Page PageView { get; set; }
        private InternalOrder orderData;

        public InternalOrdersUC()
        {
            InitializeComponent();
        }

        public void ShowInternalOrderDataByWaiter(InternalOrder order)
        {
            DisplayOrderInformation(order);
            Console.WriteLine("ESTADO" + order.status);
            UpdateStatusUI(order.status, true);
            txtName.Text = UserSingleton.Instance.Name;
            orderData = order;
        }

        public void ShowHomeOrderData(InternalOrder order)
        {
            DisplayOrderInformation(order);
            txtName.Text = GetClientName(order.clientEmail);
            UpdateStatusUI(order.status, false);
            orderData = order;
        }

        private void DisplayOrderInformation(InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            lblTotal.Text = "$ " + order.total.ToString("0.00");
            lblDate.Text = "Creado: " + order.date.ToString("dd/MM/yyyy HH:mm");
            lblStatus.Content = GetOrderStatusDescription(order.status);
        }

        private string GetClientName(string clientEmail)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.GetClientName(clientEmail);
        }

        private string GetOrderStatusDescription(int status)
        {
            switch (status)
            {
                case Constants.ORDER_STATUS_PENDING_PREPARATION:
                    return "En espera de preparación";
                case Constants.ORDER_STATUS_PREPARED:
                    return "Preparado";
                case Constants.ORDER_STATUS_SENT:
                    return "Enviado";
                case Constants.ORDER_STATUS_DELIVERED:
                    return "Recibido";
                default:
                    return "Estado desconocido";
            }
        }

        private void UpdateStatusUI(int status, bool isWaiter)
        {
            ResetUI();
            switch (status)
            {
                case Constants.ORDER_STATUS_PENDING_PREPARATION:
                    btnEditOrder.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    break;
                case Constants.ORDER_STATUS_PREPARED:
                    if (isWaiter)
                    {
                        btnViewDetails.Visibility = Visibility.Visible;
                        btnReceived.Visibility = Visibility.Visible;
                    } else
                    {
                        btnViewDetails.Visibility = Visibility.Visible;
                        btnSent.Visibility = Visibility.Visible;
                    }
                    break;
                case Constants.ORDER_STATUS_SENT:
                    btnViewDetails.Visibility = Visibility.Visible;
                    btnReceived.Visibility = Visibility.Visible;
                    break;
                case Constants.ORDER_STATUS_DELIVERED:
                    btnViewDetails.Visibility = Visibility.Visible;
                    break;
            }
        }

        private void ResetUI()
        {
            btnEditOrder.Visibility = Visibility.Collapsed;
            btnViewDetails.Visibility = Visibility.Collapsed;
            btnCancel.Visibility = Visibility.Collapsed;
            btnReceived.Visibility = Visibility.Collapsed;
            btnSent.Visibility = Visibility.Collapsed;
        }

        public void ShowInternalOrderByChef(InternalOrder order)
        {
            DisplayOrderInformation(order);
            txtName.Visibility = Visibility.Hidden;
            btnViewDetails.Visibility = Visibility.Visible;
            orderData = order;
        }

        private void BtnEditInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            RegisterOrderView registerOrderView = new RegisterOrderView(orderData.clientEmail != null);
            registerOrderView.SetEditionData(orderData);
            PageView.NavigationService.Navigate(registerOrderView);
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            ShowProductsByOrderView showProductsByOrderView = new ShowProductsByOrderView(orderData.internalOrderId);
            PageView.NavigationService.Navigate(showProductsByOrderView);
        }

        private void BtnReceived_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to mark the order as received
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to cancel the order
        }

        private void BtnSent_Click(object sender, RoutedEventArgs e)
        {
            // Implement the logic to mark the order as sent
        }
    }
}