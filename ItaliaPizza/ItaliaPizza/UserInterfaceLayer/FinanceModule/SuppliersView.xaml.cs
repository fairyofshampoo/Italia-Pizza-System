﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.UserInterfaceLayer.UsersModule;
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
    /// Interaction logic for SuppliersView.xaml
    /// </summary>
    public partial class SuppliersView : Page
    {
        private int rowsAdded = 0;
        private List<Button> dynamicFilterButtons = new List<Button>();
        public SuppliersView()
        {
            InitializeComponent();
            LoadButtons();
            SetSuppliers();
        }

        private void SetSuppliers()
        {
            List<Supplier> suppliers = GetSuppliers();
            if (suppliers.Any())
            {
                ShowSuppliers(suppliers);
                txtSearchBar.IsReadOnly = false;
            }
            else
            {
                txtSearchBar.IsReadOnly = true;
                scrollViewer.Visibility = Visibility.Collapsed;
                lblSupplierNotFound.Visibility = Visibility.Visible;
                lblSupplierNotFound.Content = "No hay proveedores registrados";
            }
        }

        private void ShowSuppliers(List<Supplier> suppliers)
        {
            rowsAdded = 0;
            SuppliersGrid.Children.Clear();
            SuppliersGrid.RowDefinitions.Clear();

            if (suppliers.Any())
            {
                scrollViewer.Visibility = Visibility.Visible;
                lblSupplierNotFound.Visibility = Visibility.Collapsed;
                foreach (Supplier supplier in suppliers)
                {
                    AddSupplierToGrid(supplier);
                }
            }
            else
            {
                scrollViewer.Visibility = Visibility.Collapsed;
                lblSupplierNotFound.Visibility = Visibility.Visible;
            }
        }

        private void AddSupplierToGrid(Supplier supplier)
        {
            SupplierUC supplierCard = new SupplierUC();
            supplierCard.SuppliersView = this;
            Grid.SetRow(supplierCard, rowsAdded);
            supplierCard.SetDataCards(supplier);
            SuppliersGrid.Children.Add(supplierCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            SuppliersGrid.RowDefinitions.Add(row);
        }

        private List<Supplier> GetSuppliers()
        {
            List<Supplier> suppliers = new List<Supplier>();
            SupplierDAO supplierDAO = new SupplierDAO();
            suppliers = supplierDAO.GetLastSuppliersRegistered();
            return suppliers;
        }

        private void LoadButtons()
        {
            List<SupplyArea> supplyAreas = GetAllSupplyAreas();

            Button btnAll = new Button();
            btnAll.Content = "Todos";
            btnAll.Style = (Style)FindResource("CustomButtonStyle");
            btnAll.Click += BtnAll_Click;
            filtersStackPanel.Children.Add(btnAll);
            dynamicFilterButtons.Add(btnAll);

            foreach (SupplyArea area in supplyAreas)
            {
                Button btnArea = new Button();
                btnArea.Content = area.area_name;
                btnArea.Style = (Style)FindResource("CustomButtonStyle");
                btnArea.Click += (sender, e) => BtnArea_Click(sender, e, area);
                filtersStackPanel.Children.Add(btnArea);
                dynamicFilterButtons.Add(btnArea);
            }
        }

        private List<SupplyArea> GetAllSupplyAreas()
        {
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();
            List<SupplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();
            return supplyAreas;
        }

        private void BtnArea_Click(object sender, RoutedEventArgs e, SupplyArea area)
        {
            Button clickedButton = (Button)sender;

            clickedButton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            foreach (Button button in dynamicFilterButtons)
            {
                if (button != clickedButton)
                {
                    button.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
                }
            }

            SearchSupplierByArea(area);
        }

        private void SearchSupplierByArea(SupplyArea area)
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            List<Supplier> suppliers = supplierDAO.SearchProductByArea(area.area_name);
            ShowSuppliers(suppliers);
        }

        private void BtnAll_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;

            clickedButton.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            foreach (Button button in dynamicFilterButtons)
            {
                if (button != clickedButton)
                {
                    button.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
                }
            }

            SetSuppliers();
        }

        private void TxtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = txtSearchBar.Text;
            if(searchText.Length > 3)
            {
                SearchSupplierByName(searchText);
            }
        }

        private void SearchSupplierByName(string searchText)
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            List<Supplier> suppliers = supplierDAO.SearchProductByName(searchText);
            ShowSuppliers(suppliers);
        }

        private void BtnAddSupplier_Click(object sender, RoutedEventArgs e)
        {
            SupplierRegisterView supplierRegisterView = new SupplierRegisterView();
            this.NavigationService.Navigate(supplierRegisterView);
        }
    }
}
