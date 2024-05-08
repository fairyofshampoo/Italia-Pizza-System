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
using ItaliaPizza.UserInterfaceLayer.FinanceModule;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for InventoryView.xaml
    /// </summary>
    public partial class InventoryView : Page
    {
        public InventoryView()
        {
            InitializeComponent();
            GetSupplies();
        }

        private void BtnNewInventoryReport_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddSupply_Click(object sender, RoutedEventArgs e)
        {
            SupplyRegister supplierRegisterView = new SupplyRegister();
            this.NavigationService.Navigate(supplierRegisterView);
        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchBar.Text;
            if (searchText.Length > 3)
            {
                SearchSupplyByName(searchText);

            }
            else
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    GetSupplies();
                }
            }
        }

        private void SearchSupplyByName(string searchText)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.SearchSupplyByName(searchText);
            ShowSupplies(supplies);
        }

        private void ShowSupplies(List<Supply> suppliesList)
        {
            suppliesListView.Items.Clear();
            SupplyAddUC supplyUC = new SupplyAddUC();
            supplyUC.InventoryView = this;
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (Supply supply in suppliesList)
            {
                AddSupplyToList(supply);
            }
        }

        private void AddSupplyToList(Supply supply)
        {
            SupplyAddUC supplyCard = new SupplyAddUC();
            supplyCard.InventoryView = this;
            supplyCard.SetSupplyData(supply);
            suppliesListView.Items.Add(supplyCard);
        }

        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetAllSupplies();

            if (availableSupplies.Count > 0)
            {
                ShowSupplies(availableSupplies);

            }
            else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void ShowNoSuppliesMessage()
        {
            suppliesListView.Items.Clear();
            Label lblNoSupplies = new Label();
            lblNoSupplies.Style = (Style)FindResource("NoSuppliesLabelStyle");
            lblNoSupplies.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoSupplies.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoSupplies);
        }

    }
}
