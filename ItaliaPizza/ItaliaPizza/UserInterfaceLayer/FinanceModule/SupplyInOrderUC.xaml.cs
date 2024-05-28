using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
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
    /// Interaction logic for SupplyInOrderUC.xaml
    /// </summary>
    public partial class SupplyInOrderUC : UserControl
    {
        private Supply supplyData;
        public SupplyInOrderUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
        }

        public void SetSupplyData(Supply supply)
        {
            supplyData = supply;
            txtName.Text = supply.name;
            txtAmount.Text = supply.amount.ToString();
            txtUnit.Text = supply.measurementUnit;
            if (!supplyData.status.HasValue || !supplyData.status.Value)
            {
                SetRedText();
            }

        }

        private void SetRedText()
        {
            txtName.Foreground = Brushes.Red;
            txtAmount.Foreground = Brushes.Red;
            txtUnit.Foreground = Brushes.Red;
        }
    }
}
