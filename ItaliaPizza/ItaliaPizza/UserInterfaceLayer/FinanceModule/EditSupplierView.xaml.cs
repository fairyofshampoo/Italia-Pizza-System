using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Navigation;
using ItaliaPizza.UserInterfaceLayer.Controllers;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for EditSupplierView.xaml
    /// </summary>
    public partial class EditSupplierView : Page
    {
        private string supplierEmail;
        public ObservableCollection<SupplyAreaViewModel> SupplyAreas { get; set; }

        public SupplierController SupplierController = new SupplierController();
        public EditSupplierView()
        {
            InitializeComponent();
            SetSupplyAreaItems();
            txtPhone.PreviewTextInput += TxtPhone_PreviewTextInput;
        }

        public void SetSupplierData(string email)
        {
            supplierEmail = email;
            SupplierDAO supplierDAO = new SupplierDAO();
            Supplier supplierData = supplierDAO.GetSupplierByEmail(supplierEmail);
            int statusAccount = Constants.INACTIVE_STATUS;

            if(supplierData != null)
            {
                txtCompanyName.Text = supplierData.companyName;
                txtManagerName.Text = supplierData.manager;
                txtEmail.Text = supplierData.email;
                txtPhone.Text = supplierData.phone;
                statusAccount = supplierData.status;

                if(statusAccount == Constants.INACTIVE_STATUS)
                {
                    txtManagerName.IsEnabled = false;
                    txtCompanyName.IsEnabled = false;
                    txtEmail.IsEnabled = false;
                    txtPhone.IsEnabled = false;
                    SupplyAreaCheckboxList.IsEnabled = false;

                    btnDesactivate.IsEnabled = false;
                    btnDesactivate.Visibility = Visibility.Hidden;

                    btnActivate.IsEnabled = true;
                    btnActivate.Visibility = Visibility.Visible;

                    btnSave.IsEnabled = false;
                    btnSave.Background = Brushes.Gray;
                }
                foreach (SupplyAreaViewModel areaViewModel in SupplyAreas)
                {
                    if (supplierData.SupplyAreas.Any(area => area.area_name == areaViewModel.AreaName))
                    {
                        areaViewModel.IsSelected = true;
                    }
                }
            }
        }

        private void SetSupplyAreaItems()
        {
            SupplierAreaDAO supplierAreaDAO = new SupplierAreaDAO();

            List<SupplyArea> supplyAreas = supplierAreaDAO.GetAllSupplyAreas();

            SupplyAreas = new ObservableCollection<SupplyAreaViewModel>(
                supplyAreas.Select(area => new SupplyAreaViewModel { AreaName = area.area_name })
            );
            SupplyAreaCheckboxList.ItemsSource = SupplyAreas;
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            Supplier supplier = new Supplier
            {
                phone = txtPhone.Text,
                companyName = txtCompanyName.Text,
                manager = txtManagerName.Text,
                SupplyAreas = GetSupplyAreas()
            };

            if (ValidateSupplier(supplier))
            {
                if (SupplierController.UpdateSupplier(supplier, supplierEmail))
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    GoBack();
                }
            }
        }

        private ICollection<SupplyArea> GetSupplyAreas()
        {
            var selectedAreas = SupplyAreas.Where(area => area.IsSelected).Select(area => new SupplyArea { area_name = area.AreaName });

            return selectedAreas.ToList();
        }

        private bool ValidateSupplier(Supplier supplier)
        {
            var validations = new List<(bool IsValid, Label HintLabel)>
            {
                (SupplierController.ValidateManagerName(supplier.manager), lblManagerHint),
                (SupplierController.ValidateEmail(supplier.email), lblEmailHint),
                (SupplierController.ValidatePhone(supplier.phone), lblPhoneHint),
                (SupplierController.ValidateCompanyName(supplier.companyName), lblCompanyHint),
                (SupplierController.ValidateSupplyAreas(supplier.SupplyAreas), lblSupplyAreasHint)
            };

            bool isValid = true;

            foreach (var (isValidField, hintLabel) in validations)
            {
                if (!isValidField)
                {
                    hintLabel.Foreground = Brushes.Red;
                    isValid = false;
                }
                else
                {
                    hintLabel.Foreground = Brushes.LightGray;
                }
            }

            return isValid;
        }

        private void TxtPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!Validations.IsNumber(e.Text))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 10)
            {
                e.Handled = true;
            }

        }

        private void TxtCompanyName_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblCompanyHint.Foreground = Brushes.LightGray;
        }

        private void TxtManagerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblManagerHint.Foreground = Brushes.LightGray;
        }

        private void TxtEmail_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblEmailHint.Foreground = Brushes.LightGray;
        }

        private void TxtPhone_TextChanged(object sender, TextChangedEventArgs e)
        {
            lblPhoneHint.Foreground = Brushes.LightGray;
        }

        private void BtnActivate_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea activar al proveedor?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                if (SupplierController.UpdateSupplierStatus(supplierEmail, Constants.ACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    GoBack();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private void BtnDesactivate_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea desactivar al proveedor?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if(dialogWindow.ShowDialog() == true)
            {

                if(SupplierController.UpdateSupplierStatus(supplierEmail, Constants.INACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    GoBack();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
        }

        private void GoBack()
        {
            SuppliersView suppliersView = new SuppliersView();
            NavigationService.Navigate(suppliersView);
        }
    }
}
