using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
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
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for SupplyProductCardUC.xaml
    /// </summary>
    public partial class SupplyProductCardUC : UserControl
    {
        public InventoryView InventoryView { get; set; }
        private Supply supplyData;
        public SupplyProductCardUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtArea.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            btnEdit.Visibility = Visibility.Hidden;
        }

        public void SetSupplyData(Supply supply)
        {
            supplyData = supply;
            txtName.Text = supply.name;
            txtAmount.Text = supply.amount.ToString();
            txtArea.Text = supply.SupplyArea.area_name;
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
            txtArea.Foreground = Brushes.Red;
            txtUnit.Foreground = Brushes.Red;
        }


        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            SupplyEditView supplyEditView = new SupplyEditView();
            supplyEditView.SetModifySupply(supplyData);
            this.InventoryView.NavigationService.Navigate(supplyEditView);
        }
    }
}
