using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            GetAllSupplies();
        }

        private void BtnNewInventoryReport_Click(object sender, RoutedEventArgs e)
        {
            InventoryReport inventoryReport = new InventoryReport();
            this.NavigationService.Navigate(inventoryReport);
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
                    GetAllSupplies();
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
            SupplyUC supplyUC = new SupplyUC();
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
            SupplyUC supplyCard = new SupplyUC();
            supplyCard.InventoryView = this;
            supplyCard.SetSupplyData(supply);
            suppliesListView.Items.Add(supplyCard);
        }

        private void GetAllSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.GetAllSupplies();

            if (supplies.Count > 0)
            {
                ShowSupplies(supplies);

            }
            else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void GetSuppliesByStatus(bool status)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.GetSuppliesByStatus(status);

            if (supplies.Count > 0)
            {
                ShowSupplies(supplies);

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

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {
            GetAllSupplies();
            radioButtonActive.IsChecked = false;
            radioButtonInactive.IsChecked = false;
        }

        private void RadioButtonInactive_Checked(object sender, RoutedEventArgs e)
        {
            GetSuppliesByStatus(false);
            radioButtonAll.IsChecked = false;
            radioButtonActive.IsChecked = false;
        }

        private void RadioButtonActive_Checked(object sender, RoutedEventArgs e)
        {
            GetSuppliesByStatus(true);
            radioButtonInactive.IsChecked = false;
            radioButtonAll.IsChecked = false;
        }
    }
}