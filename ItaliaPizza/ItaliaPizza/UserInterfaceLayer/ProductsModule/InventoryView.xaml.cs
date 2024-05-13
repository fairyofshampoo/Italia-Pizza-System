using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;
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
                    GetAllSuppliesAndExternalProducts();
                }
            }
        }

        private void SearchItemByName(string searchText)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<object> suppliesAndProducts = supplyDAO.SearchSupplyOrExternalProductByName(searchText);
            ShowInventory(suppliesAndProducts);
        }

        private void ShowInventory(List<object> suppliesAndProducts)
        {
            suppliesListView.Items.Clear();
            SupplyProductCardUC supplyUC = new SupplyProductCardUC();
            supplyUC.InventoryView = this;
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (object item in suppliesAndProducts)
            {
                AddSupplyToList(item);
            }
        }

        private void AddSupplyToList(object item)
        {
            SupplyProductCardUC supplyCard = new SupplyProductCardUC();
            supplyCard.InventoryView = this;
            supplyCard.SetObjectData(item);
            suppliesListView.Items.Add(supplyCard);
        }

        private void GetAllSuppliesAndExternalProducts()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<object> suppliesAndProducts = supplyDAO.GetAllSuppliesAndExternalProducts();

            if (suppliesAndProducts.Count > 0)
            {
                ShowInventory(suppliesAndProducts);

            }
            else
            {
                ShowNoItemsMessage();
            }
        }

        private void GetItemsByStatus(bool status)
        {
            byte productStatus = 0;
            if (status)
            {
                productStatus = 1;
            }
            SupplyDAO supplyDAO = new SupplyDAO();
            List<object> suppliesAndProducts = supplyDAO.GetSupplyOrExternalProductByStatus(status, productStatus);

            if (suppliesAndProducts.Count > 0)
            {
                ShowInventory(suppliesAndProducts);

            }
            else
            {
                ShowNoItemsMessage();
            }
        }

        private void GetAllSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> supplies = supplyDAO.GetAllSupplies();
            List<object> suppliesAsObjects = supplies.Cast<object>().ToList();

            if (suppliesAsObjects.Count > 0)
            {
                ShowInventory(suppliesAsObjects);
            }
            else
            {
                ShowNoSuppliesMessage();
            }
        }

        private void GetAllExternalProducts()
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> products = productDAO.GetAllExternalProducts();
            List<object> productsAsObjects = products.Cast<object>().ToList();

            if (productsAsObjects.Count > 0)
            {
                ShowInventory(productsAsObjects);
            }
            else
            {
                ShowNoProductsMessage();
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

        private void ShowNoProductsMessage()
        {
            suppliesListView.Items.Clear();
            Label lblNoProducts = new Label();
            lblNoProducts.Style = (Style)FindResource("NoProductsLabelStyle");
            lblNoProducts.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoProducts.VerticalAlignment = VerticalAlignment.Center;
            suppliesListView.Items.Add(lblNoProducts);
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
            GetAllSuppliesAndExternalProducts();
            radioButtonActive.IsChecked = false;
            radioButtonInactive.IsChecked = false;
            radioButtonProducts.IsChecked = false;
            radioButtonSupplies.IsChecked = false;
        }

        private void RadioButtonInactive_Checked(object sender, RoutedEventArgs e)
        {
            GetItemsByStatus(false);
            radioButtonAll.IsChecked = false;
            radioButtonActive.IsChecked = false;
            radioButtonProducts.IsChecked = false;
            radioButtonSupplies.IsChecked = false;
        }

        private void RadioButtonActive_Checked(object sender, RoutedEventArgs e)
        {
            GetItemsByStatus(true);
            radioButtonInactive.IsChecked = false;
            radioButtonAll.IsChecked = false;
            radioButtonProducts.IsChecked = false;
            radioButtonSupplies.IsChecked = false;
        }

        private void RadioButtonSupplies_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonActive.IsChecked = false;
            radioButtonInactive.IsChecked = false;
            radioButtonProducts.IsChecked = false;
            radioButtonAll.IsChecked = false;
            GetAllSupplies();
        }

        private void RadioButtonProducts_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonActive.IsChecked = false;
            radioButtonInactive.IsChecked = false;
            radioButtonSupplies.IsChecked = false;
            radioButtonAll.IsChecked = false;
            GetAllExternalProducts();
        }
    }
}