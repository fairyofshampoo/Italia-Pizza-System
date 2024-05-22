using ItaliaPizza.ApplicationLayer;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{

    public partial class SearchInternalOrderView : Page
    {
        private string waiterEmail = ApplicationLayer.UserSingleton.Instance.Email;
        public bool isWaiter = false;

        public SearchInternalOrderView(bool isWaiter)
        {
            InitializeComponent();
            this.isWaiter = isWaiter;
            if (isWaiter)
            {
                menuFrame.Content = new WaiterMenu(this);
                ShowElementsForWaiter();
                ShowOrderForWaiter();
            }
            else
            {
                menuFrame.Content = new ChefMenu(this);
                ShowElementsForChef();
                ShowOrderForChef();
            }

        }

        private void ShowElementsForWaiter()
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
            List<InternalOrder> orders = GetOrdersForPreparation();
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

        private List<InternalOrder> GetOrdersForPreparation()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(Constants.ORDER_STATUS_PENDING_PREPARATION);
            return internalOrders;
        }

        private void ShowInternalOrders(List<InternalOrder> orders)
        {
            ordersListBox.Items.Clear();

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
            orderCard.PageView = this;
            ordersListBox.Items.Add(orderCard);
            orderCard.ShowInternalOrderDataByWaiter(order);
        }

        private void AddOrdersChef(InternalOrder order)
        {
            InternalOrdersUC orderCard = new InternalOrdersUC();
            orderCard.PageView = this;
            orderCard.ShowInternalOrderByChef(order);
            ordersListBox.Items.Add(orderCard);
        }

        private List<InternalOrder> GetInternalOrder()
        {
            List<InternalOrder> internalOrders = new List<InternalOrder>();
            OrderDAO internalOrderDAO = new OrderDAO();
            internalOrders = internalOrderDAO.GetInternalOrdersByStatusAndWaiter(Constants.ORDER_STATUS_PENDING_PREPARATION, waiterEmail);
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
                SearchInternalOrderByStatusAndWaiter(Constants.ORDER_STATUS_PREPARING);
            } 
            else
            {
                SearchInternalOrderByStatus(Constants.ORDER_STATUS_PREPARING);
            }
            
        }

        private void BtnFinishedOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatusAndWaiter(3);
        }

        private void SearchInternalOrderByStatusAndWaiter(int status)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatusAndWaiter(status, waiterEmail);
            ShowInternalOrders(internalOrders);
        }

        private void SearchInternalOrderByStatus (int status)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(status);
            ShowInternalOrders(internalOrders);
        }

        private void BtnAddInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            RegisterOrderView registerOrderView = new RegisterOrderView(false);
            NavigationService.Navigate(registerOrderView);
        }

        private void BtnRefreshScreen_Click(object sender, RoutedEventArgs e)
        {
            ShowOrderForChef();
        }
    }
}
