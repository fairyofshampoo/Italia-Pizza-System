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
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierOrderDetailView.xaml
    /// </summary>
    public partial class SupplierOrderDetailView : Page
    {
        private SupplierOrder Order;
        public SupplierOrderDetailView(SupplierOrder supplierOrder)
        {
            InitializeComponent();
            SetOrderData(supplierOrder);
        }

        public void SetOrderData(SupplierOrder supplierOrder)
        {
            Order = supplierOrder;
            SetOrderTitle(supplierOrder.orderCode);
            SetSupplierInfo(supplierOrder.Supplier);
            SetStatus(supplierOrder.status);
            SetCreationDate(supplierOrder.date);
            SetModificationDate(supplierOrder.modificationDate);
            SetTotalPayment(supplierOrder.total);
            GetSuppliesInOrder();
        }

        private void SetOrderTitle(int orderCode)
        {
            lblOrderTitle.Content = "Pedido: " + orderCode;
        }

        private void SetSupplierInfo(Supplier supplier)
        {
            txtSupplierName.Text = supplier.manager + ": " + supplier.companyName;
        }

        private void SetStatus(int status)
        {
            lblStatus.Content = GetStringStatus(status);
            if (status == Constants.INACTIVE_STATUS)
            {
                brdStatus.BorderBrush = Brushes.Red;
            }
            else if (status == Constants.COMPLETE_STATUS)
            {
                brdStatus.BorderBrush = Brushes.Orange;
            }
        }

        private string GetStringStatus(int status)
        {
            switch (status)
            {
                case Constants.ACTIVE_STATUS:
                    return "Abierto";
                case Constants.INACTIVE_STATUS:
                    return "Cancelado";
                case Constants.COMPLETE_STATUS:
                    return "Recibido";
                default:
                    return "Desconocido";
            }
        }

        private void SetCreationDate(DateTime creationDate)
        {
            txtCreationDate.Text = "Creado: " + creationDate.ToString("dd/MM/yyyy HH:mm");
        }

        private void SetModificationDate(DateTime modificationDate)
        {
            txtModificationDate.Text = "Modificado: " + modificationDate.ToString("dd/MM/yyyy HH:mm");
        }

        private void SetTotalPayment(decimal total)
        {
            txtTotalPayment.Text = "Total a pagar: $" + total.ToString();
        }


        private void GetSuppliesInOrder()
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            List<Supply> supplies = supplyOrderDAO.GetSuppliesByOrderId(Order.orderCode);

            if (supplies.Count > 0)
            {
                ShowSupplies(supplies);

            }
        }

        private void ShowSupplies(List<Supply> supplies)
        {
            suppliesListView.Items.Clear();
            SupplyInOrderUC supplyUC = new SupplyInOrderUC();
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            foreach (Supply supply in supplies)
            {
                AddSupplyToList(supply);
            }
        }

        private void AddSupplyToList(Supply supply)
        {
            SupplyInOrderUC supplyUC = new SupplyInOrderUC();
            supplyUC.SetSupplyData(supply);
            suppliesListView.Items.Add(supplyUC);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
