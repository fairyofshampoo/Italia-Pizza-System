using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
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

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Lógica de interacción para SupplyRegister.xaml
    /// </summary>
    public partial class SupplyRegister : Page
    {
        public SupplyRegister()
        {
            InitializeComponent();
            SetComboBoxItems();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (!IsSupplyExisting())
                {
                    if (RegisterSupply())
                    {
                        DialogManager.ShowSuccessMessageBox("Insumo registrado exitosamente");
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al agregar el insumo");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("El insumo ingresado ya se encuentra registrado");
                }
            }
        }

        private bool RegisterSupply()
        {
            string name = txtName.Text;
            int amount = int.Parse(txtAmount.Text);         
            string measurementUnit = cmbMeasurementUnit.SelectedItem.ToString();
            string category = cmbCategory.SelectedItem.ToString();
            
            SupplyDAO supplyDAO = new SupplyDAO();
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();

            Supply supply = new Supply
            {
                name = name,
                amount = amount,
                status = true,
                measurementUnit = measurementUnit,
                category = supplierAreaDAO.GetSupplyAreaIdByName(category),
            };

            return supplyDAO.AddSupply(supply);
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

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
