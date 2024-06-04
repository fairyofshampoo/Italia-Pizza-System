using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    public partial class InternalOrdersUC : UserControl
    {
        public Page PageView { get; set; }
        public InternalOrder orderData;
        private bool isWaiter;

        public InternalOrdersUC()
        {
            InitializeComponent();
        }

        public void ShowInternalOrderDataByWaiter(InternalOrder order)
        {
            DisplayOrderInformation(order);
            Console.WriteLine("ESTADO" + order.status);
            isWaiter = true;
            UpdateStatusUI(order.status);
            txtName.Text = UserSingleton.Instance.Name;
            orderData = order;
        }

        public void ShowHomeOrderData(InternalOrder order)
        {
            DisplayOrderInformation(order);
            txtName.Text = GetClientName(order.clientEmail);
            isWaiter = false;
            UpdateStatusUI(order.status);
            orderData = order;
        }

        private void DisplayOrderInformation(InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            lblDate.Text = "Creado: " + order.date.ToString("dd/MM/yyyy HH:mm");
            lblStatus.Content = GetOrderStatusDescription(order.status);
        }

        private string GetClientName(string clientEmail)
        {
            ClientDAO clientDAO = new ClientDAO();
            string result = null;
            try
            {
                result = clientDAO.GetClientName(clientEmail);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


            return result ;
        }

        private string GetOrderStatusDescription(int status)
        {
            Console.WriteLine("ESTADO" + status);
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
                case Constants.ORDER_STATUS_PREPARING:
                    return "Preparando";
                default:
                    return "Estado desconocido";
            }
        }

        private void UpdateStatusUI(int status)
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
            ShowProductsByOrderView showProductsByOrderView = new ShowProductsByOrderView(orderData.internalOrderId, orderData.status);
            PageView.NavigationService.Navigate(showProductsByOrderView);
        }

        private void BtnReceived_Click(object sender, RoutedEventArgs e)
        {
            OrderDAO orderDAO = new OrderDAO();
            try
            {
                if (orderDAO.ChangeOrderStatus(Constants.ORDER_STATUS_DELIVERED, orderData.internalOrderId))
                {
                    UpdateStatusUI(Constants.ORDER_STATUS_DELIVERED);
                }
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            OrderDAO orderDAO = new OrderDAO();
            try
            {
                orderDAO.CancelOrder(orderData.internalOrderId);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private void BtnSent_Click(object sender, RoutedEventArgs e)
        {
            OrderDAO orderDAO = new OrderDAO();
            try
            {
                if (orderDAO.ChangeOrderStatus(Constants.ORDER_STATUS_SENT, orderData.internalOrderId))
                {
                    UpdateStatusUI(Constants.ORDER_STATUS_SENT);
                }
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }
        }
    }
}