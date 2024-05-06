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
    /// Interaction logic for SupplyOrderRemoveUC.xaml
    /// </summary>
    public partial class SupplyOrderRemoveUC : UserControl
    {
        public SupplyOrderView SupplyOrderView { get; set; }
        private Supply supplyData;
        public SupplyOrderRemoveUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            int fontSize = 25;
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            txtAmount.FontSize = fontSize;
            txtName.FontSize = fontSize;
            txtUnit.FontSize = fontSize;
            txtAmount.Visibility = Visibility.Visible;
            brdChangeAmount.Visibility = Visibility.Collapsed;
            btnRemoveSupply.Visibility = Visibility.Hidden;
        }

        public void SetSupplyData(Supply supply)
        {
            txtAmount.Visibility = Visibility.Collapsed;
            supplyData = supply;
            txtName.Text = supply.name;
            txtChangeAmount.Text = GetAmountData();
            txtUnit.Text = supply.measurementUnit;
        }

        public string GetAmountData()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            decimal orderedQuantity = supplyOrderDAO.GetOrderedQuantityBySupplierOrderId(this.SupplyOrderView.OrderId, supplyData.name);
            return orderedQuantity.ToString();
        }

        private void BtnRemoveSupply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TxtChangeAmountChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
