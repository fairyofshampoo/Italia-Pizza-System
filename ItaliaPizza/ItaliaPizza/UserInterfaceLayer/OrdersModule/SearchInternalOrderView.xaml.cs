using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    /*
    public partial class SearchInternalOrderView : Page
    {
        private string waiterEmail = "lalo@gmail.com"; //Esto debe ser cambiado por el singleton

        private int rowsAdded = 0;

        public SearchInternalOrderView()
        {
            InitializeComponent();
            List<InternalOrder> orders = GetInternalOrder();
            if (orders.Any())
            {
                ShowInternalOrders(orders);
            }
            else
            {
                //Mostrar Mesnaje avisando que no hay ordenes registradas
            }
        }

        private void ShowInternalOrders(List<InternalOrder> orders)
        {
            rowsAdded = 0;
            OrdersGrid.Children.Clear();
            OrdersGrid.RowDefinitions.Clear();

            if (orders.Any())
            {
                foreach (InternalOrder order in orders)
                {
                    AddOrders(order);
                }
            }

        } 

        private void AddOrders(InternalOrder order)
        {
            InternalOrdersUC orderCard = new InternalOrdersUC();    
            orderCard.searchInternalOrderView = this;
            Grid.SetRow(orderCard, rowsAdded);
            orderCard.ShowInternalOrderData(order);
            OrdersGrid.Children.Add(orderCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            OrdersGrid.RowDefinitions.Add(row);

        }

        private List<InternalOrder> GetInternalOrder()
        {
            List<InternalOrder> internalOrders = new List<InternalOrder>();
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            internalOrders = internalOrderDAO.GetInternalOrdersByStatus("open", waiterEmail);
            return internalOrders;
        }

        private void txtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;

        }

        private void SearchInternalOrderByCode(int  orderCode)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            InternalOrder internalOrder = internalOrderDAO.GetInternalOrdersByNumber(orderCode, waiterEmail);
            if (internalOrder != null)
            {
                List<InternalOrder> internalOrders = new List<InternalOrder>();
                internalOrders.Add(internalOrder); 
                ShowInternalOrders(internalOrders);
            } else
            {
                //Aquí se muestra el label
            }
        }

        private void BtnOpenOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatus("Open");
        }

        private void BtnPreparingOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatus("Preparing");
        }

        private void BtnFinishedOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatus("Finished");
        }

        private void SearchInternalOrderByStatus(string status)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(status, waiterEmail);
            ShowInternalOrders(internalOrders);
        }

        private void BtnAddInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            //Mandar a llamar la pantalpara hacer otra orden
        }

        private void BtnSearchOrder_Click(object sender, RoutedEventArgs e)
        {
            string search = txtSearchBar.Text;
            bool band = true;
            foreach (char caracter in search)
            {
                if (!char.IsDigit(caracter))
                {
                    band = false; break;    
                }
            }
            if(band)
            {
                int orderCode = int.Parse(search);
                InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
                InternalOrder internalOrder = internalOrderDAO.GetInternalOrdersByNumber(orderCode, waiterEmail);
                if (internalOrder != null)
                {
                    List<InternalOrder> internalOrders = new List<InternalOrder> { internalOrder };
                    ShowInternalOrders(internalOrders);
                }
            }
        }
    }
    */
}
