﻿using ItaliaPizza.ApplicationLayer;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplyOrder.xaml
    /// </summary>
    public partial class SupplyOrderView : Page
    {
        private Supplier supplierData;
        private bool isAnUpdate;
        public int OrderId { get; set; }
        private float totalPayment;
        public SupplyOrderView()
        {
            InitializeComponent();
            GetSupplies();
        }

        public void SetSupplyOrderData(SupplierOrder supplierOrder, bool isAnUpdate)
        {
            this.isAnUpdate = isAnUpdate;
            SetSupplier(supplierOrder.Supplier);
            SetResumeOrderTitle();

            if (isAnUpdate)
            {
                SetOrderSupplies();
            } else {

                GenerateSupplierOrder();
            }
        }

        private void SetResumeOrderTitle()
        {
            orderListView.Items.Clear();
            SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
            supplyCard.SupplyOrderView = this;
            supplyCard.SetTitleData();
            orderListView.Items.Add(supplyCard);
        }

        private void SetOrderSupplies()
        {
            orderListView.Items.Clear();
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<Supply> suppliesInOrder = supplyOrderDAO.GetSuppliesByOrderId(this.OrderId);
            foreach (Supply supply in suppliesInOrder)
            {
                AddSupplyToResume(supply);
                Console.WriteLine(supply.name);
            }
        }

        private void AddSupplyToResume(Supply supply)
        {
            SupplyOrderRemoveUC supplyCard = new SupplyOrderRemoveUC();
            supplyCard.SupplyOrderView = this;
            supplyCard.SetSupplyData(supply);
            orderListView.Items.Add(supplyCard);
        }

        private void SetSupplier(Supplier supplier)
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
        private void CalculateTotalPayment()
        {

        }

        private void GetSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetSuppliesByStatus(true);

            if(availableSupplies.Count > 0 )
            {
                ShowSupplies(availableSupplies);

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

        private void ShowSupplies(List<Supply> suppliesList)
        {
            suppliesListView.Items.Clear();
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
            string searchText = txtSearchBar.Text;
            if (searchText.Length > 3)
            {
                SearchSupplyByName(searchText);

            } else
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

        public void AddSupplyToOrder(Supply supply)
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            string supplyName = supply.name;
            decimal defaultAmount = 1;
            bool result = supplyOrderDAO.AddSupplyToOrder(supplyName, OrderId, defaultAmount);
            Console.WriteLine("RESULTADO DE ADD SUPPLY TO ORDER " + result.ToString());
            if (result)
            {
                SetOrderSupplies();
            }
        }

        public void IncreaseAmount(Supply supply)
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            bool result = supplyOrderDAO.IncreaseSupplyAmountInOrder(supply, OrderId);

            if (result)
            {
                SetResumeOrderTitle();
                SetOrderSupplies();
            }
        }

        private void GenerateSupplierOrder()
        {
            SupplyOrderDAO supplierDAO = new SupplyOrderDAO();
            int result = supplierDAO.AddSupplierOrder(SetNewSupplierOrder());
            if(result != Constants.UNSUCCESSFUL_RESULT || result != Constants.EXCEPTION_RESULT)
            {
                Console.WriteLine("CODIGO ORDEN: " + result);
                this.OrderId = result;
            } else
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }
        }

        private SupplierOrder SetNewSupplierOrder()
        {
            DateTime currentDate = DateTime.Today;
            TimeSpan time = new TimeSpan(currentDate.Hour, currentDate.Minute, currentDate.Second);
            SupplierOrder newSupplierOrder = new SupplierOrder()
            {
                status = Constants.INACTIVE_STATUS,
                date = currentDate,
                time = time,
                total = 0,
                modificationDate = currentDate,
                supplierId = supplierData.email,
            };

            return newSupplierOrder;
        }
    }
}
