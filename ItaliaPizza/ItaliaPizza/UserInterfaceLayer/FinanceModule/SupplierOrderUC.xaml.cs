﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using ItaliaPizza.UserInterfaceLayer.Controllers;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierOrderUC.xaml
    /// </summary>
    public partial class SupplierOrderUC : UserControl
    {
        private SupplierOrder SupplierOrderData;
        private SupplierOrderHistory supplierOrderHistory;
        private readonly SupplierOrderController orderController = new SupplierOrderController();

        public SupplierOrderUC(SupplierOrderHistory supplierOrderHistory)
        {
            InitializeComponent();
            this.supplierOrderHistory = supplierOrderHistory;
        }

        public void SetDataCards(SupplierOrder supplierOrder)
        {
            SupplierOrderData = supplierOrder;
            SetOrderTitle(supplierOrder.orderCode);
            SetSupplierInfo(supplierOrder.Supplier);
            SetStatus(supplierOrder.status);
            SetCreationDate(supplierOrder.date);
            SetModificationDate(supplierOrder.modificationDate);
            SetTotalPayment(supplierOrder.total);
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
                btnReceive.Visibility = Visibility.Hidden;
                btnCancel.Visibility = Visibility.Hidden;
                lblOrderTitle.Foreground = Brushes.Red;
            }
            else if (status == Constants.COMPLETE_STATUS)
            {
                brdStatus.BorderBrush = Brushes.Orange;
                btnReceive.Visibility = Visibility.Hidden;
                btnCancel.Visibility = Visibility.Hidden;
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
            txtTotalPayment.Text = "$" + total.ToString();
        }

        private void SupplierOrder_Click(object sender, MouseButtonEventArgs e)
        {
            GoToEditOrderView();
        }

        private void GoToEditOrderView()
        {
            if(SupplierOrderData.status == Constants.ACTIVE_STATUS)
            {
                SupplyOrderView supplyOrderView = new SupplyOrderView();
                supplyOrderView.SetSupplyOrderData(SupplierOrderData, true);
                this.supplierOrderHistory.NavigationService.Navigate(supplyOrderView);
            } else
            {
                SupplierOrderDetailView supplierOrderDetail = new SupplierOrderDetailView(SupplierOrderData);
                this.supplierOrderHistory.NavigationService.Navigate(supplierOrderDetail);
            }
        }

        private void BtnReceive_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Ha recibido este pedido? Una vez confirmado, no es posible deshacer la acción", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);
            if (dialogWindow.ShowDialog() == true)
            {
                if (orderController.ChangeOrderToReceived(SupplierOrderData.orderCode))
                {
                    UpdateInventory();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("No se ha podido confirmar su pedido. Intente nuevamente");
                }
            }
        }

        private void UpdateInventory()
        {

            if (orderController.UpdateInventory(SupplierOrderData.orderCode))
            {
                DialogManager.ShowSuccessMessageBox("Se ha confirmado exitosamente su pedido y se ha actualizado el inventario");
                SupplierOrderData.status = Constants.COMPLETE_STATUS;
                SetStatus(Constants.COMPLETE_STATUS);
            }
            else
            {
                DialogManager.ShowErrorMessageBox("Ocurrió un error al actualizar el inventario");
            }
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Está seguro de cancelar este pedido?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);
            if(dialogWindow.ShowDialog() == true)
            {
                if (orderController.CancelOrder(SupplierOrderData.orderCode))
                {
                    SetModificationDate(DateTime.Now);
                    DialogManager.ShowSuccessMessageBox("Se ha cancelado exitosamente su pedido");
                    SupplierOrderData.status = Constants.INACTIVE_STATUS;
                    SetStatus(Constants.INACTIVE_STATUS);
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("No se ha podido cancelar su pedido. Intente nuevamente");
                }
            }
        }
    }
}
