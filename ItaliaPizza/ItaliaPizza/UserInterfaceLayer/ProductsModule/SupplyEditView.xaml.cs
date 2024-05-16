using ItaliaPizza.ApplicationLayer;
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
using System.Collections.ObjectModel;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;

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
        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {
            SupplyDAO supplyDAO = new SupplyDAO();

            if (!supplyDAO.ExistsSupplyInRecipe(txtName.Text))
            {
                DialogWindow dialogWindow = new DialogWindow();
                dialogWindow.SetDialogWindowData("Confirmación", "¿Desea eliminar el insumo?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

                if (dialogWindow.ShowDialog() == true)
                {
                    string name = txtName.Text;

                    if (supplyDAO.ChangeSupplyStatus(name, Constants.INACTIVE_STATUS))
                    {
                        DialogManager.ShowSuccessMessageBox("Insumo desactivado exitosamente");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el insumo");
                    }
                }
            }
            else
            {
                DialogManager.ShowErrorMessageBox("Este insumo se encuentra registrado en al menos una receta activa");
            }           
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea activar el insumo?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                string name = txtName.Text;
                SupplyDAO supplyDAO = new SupplyDAO();

                if (supplyDAO.ChangeSupplyStatus(name, Constants.ACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Insumo activado exitosamente");
                    NavigationService.GoBack();
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
                if (EditSupply())
                {
                    DialogManager.ShowSuccessMessageBox("Insumo actualizado exitosamente");
                    NavigationService.GoBack();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el insumo");
                }
            }
        }

        private bool EditSupply()
        {
            string name = txtName.Text;
            decimal amount = decimal.Parse(txtAmount.Text);
            string measurementUnit = cmbMeasurementUnit.SelectedItem.ToString();
            string category = cmbCategory.SelectedItem.ToString();

            SupplyDAO supplyDAO = new SupplyDAO();
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();

            Supply supply = new Supply
            {
                name = name,
                amount = amount,
                measurementUnit = measurementUnit,
                category = supplierAreaDAO.GetSupplyAreaIdByName(category),
            };

            return supplyDAO.ModifySupply(supply, name);
        }

        public void SetModifySupply(Supply supplyInfo)
        {
            if (supplyInfo != null)
            {
                SetComboBoxItems();
                txtName.Text = supplyInfo.name;
                txtAmount.Text = supplyInfo.amount.ToString();

                if (!string.IsNullOrEmpty(supplyInfo.measurementUnit) && cmbMeasurementUnit.Items.Contains(supplyInfo.measurementUnit))
                {
                    cmbMeasurementUnit.SelectedItem = supplyInfo.measurementUnit;
                }

                if (!string.IsNullOrEmpty(supplyInfo.SupplyArea.area_name) && cmbCategory.Items.Contains(supplyInfo.SupplyArea.area_name))
                {
                    cmbCategory.SelectedItem = supplyInfo.SupplyArea.area_name;
                }

                if (supplyInfo.status == false)
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
                }

            }
        }

        private void SetComboBoxItems()
        {
            cmbMeasurementUnit.ItemsSource = new string[]
            {
                "Kilogramo", "Gramo", "Pie", "Litro", "Mililitro", "Onza", "Unidad"
            };

            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();
            List<SupplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();

            List<string> areaNames = supplyAreas.Select(area => area.area_name).ToList();

            cmbCategory.ItemsSource = areaNames;
        }

        private bool ValidateFields()
        {
            bool validateFields = true;
           
            if (cmbCategory.SelectedItem == null)
            {
                cmbCategory.BorderBrush = Brushes.Red;
                cmbCategory.BorderThickness = new Thickness(2);
                lblCategoryError.Visibility = Visibility.Visible;
                validateFields = false;
            }
           
            return validateFields;
        }

        private void ResetFields()
        {           
            cmbCategory.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbCategory.BorderThickness = new Thickness(0);
            lblCategoryError.Visibility = Visibility.Collapsed;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
