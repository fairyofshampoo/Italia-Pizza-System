using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
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
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierUC.xaml
    /// </summary>
    public partial class SupplierUC : UserControl
    {
        private Supplier SupplierData;
        public SuppliersView SuppliersView { get; set; }
        public SupplierUC()
        {
            InitializeComponent();
        }

        private void BtnSupplyOrder_Click(object sender, RoutedEventArgs e)
        {
            bool isAnUpdate = false;
            SupplierOrder supplierOrder = new SupplierOrder();
            supplierOrder.Supplier = SupplierData;
            SupplyOrderView supplyOrderView = new SupplyOrderView();
            supplyOrderView.SetSupplyOrderData(supplierOrder, isAnUpdate);
            SuppliersView.NavigationService.Navigate(supplyOrderView);
        }

        private void BtnEditSupplier_Click(object sender, RoutedEventArgs e)
        {
            EditSupplierView editSupplierView = new EditSupplierView();
            editSupplierView.SetSupplierData(SupplierData.email);
            SuppliersView.NavigationService.Navigate(editSupplierView);

        }

        public void SetDataCards(Supplier supplier)
        {
            lblSupplierName.Content = supplier.manager + ": " + supplier.companyName;
            lblPhoneNumber.Content = supplier.phone;
            lblEmail.Content = supplier.email;
            if(supplier.status == Constants.INACTIVE_STATUS)
            {
                btnNewOrder.Visibility = Visibility.Collapsed;
                lblSupplierName.Foreground = Brushes.Red;
            }

            StringBuilder supplyAreasText = new StringBuilder();
            foreach (var supplyArea in supplier.SupplyAreas)
            {
                supplyAreasText.Append(supplyArea.area_name);
                supplyAreasText.Append("  ");
            }

            lblSupplyArea.Content = supplyAreasText.ToString();
            SupplierData = supplier;
        }

        private void Supplier_Click(object sender, MouseButtonEventArgs e)
        {
            SupplierOrderHistory orderHistory = new SupplierOrderHistory(SupplierData);
            this.SuppliersView.NavigationService.Navigate(orderHistory);
        }
    }
}
