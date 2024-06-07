﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    public partial class ProductsView : Page
    {
        private int rowsAdded = 0;

        public ProductsView()
        {
            InitializeComponent();
            SetLastProducts();
            menuFrame.Content = new ManagerMenu(this);
        }

        private void BtnAllFilter_Click(object sender, RoutedEventArgs e)
        {
            btnExternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            SetLastProducts();
        }

        private void BtnInternalFilter_Click(object sender, RoutedEventArgs e)
        {
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnExternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            SearchProductByType(Constants.INTERNAL_PRODUCT);
        }

        private void BtnExternalFilter_Click(object sender, RoutedEventArgs e)
        {
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnExternalFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));
            SearchProductByType(Constants.EXTERNAL_PRODUCT);
        }       

        private void TxtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;
            if (searchText.Length > 3)
            {
                SearchProductByName(searchText);
            }
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductRegisterView productRegisterView = new ProductRegisterView();
            NavigationService.Navigate(productRegisterView);
        }

        private void SetLastProducts()
        {
            List<Product> products = GetLastProducts();
            if (products.Any())
            {
                ShowProducts(products);
                txtSearchBar.IsReadOnly = false;
            }
            else
            {
                txtSearchBar.IsReadOnly= true;
                DialogManager.ShowErrorMessageBox("No hay productos registrados");
            }
        }

        private void ShowProducts(List<Product> products)
        {
            rowsAdded = 0;
            ProductsGrid.Children.Clear();
            ProductsGrid.RowDefinitions.Clear();

            if (products.Any())
            {
                lblProductNotFound.Visibility = Visibility.Collapsed;
                foreach (Product product in products)
                {
                    AddProducts(product);
                }
            }
            else
            {
                lblProductNotFound.Visibility= Visibility.Visible;
            }
        }

        private List<Product> GetLastProducts()
        {
            List<Product> lastProducts = new List<Product>();
            ProductDAO productDAO = new ProductDAO();
            try
            {
                lastProducts = productDAO.GetLastProductsRegistered();
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }
            return lastProducts;
        }    
        
        private void AddProducts(Product product)
        {
            ProductUC productCard = new ProductUC();
            productCard.ProductsView = this;
            Grid.SetRow(productCard, rowsAdded);
            productCard.SetDataCards(product);
            ProductsGrid.Children.Add(productCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            ProductsGrid.RowDefinitions.Add(row);
        }

        private void SearchProductByName(string name)
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> products = new List<Product>();
            try
            {
                products = productDAO.SearchProductByName(name);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


            ShowProducts(products);
        }

        private void SearchProductByType(int type)
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> products = productDAO.SearchProductByType(type);
            ShowProducts(products);
        }
    }
}
