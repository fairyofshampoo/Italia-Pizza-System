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

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for SupplyAddUC.xaml
    /// </summary>
    public partial class SupplyAddUC : UserControl
    {
        public InventoryView InventoryView { get; set; }
        private Supply supplyData;
        public SupplyAddUC()
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
            Console.WriteLine(supply.name + " ESTADO:" + supply.status);
            if (!supplyData.status.HasValue || !supplyData.status.Value)
            {
                SetRedText();
            }

        }

        private void SetRedText()
        {
            txtName.Foreground = Brushes.Red;
            txtAmount.Foreground = Brushes.Red;
            txtSupplyArea.Foreground = Brushes.Red;
            txtUnit.Foreground = Brushes.Red;
        }


        private void BtnEditSupply_Click(object sender, RoutedEventArgs e)
        {
            SupplyEditView supplyEditView = new SupplyEditView();
            supplyEditView.SetModifySupply(supplyData);
            this.InventoryView.NavigationService.Navigate(supplyEditView);
        }
    }
}
