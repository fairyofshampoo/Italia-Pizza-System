using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
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
    
    public partial class InternalOrdersUC : UserControl
    {
        public SearchInternalOrderView searchInternalOrderView { get; set;}
        private InternalOrder orderData;

        public InternalOrdersUC()
        {
            InitializeComponent();
        }

        public void ShowInternalOrderDataByWaiter(InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            lblTotal.Text = "$ " + order.total.ToString();
            lblDate.Text = "Creado: " + order.date.ToString("dd/MM/yyyy HH:mm");

            if(order.status == Constants.ORDER_STATUS_PENDING_PREPARATION)
            {
                btnEditOrder.Visibility = Visibility.Visible;
                btnCancel.Visibility = Visibility.Visible;
            } else
            {
                btnViewDetails.Visibility = Visibility.Visible;
            }

            if(order.status == Constants.ORDER_STATUS_PREPARED)
            {
                btnReceived.Visibility = Visibility.Visible;
            }

            orderData = order;
        }

        public void ShowInternalOrderByChef (InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            lblTotal.Text = "$ " + order.total.ToString();
            lblDate.Text = "Creado: " + order.date.ToString("dd/MM/yyyy HH:mm");
            btnViewDetails.Visibility = Visibility.Visible;
            orderData = order;
        }

        private void BtnEditInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            //Se debe hacer referencia a la pantalla de editar
        }

        private void BtnViewDetails_Click(object sender, RoutedEventArgs e)
        {
            ShowProductsByOrderView showProductsByOrderView = new ShowProductsByOrderView(orderData.internalOrderId);
            searchInternalOrderView.NavigationService.Navigate(showProductsByOrderView);
            //Mostrar todos los productos de la orden para que se pueda preparar
        }

        private void BtnReceived_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSent_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
