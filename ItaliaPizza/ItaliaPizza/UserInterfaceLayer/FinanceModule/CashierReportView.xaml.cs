using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO;
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
    /// Interaction logic for CashierReportView.xaml
    /// </summary>
    public partial class CashierReportView : Page
    {
        private OrderDAO orderDAO = new OrderDAO();
        private CashoutDAO cashoutDAO = new CashoutDAO();
        private SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();

        public CashierReportView()
        {
            InitializeComponent();
            SetReportDataInPage();
        }

        private void SetReportDataInPage()
        {
            DateTime dateTime = DateTime.Now;

            txtCashierName.Text = "Corte efectuado por: " + UserSingleton.Instance.Name;
            txtDate.Text = "Fecha de corte actual: " + dateTime.ToShortDateString();

            decimal totalOrders = orderDAO.GetSumOfTotalOrdersByDate(dateTime.Day, dateTime.Month, dateTime.Year);
            txtOrders.Text = FormatCurrency(totalOrders);
            decimal cashin = GetCashinTotal(dateTime);
            txtCashin.Text = FormatCurrency(cashin);
            decimal totalCashin = totalOrders + cashin;
            txtTotalCashin.Text = FormatCurrency(totalCashin);

            decimal totalSupplyOrders = supplyOrderDAO.GetSumOfTotalSupplierOrdersByDate(dateTime.Day, dateTime.Month, dateTime.Year);
            txtSupplierOrder.Text = FormatCurrency(totalSupplyOrders);

            decimal cashout = GetCashoutTotal(dateTime);
            txtCashout.Text = FormatCurrency(cashout);
            decimal totalCashout = totalSupplyOrders + cashout;
            txtTotalCashout.Text = FormatCurrency(totalCashout);
            decimal totalCash = totalCashin - (totalCashout);
            txtTotal.Text = FormatCurrency(totalCash);
        }

        private decimal GetCashinTotal(DateTime dateTime)
        {
            byte cashinType = 1;
            decimal totalCashin = cashoutDAO.GetTotalCashoutsByDateAndType(dateTime.Day, dateTime.Month, dateTime.Year, cashinType);
            return totalCashin;
        }

        private decimal GetCashoutTotal(DateTime dateTime)
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            byte cashoutType = 0;
            decimal totalCashout = cashoutDAO.GetTotalCashoutsByDateAndType(dateTime.Day, dateTime.Month, dateTime.Year, cashoutType);
            return totalCashout;
        }

        private string FormatCurrency(decimal amount)
        {
            return "$" + amount.ToString("0.00");
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnDownloadReport_Click(object sender, RoutedEventArgs e)
        {
            // Lógica para descargar el reporte, si es necesario
        }
    }
}
