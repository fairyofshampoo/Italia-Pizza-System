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
        //private InternalOrder internalOrderData;

        public SearchInternalOrderView searchInternalOrderView { get; set;}

        public InternalOrdersUC()
        {
            InitializeComponent();
        }

        public void ShowInternalOrderData(InternalOrder order)
        {
            lblOrderNumber.Content = "Número del pedido: " + order.internalOrderId;
            string waiterName = GetWaiterName(order.waiterEmail);
            lblWaiter.Content = waiterName;
            lblTotal.Content = order.total;
            lblDate.Content = order.date;
            lblTime.Content = order.time;
        }

        private string GetWaiterName(string waiterName)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.GetEmployeeNameByEmail(waiterName);
        }

        private void BtnEditInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            //Se debe hacer referencia a la pantalla de editar
        }
    }
}
