﻿using ItaliaPizza.ApplicationLayer;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Lógica de interacción para SupplyEditView.xaml
    /// </summary>
    public partial class SupplyEditView : Page
    {
        public SupplyEditView()
        {
            InitializeComponent();
            SetComboBoxItems();
        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea eliminar el insumo?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                string name = txtName.Text;
                SupplyDAO supplyDAO = new SupplyDAO();

                if (supplyDAO.ChangeStatus(name, Constants.INACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Insumo actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el insumo");
                }
            }
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea activar el insumo?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                string name = txtName.Text;
                SupplyDAO supplyDAO = new SupplyDAO();

                if (supplyDAO.ChangeStatus(name, Constants.ACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Insumo actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el insumo");
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (!IsSupplyExisting())
                {
                    if (EditSupply()) 
                    {
                        DialogManager.ShowSuccessMessageBox("Insumo actualizado exitosamente");
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el insumo");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("El insumo ingresado ya se encuentra registrado");
                }
            }
        }

        private bool EditSupply()
        {
            string name = txtName.Text;
            decimal amount = Decimal.Parse(txtAmount.Text);
            string categoty = cmbCategory.SelectedItem.ToString();
            string measurementUnit = cmbMeasurementUnit.SelectedItem.ToString();

            SupplyDAO supplyDAO = new SupplyDAO();
            Supply supply = new Supply
            {
                name = name,
                //amount = amount,
                category = categoty,
            };

            return supplyDAO.AddSupply(supply); //Método de editar
        }

        public void SetModifySupply(Supply supplyInfo)
        {
            if (supplyInfo != null)
            {
                txtName.Text = supplyInfo.name;
                txtAmount.Text = supplyInfo.amount.ToString();

                if (!string.IsNullOrEmpty(supplyInfo.measurementUnit) && cmbMeasurementUnit.Items.Contains(supplyInfo.measurementUnit))
                {
                    cmbMeasurementUnit.SelectedItem = supplyInfo.measurementUnit;
                }

                if (!string.IsNullOrEmpty(supplyInfo.category) && cmbCategory.Items.Contains(supplyInfo.category))
                {
                    cmbCategory.SelectedItem = supplyInfo.category;
                }

             /*   if (supplyInfo.status == Constants.ACTIVE_STATUS)
                {
                    txtName.IsEnabled = false;
                    txtAmount.IsEnabled = false;
                    cmbCategory.IsEnabled = false;
                    cmbMeasurementUnit.IsEnabled = false;

                    btnDesactive.IsEnabled = false;
                    btnDesactive.Visibility = Visibility.Hidden;

                    btnActive.IsEnabled = true;
                    btnActive.Visibility = Visibility.Visible;

                    btnSave.IsEnabled = false;
                    btnSave.Background = Brushes.Gray;
                }            */                 
            }
        }

        private bool IsSupplyExisting()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            string name = txtName.Text;
            bool isSupplyAlreadyExisting = supplyDAO.IsSupplyNameExisting(name);
            return isSupplyAlreadyExisting;
        }

        private void SetComboBoxItems()
        {
            cmbMeasurementUnit.ItemsSource = new string[]
            {
                "Kilogramo", "Gramo", "Pie", "Litro", "Mililitro", "Onza"
            };

            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();
            List<SupplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();

            List<string> areaNames = supplyAreas.Select(area => area.area_name).ToList();

            cmbCategory.ItemsSource = areaNames;
        }

        private bool ValidateFields()
        {
            bool validateFields = true;
            decimal amount = 0;

            if (txtName.Text.Equals(string.Empty) || !Validations.IsSupplyNameValid(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                txtName.BorderThickness = new Thickness(2);
                lblNameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtAmount.Text.Equals(string.Empty) || !Decimal.TryParse(txtAmount.Text, out amount) || amount < 0)
            {
                txtAmount.BorderBrush = Brushes.Red;
                txtAmount.BorderThickness = new Thickness(2);
                lblAmountError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbCategory.SelectedItem == null)
            {
                cmbCategory.BorderBrush = Brushes.Red;
                cmbCategory.BorderThickness = new Thickness(2);
                lblCategoryError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbMeasurementUnit.SelectedItem == null)
            {
                cmbMeasurementUnit.BorderBrush = Brushes.Red;
                cmbMeasurementUnit.BorderThickness = new Thickness(2);
                lblMeasurementUnitError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            return validateFields;
        }

        private void ResetFields()
        {
            txtName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtName.BorderThickness = new Thickness(0);
            lblNameError.Visibility = Visibility.Collapsed;

            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            lblAmountError.Visibility = Visibility.Collapsed;

            cmbCategory.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbCategory.BorderThickness = new Thickness(0);
            lblCategoryError.Visibility = Visibility.Collapsed;

            cmbMeasurementUnit.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbMeasurementUnit.BorderThickness = new Thickness(0);
            lblMeasurementUnitError.Visibility = Visibility.Collapsed;
        }
    }
}