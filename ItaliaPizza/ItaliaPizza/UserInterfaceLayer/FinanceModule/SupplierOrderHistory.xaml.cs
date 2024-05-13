using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierOrderHistory.xaml
    /// </summary>
    public partial class SupplierOrderHistory : Page
    {
        private int rowAdded = 0;
        private int columnsAdded = 0;
        private Supplier supplierData;
        public SupplierOrderHistory(Supplier supplierData)
        {
            InitializeComponent();
            this.supplierData = supplierData;
            ShowAllOrders();
        }

        private void ShowAllOrders()
        {
            List<SupplierOrder> orders = GetOrdersBySupplier();
            rowAdded = 0;
            columnsAdded = 0;
            ordersGrid.Children.Clear();
            ordersGrid.RowDefinitions.Clear();
            ordersGrid.ColumnDefinitions.Clear();

            if (orders.Any())
            {
                foreach (SupplierOrder order in orders)
                {
                    for (int index = 0; index < 3; index++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        column.Width = new GridLength(600);
                        ordersGrid.ColumnDefinitions.Add(column);
                    }

                    if (columnsAdded == 2)
                    {
                        columnsAdded = 0;
                        rowAdded++;
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(270);
                        ordersGrid.RowDefinitions.Add(row);
                    }

                    AddOrders(order);
                }
            }
        }

        private void AddOrders(SupplierOrder order)
        {
            SupplierOrderUC orderCard = new SupplierOrderUC();
            Grid.SetRow(orderCard, rowAdded);
            Grid.SetColumn(orderCard, columnsAdded);
            order.Supplier = supplierData;
            orderCard.SetDataCards(order);
            ordersGrid.Children.Add(orderCard);
            columnsAdded++;
        }

        private List<SupplierOrder> GetOrdersBySupplier()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            return supplyOrderDAO.GetOrdersBySupplierId(supplierData.email);
        }

        private void BtnAddSupplierOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonOpen_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonReceived_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void RadioButtonCanceled_Checked(object sender, RoutedEventArgs e)
        {

        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
