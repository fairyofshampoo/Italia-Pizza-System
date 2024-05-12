using ItaliaPizza.ApplicationLayer;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierOrderUC.xaml
    /// </summary>
    public partial class SupplierOrderUC : UserControl
    {
        private SupplierOrder SupplierOrderData;
        public SupplierOrderUC()
        {
            InitializeComponent();
        }

        public void SetDataCards(SupplierOrder supplierOrder)
        {
            lblOrderTitle.Content = "Pedido: " + supplierOrder.orderCode;
            txtSupplierName.Text = supplierOrder.Supplier.manager + ": " + supplierOrder.Supplier.companyName;
            if (supplierOrder.status == Constants.INACTIVE_STATUS)
            {
                lblOrderTitle.Foreground = Brushes.Red;
            }
            SupplierData = supplier;
        }

        private string GetStringStatus()
        {

        }

        private void SupplierOrder_Click(object sender, MouseButtonEventArgs e)
        {

        }

        private void BtnReceive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
