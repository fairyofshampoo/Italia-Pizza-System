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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplyOrder.xaml
    /// </summary>
    public partial class SupplyOrderView : Page
    {
        private Supplier supplierData;
        public SupplyOrderView()
        {
            InitializeComponent();
            GetSupplies();
        }

        public void SetSupplier(Supplier supplier)
        {
            lblSupplierName.Text = supplier.manager + ": " + supplier.companyName;
            StringBuilder supplyAreasText = new StringBuilder();

            foreach (var supplyArea in supplier.SupplyAreas)
            {
                supplyAreasText.Append(supplyArea.area_name);
                supplyAreasText.Append("  ");
            }

            lblSupplyArea.Content = supplyAreasText.ToString();
            this.supplierData = supplier;
        }
        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetSuppliesByStatus(true);

            if(availableSupplies.Count > 0 )
            {
                SetSuppliesInPage(availableSupplies);
            } else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void ShowNoSuppliesMessage()
        {
            Label lblNoSupplies = new Label();
            lblNoSupplies.Style = (Style)FindResource("NoSuppliesLabelStyle");
            lblNoSupplies.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoSupplies.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoSupplies);
        }

        private void SetSuppliesInPage(List<Supply> suppliesList)
        {
            SupplyOrderAddUC supplyUC = new SupplyOrderAddUC();
            supplyUC.SupplyOrderView = this;
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (Supply supply in suppliesList)
            {
                AddSupplyToList(supply);
            }
        }

        private void AddSupplyToList(Supply supply)
        {
            SupplyOrderAddUC supplyCard = new SupplyOrderAddUC();
            supplyCard.SupplyOrderView = this;
            supplyCard.SetSupplyData(supply);
            suppliesListView.Items.Add(supplyCard);
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
