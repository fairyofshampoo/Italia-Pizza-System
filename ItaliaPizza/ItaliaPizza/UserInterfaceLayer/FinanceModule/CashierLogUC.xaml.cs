using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
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
using System.Xml.Linq;
using ItaliaPizza.DataLayer.DAO.Interface;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for CashierLogUC.xaml
    /// </summary>
    public partial class CashierLogUC : UserControl
    {
        CashierLog CashierLogData { get; set; }
        CashReconciliationHistory cashReconciliation;
        public CashierLogUC(CashReconciliationHistory reconciliationHistory)
        {
            this.cashReconciliation = reconciliationHistory;
            InitializeComponent();
        }

        public void SetTitleData()
        {
            txtDate.FontWeight = FontWeights.Bold;
            txtCashin.FontWeight = FontWeights.Bold;
            txtCashout.FontWeight = FontWeights.Bold;
            txtTotal.FontWeight = FontWeights.Bold;
            btnWatch.Visibility = Visibility.Hidden;
        }

        public void SetCashierLogData(CashierLog cashierLog)
        {
            CashierLogData = cashierLog;
            decimal totalSupplyOrders = CashierLogData.supplierOrderCashout;
            decimal cashout = CashierLogData.miscellaneousCashout;
            decimal totalCashout = totalSupplyOrders + cashout;
            decimal totalOrders = CashierLogData.ordersCashin;
            decimal cashin = CashierLogData.miscellaneousCashin ?? 0;
            decimal totalCashin = totalOrders + cashin;
            txtDate.Text = cashierLog.openingDate.ToString();
            txtCashin.Text = totalCashin.ToString();
            txtCashout.Text = totalCashout.ToString();
            txtTotal.Text = cashierLog.finalBalance.ToString();

        }
        private void BtnWatch_Click(object sender, RoutedEventArgs e)
        {
            BalanceLogView balanceLogView = new BalanceLogView(CashierLogData);
            this.cashReconciliation.NavigationService.Navigate(balanceLogView);
        }
    }
}
