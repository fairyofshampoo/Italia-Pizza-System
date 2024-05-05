using ItaliaPizza.DataLayer;
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
    /// Interaction logic for SupplyOrderAddUC.xaml
    /// </summary>
    public partial class SupplyOrderAddUC : UserControl
    {
        public SupplyOrderView SupplyOrderView { get; set; }
        private Supply supplyData;
        public SupplyOrderAddUC()
        {
            InitializeComponent();
        }
        public void SetTitleData()
        {
            int fontSize = 25;
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtSupplyArea.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            txtAmount.FontSize = fontSize;
            txtName.FontSize = fontSize;
            txtSupplyArea.FontSize = fontSize;
            txtUnit.FontSize = fontSize;
            btnAddSupply.Visibility = Visibility.Hidden;
        }
        public void SetSupplyData(Supply supply)
        {
            supplyData = supply;
            txtName.Text = supply.name;
            txtAmount.Text = supply.amount.ToString();
            txtSupplyArea.Text = supply.SupplyArea.area_name;
            txtUnit.Text = supply.measurementUnit;
        }

        private void BtnAddSupply_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
