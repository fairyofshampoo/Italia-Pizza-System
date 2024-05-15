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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for CashierLogUC.xaml
    /// </summary>
    public partial class CashierLogUC : UserControl
    {
        CashierLog CashierLogData { get; set; }
        public CashierLogUC()
        {
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
            txtDate.Text = cashierLog.creationDate.ToString();
            txtCashin.Text = cashierLog.totalCashin.ToString();
            txtCashout.Text = cashierLog.totalCashout.ToString();
            txtTotal.Text = cashierLog.total.ToString();

        }
        private void BtnWatch_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
