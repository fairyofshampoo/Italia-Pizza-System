using ItaliaPizza.ApplicationLayer;
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
        private readonly Supplier supplierData;
        public SupplierOrderHistory(Supplier supplierData)
        {
            InitializeComponent();
            this.supplierData = supplierData;
            radioButtonAll.IsChecked = true;
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
            SupplierOrderUC orderCard = new SupplierOrderUC(this);
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

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonCanceled.IsChecked = false;
            radioButtonOpen.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            ShowAllOrders();
        }


        private void RadioButtonOpen_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonCanceled.IsChecked = false;
            radioButtonAll.IsChecked = false;
            radioButtonReceived.IsChecked = false;
            ShowOpenOrders();
        }

        private void ShowOpenOrders()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<SupplierOrder> orders = supplyOrderDAO.GetOrdersBySupplierIdAndStatus(supplierData.email, Constants.ACTIVE_STATUS);
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

        private void RadioButtonReceived_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonCanceled.IsChecked = false;
            radioButtonAll.IsChecked = false;
            radioButtonOpen.IsChecked = false;
            ShowReceivedOrders();
        }

        private void ShowReceivedOrders()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<SupplierOrder> orders = supplyOrderDAO.GetOrdersBySupplierIdAndStatus(supplierData.email, Constants.COMPLETE_STATUS);
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

        private void RadioButtonCanceled_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonReceived.IsChecked = false;
            radioButtonAll.IsChecked = false;
            radioButtonOpen.IsChecked = false;
            ShowCanceledOrder();
        }

        private void ShowCanceledOrder()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<SupplierOrder> orders = supplyOrderDAO.GetOrdersBySupplierIdAndStatus(supplierData.email, Constants.INACTIVE_STATUS);
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

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void GetOrdersWithinDateRange(DateTime startDate, DateTime endDate)
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<SupplierOrder> supplyOrders = supplyOrderDAO.GetOrdersBySupplierIdAndCreationDateRange(supplierData.email, startDate, endDate);
            ShowOrdersByDateRange(supplyOrders);
        }

        private void ShowOrdersByDateRange(List<SupplierOrder> orders)
        {
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

        private void DatePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePickerStart = sender as DatePicker;
            if (datePickerStart != null)
            {
                DateTime selectedStartDate = datePickerStart.SelectedDate ?? DateTime.MinValue;
                DateTime selectedEndDate = datePickerEnd.SelectedDate ?? DateTime.MaxValue;

                if (selectedStartDate > selectedEndDate)
                {
                    selectedStartDate = (selectedEndDate).Date;
                }
                else
                {
                    GetOrdersWithinDateRange(selectedStartDate, selectedEndDate);
                }
            }
        }

        private void DatePickerEnd_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePickerEnd = sender as DatePicker;
            if (datePickerEnd != null)
            {
                DateTime selectedEndDate = datePickerEnd.SelectedDate ?? DateTime.MaxValue;
                DateTime selectedStartDate = datePickerStart.SelectedDate ?? DateTime.MinValue;

                if (selectedEndDate < selectedStartDate)
                {
                    selectedEndDate = selectedStartDate.Date.AddDays(1).AddSeconds(-1);
                }
                else
                {
                    selectedEndDate = selectedEndDate.Date.AddDays(1).AddSeconds(-1);
                    GetOrdersWithinDateRange(selectedStartDate, selectedEndDate);
                }
            }
        }

        private void ShowCalendar(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                datePicker.IsDropDownOpen = true;
            }
        }
    }
}
