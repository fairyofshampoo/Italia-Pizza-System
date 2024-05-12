using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for EditSupplierView.xaml
    /// </summary>
    public partial class EditSupplierView : Page
    {
        private string supplierEmail;
        public ObservableCollection<SupplyAreaViewModel> SupplyAreas { get; set; }
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
            if (IsSupplierValid() && !IsEmailDuplicated())
            {
                if (ModifySupplier())
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    NavigationService.GoBack();
                }
            }
        }

        private bool ModifySupplier()
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            Supplier supplier = new Supplier();

            supplier.email = txtEmail.Text;
            supplier.phone = txtPhone.Text;
            supplier.companyName = txtCompanyName.Text;
            supplier.manager = txtManagerName.Text;
            supplier.SupplyAreas = GetSupplyAreas();

            return supplierDAO.ModifySupplier(supplier, supplierEmail);
        }

        private ICollection<SupplyArea> GetSupplyAreas()
        {
            var selectedAreas = SupplyAreas.Where(area => area.IsSelected).Select(area => new SupplyArea { area_name = area.AreaName });

            return selectedAreas.ToList();
        }

        private bool IsSupplierValid()
        {
            bool managerDataValid = ValidateManagerName();
            bool emailDataValid = ValidateEmail();
            bool phoneDataValid = ValidatePhone();
            bool companyDataValid = ValidateCompanyName();
            bool supplyAreasValid = ValidateSupplyAreas();

            return managerDataValid && emailDataValid && phoneDataValid && companyDataValid && supplyAreasValid;
        }

        private bool ValidateSupplyAreas()
        {
            lblSupplyAreasHint.Foreground = Brushes.LightGray;
            bool isValid = true;
            if (GetSupplyAreas().Count == 0)
            {
                lblSupplyAreasHint.Foreground = Brushes.Red;
                isValid = false;
            }
            return isValid;
        }

        private bool ValidateCompanyName()
        {
            bool isValid = true;
            string companyName = txtCompanyName.Text;

            if (!Validations.IsCompanyNameValid(companyName))
            {
                lblCompanyHint.Foreground = Brushes.Red;
                isValid = false;
            }
            return isValid;
        }

        public bool ValidatePhone()
        {
            bool isValid = true;
            string phone = txtPhone.Text;
            if (!Validations.IsPhoneValid(phone))
            {
                lblPhoneHint.Foreground = Brushes.Red;
                isValid = false;
            }
            return isValid;
        }

        public bool ValidateEmail()
        {
            bool isValid = true;
            string email = txtEmail.Text;

            if (!Validations.IsEmailValid(email))
            {
                lblEmailHint.Foreground = Brushes.Red;
                isValid = false;
            }
            return isValid;
        }

        private bool ValidateManagerName()
        {
            string manager = txtManagerName.Text;
            bool isValid = true;

            if (!Validations.IsNameValid(manager))
            {
                lblManagerHint.Foreground = Brushes.Red;
                isValid = false;
            }

            return isValid;
        }

        private bool IsEmailDuplicated()
        {
            bool isDuplicated = false;
            string email = txtEmail.Text;
            if(email == supplierEmail)
            {
                SupplierDAO supplierDAO = new SupplierDAO();
                isDuplicated = supplierDAO.IsEmailSupplierExisting(txtEmail.Text);
                if (isDuplicated)
                {
                    DialogManager.ShowWarningMessageBox("El email ingresado ya existe en el sistema, verifique que no esté duplicando al proveedor.");
                }
            }
            
            return isDuplicated;
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
                SupplierDAO supplierDAO = new SupplierDAO();

                if (supplierDAO.ChangeSupplierStatus(supplierEmail, Constants.ACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    NavigationService.GoBack();
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
                SupplierDAO supplierDAO = new SupplierDAO();

                if(supplierDAO.ChangeSupplierStatus(supplierEmail, Constants.INACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor actualizado exitosamente");
                    NavigationService.GoBack();
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
