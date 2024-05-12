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
            lblTotal.Content = order.total;
            lblDate.Content = order.date;
            btnEditOrder.Visibility = Visibility.Visible;
            imgDollarIcon.Visibility = Visibility.Visible; 
            orderData = order;
        }

        public void ShowInternalOrderByChef (InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            lblTotal.Content = order.total;
            lblDate.Content = order.date;
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
    }
}
