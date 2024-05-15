using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
using ItaliaPizza.UserInterfaceLayer.UsersModule;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for CashReconciliationHistory.xaml
    /// </summary>
    public partial class CashReconciliationHistory : Page
    {
        public CashReconciliationHistory()
        {
            InitializeComponent();
            menuFrame.Content = new TellerMenu(this);
            GetAllLogs();
        }

        private void BtnCash_Click(object sender, RoutedEventArgs e)
        {
            RegisterCashoutView cashoutView = new RegisterCashoutView();
            NavigationService.Navigate(cashoutView);
        }

        private void BtnNewCashierLogReport_Click(object sender, RoutedEventArgs e)
        {
            CashierReportView cashierReportView = new CashierReportView();
            NavigationService.Navigate(cashierReportView);
        }

        private void ShowCalendar(object sender, RoutedEventArgs e)
        {
            if (sender is DatePicker datePicker)
            {
                datePicker.IsDropDownOpen = true;
            }
        }

        private void DatePickerStart_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            DatePicker datePickerStart = sender as DatePicker;
            if (datePickerStart != null)
            {
                DateTime selectedStartDate = datePickerStart.SelectedDate ?? DateTime.MinValue;
                GetLogsByDate(selectedStartDate);
            }
        }

        private void GetLogsByDate(DateTime selectedDate)
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            List<CashierLog> logs = cashoutDAO.GetCashierLogsByDateRange(selectedDate);

            if (logs.Count > 0)
            {
                ShowLogs(logs);

            }
            else
            {
                ShowNoLogsMessage();
            }
        }

        private void GetAllLogs()
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            List<CashierLog> logs = cashoutDAO.GetCashierLogs();

            if (logs.Count > 0)
            {
                ShowLogs(logs);

            }
            else
            {
                ShowNoLogsMessage();
            }
        }

        private void ShowNoLogsMessage()
        {
            cashierLogsListBox.Items.Clear();
            Label lblNoItems = new Label();
            lblNoItems.Style = (Style)FindResource("NoItemsLabelStyle");
            lblNoItems.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoItems.VerticalAlignment = VerticalAlignment.Center;
            cashierLogsListBox.Items.Add(lblNoItems);
        }

        private void ShowLogs(List<CashierLog> logs)
        {
            cashierLogsListBox.Items.Clear();
            CashierLogUC cashierLogUC = new CashierLogUC();
            cashierLogUC.SetTitleData();
            cashierLogsListBox.Items.Add(cashierLogUC);

            foreach (CashierLog log in logs)
            {
                AddLogToList(log);
            }
        }

        private void AddLogToList(CashierLog log)
        {
            CashierLogUC cashierLogUC = new CashierLogUC();
            cashierLogUC.SetCashierLogData(log);
            cashierLogsListBox.Items.Add(cashierLogUC);
        }
    }
}
