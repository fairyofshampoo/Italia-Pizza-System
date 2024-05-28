using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Linq;
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
            radioButtonAll.IsChecked = true;
            menuFrame.Content = new ManagerMenu(this);
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
            if (searchText.Length > 3 && searchText.Length < 100)
            {
                SearchItemByName(searchText);

            }
            else
            {
                if (string.IsNullOrEmpty(searchText))
                {
                    GetAllSupplies();
                }
            }
        }

        private void SearchItemByName(string searchText)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.SearchSupplyByName(searchText);
            ShowInventory(supplies);
        }

        private void ShowInventory(List<Supply> supplies)
        {
            suppliesListView.Items.Clear();
            SupplyProductCardUC supplyUC = new SupplyProductCardUC();
            supplyUC.InventoryView = this;
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (Supply item in supplies)
            {
                AddSupplyToList(item);
            }
        }

        private void AddSupplyToList(Supply item)
        {
            SupplyProductCardUC supplyCard = new SupplyProductCardUC();
            supplyCard.InventoryView = this;
            supplyCard.SetSupplyData(item);
            suppliesListView.Items.Add(supplyCard);
        }

        private void GetAllSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.GetAllSupplies();

            if (supplies.Count > 0)
            {
                ShowInventory(supplies);

            }
            else
            {
                ShowNoItemsMessage();
            }
        }

        private void GetSupplyByStatus(bool status)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.GetSuppliesByStatus(status);

            if (supplies.Count > 0)
            {
                ShowInventory(supplies);

            }
            else
            {
                ShowNoItemsMessage();
            }
        }

        private void ShowNoItemsMessage()
        {
            suppliesListView.Items.Clear();
            Label lblNoItems = new Label();
            lblNoItems.Style = (Style)FindResource("NoItemsLabelStyle");
            lblNoItems.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoItems.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoItems);
        }

        private void RadioButtonAll_Checked(object sender, RoutedEventArgs e)
        {
            GetAllSupplies();
            radioButtonActive.IsChecked = false;
            radioButtonInactive.IsChecked = false;
        }

        private void RadioButtonInactive_Checked(object sender, RoutedEventArgs e)
        {
            GetSupplyByStatus(false);
            radioButtonAll.IsChecked = false;
            radioButtonActive.IsChecked = false;
        }

        private void RadioButtonActive_Checked(object sender, RoutedEventArgs e)
        {
            GetSupplyByStatus(true);
            radioButtonInactive.IsChecked = false;
            radioButtonAll.IsChecked = false;
        }
    }
}