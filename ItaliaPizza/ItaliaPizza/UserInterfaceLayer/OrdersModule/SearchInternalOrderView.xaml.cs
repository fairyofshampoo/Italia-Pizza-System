﻿using ItaliaPizza.DataLayer;
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

        public SearchInternalOrderView(bool isAWaiter)
        {
            InitializeComponent();

            if(isAWaiter)
            {
                ShowOrderForWaiter();
            }
            else
            {
                menuFrame.Content = new ChefMenu(this);
                ShowOrderForChef();
            }

        }

        private void ShowOrderForWaiter()
        {
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

        private void ShowOrderForChef()
        {
            List<InternalOrder> orders = GetOrdersForPreapartion();
        }

        private void VerifyOrders(List<InternalOrder> orders)
        {

        }

        private List<InternalOrder> GetOrdersForPreapartion()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetOrdersForPreapartion();
            return internalOrders;
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
            internalOrders = internalOrderDAO.GetInternalOrdersByStatus(1, waiterEmail);
            return internalOrders;
        }

        private void SearchInternalOrderByCode(string  orderCode)
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
            SearchInternalOrderByStatus(1);
        }

        private void BtnPreparingOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatus(2);
        }

        private void BtnFinishedOrder_Click(object sender, RoutedEventArgs e)
        {
            SearchInternalOrderByStatus(3);
        }

        private void SearchInternalOrderByStatus(int status)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrder> internalOrders = internalOrderDAO.GetInternalOrdersByStatus(status, waiterEmail);
            ShowInternalOrders(internalOrders);
        }

        private void BtnAddInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            //Mandar a llamar la pantalpara hacer otra orden
        }

    }
}
