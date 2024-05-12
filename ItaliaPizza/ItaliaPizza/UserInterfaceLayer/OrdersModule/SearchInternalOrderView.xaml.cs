using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.KitchenModule;
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

    public partial class SearchInternalOrderView : Page
    {
        private string waiterEmail = "lalo@gmail.com"; //Esto debe ser cambiado por el singleton
        private int rowsAdded = 0;
        public bool isWaiter = false;

        public SearchInternalOrderView(bool isWaiter)
        {
            InitializeComponent();

            if (isWaiter)
            {
                ShowElemntsForWaiter();
                ShowOrderForWaiter();
            }
            else
            {
                menuFrame.Content = new ChefMenu(this);
                ShowElementsForChef();
                ShowOrderForChef();
            }

        }

        private void ShowElemntsForWaiter()
        {
            btnAddInternalOrder.Visibility = Visibility.Visible;
            btnFinishedOrder.Visibility = Visibility.Visible;
        }

        private void ShowElementsForChef()
        {
            btnRefreshPage.Visibility = Visibility.Visible;
        }

        private void ShowOrderForWaiter()
        {
            List<InternalOrder> orders = GetInternalOrder();

            VerifyOrders(orders);
        }

        private void ShowOrderForChef()
        {
            List<InternalOrder> orders = GetOrdersForPreapartion();
            VerifyOrders(orders);
        }

        private void VerifyOrders(List<InternalOrder> orders)
        {
            if (orders.Any())
            {
                ShowInternalOrders(orders);
            }
            else
            {
                //Mostrar Mesnaje avisando que no hay ordenes registradas
            }
        }

        private List<InternalOrder> GetOrdersForPreapartion()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(1);
            return internalOrders;
        }

        private void ShowInternalOrders(List<InternalOrder> orders)
        {
            rowsAdded = 0;
            OrdersGrid.Children.Clear();
            OrdersGrid.RowDefinitions.Clear();

            if (orders.Any())
            {
                if (isWaiter)
                {
                    foreach (InternalOrder order in orders)
                    {
                        AddOrdersWaiter(order);
                    }
                } 
                else
                {
                    foreach (InternalOrder order in orders)
                    {
                        AddOrdersChef(order);
                    }
                }
            }

        }

        private void AddOrdersWaiter(InternalOrder order)
        {
            InternalOrdersUC orderCard = new InternalOrdersUC();
            orderCard.searchInternalOrderView = this;
            Grid.SetRow(orderCard, rowsAdded);
            orderCard.ShowInternalOrderDataByWaiter(order);
            OrdersGrid.Children.Add(orderCard);
            rowsAdded++;
            RowDefinition row = new RowDefinition();
            OrdersGrid.RowDefinitions.Add(row);
        }

        private void AddOrdersChef(InternalOrder order)
        {
            InternalOrdersUC orderCard = new InternalOrdersUC();
            orderCard.searchInternalOrderView = this;
            Grid.SetRow(orderCard, rowsAdded);
            orderCard.ShowInternalOrderByChef(order);
            OrdersGrid.Children.Add(orderCard);
            rowsAdded++;
            RowDefinition row = new RowDefinition();
            OrdersGrid.RowDefinitions.Add(row);
        }

        private List<InternalOrder> GetInternalOrder()
        {
            List<InternalOrder> internalOrders = new List<InternalOrder>();
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            internalOrders = internalOrderDAO.GetInternalOrdersByStatusAndWaiter(1, waiterEmail);
            return internalOrders;
        }

        private void BtnOpenOrder_Click(object sender, RoutedEventArgs e)
        {
            if(isWaiter)
            {
                SearchInternalOrderByStatusAndWaiter(1);
            }
            else
            {
                SearchInternalOrderByStatus(1);
            }
            
        }

        private void BtnPreparingOrder_Click(object sender, RoutedEventArgs e)
        {
            if (isWaiter)
            {
                SearchInternalOrderByStatusAndWaiter(2);
            } 
            else
            {
                SearchInternalOrderByStatus(2);
            }
            
        }

        private void BtnFinishedOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatusAndWaiter(3);
        }

        private void SearchInternalOrderByStatusAndWaiter(int status)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatusAndWaiter(status, waiterEmail);
            ShowInternalOrders(internalOrders);
        }

        private void SearchInternalOrderByStatus (int status)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(status);
            ShowInternalOrders(internalOrders);
        }

        private void BtnAddInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            //Mandar a llamar la pantalla para hacer otra orden
        }

        private void BtnRefreshScreen_Click(object sender, RoutedEventArgs e)
        {
            ShowOrderForChef();
        }
    }
}
