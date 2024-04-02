using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using ItaliaPizza.ApplicationLayer;
using System.Xml.Linq;
using System;
using System.Windows.Media;
using ItaliaPizza.DataLayer.DAO.Interface;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class SupplierRegisterView : Page
    {
        public ObservableCollection<SupplyAreaViewModel> SupplyAreas { get; set; }

        public SupplierRegisterView()
        {
            InitializeComponent();
            SetSupplyAreaItems();
            txtPhone.PreviewTextInput += TxtPhone_PreviewTextInput;
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

        private void BtnSave_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            if (IsNewSupplierValid() && !IsEmailDuplicated())
            {
                if (RegisterSupplier())
                {
                    DialogManager.ShowSuccessMessageBox("Proveedor registrado exitosamente");
                }
            }
        }

        private bool RegisterSupplier()
        {
            SupplierDAO supplierDAO = new SupplierDAO();
            Supplier supplier = new Supplier();
            supplier.email = txtEmail.Text;
            supplier.phone = txtPhone.Text;
            supplier.companyName = txtCompanyName.Text;
            supplier.manager = txtManagerName.Text;
            supplier.status = Constants.ACTIVE_STATUS;
            supplier.SupplyAreas = GetSupplyAreas();
            return supplierDAO.AddSupplier(supplier);
        }

        private ICollection<SupplyArea> GetSupplyAreas()
        {
            var selectedAreas = SupplyAreas.Where(area => area.IsSelected).Select(area => new SupplyArea { area_name = area.AreaName });

            return selectedAreas.ToList();
        }

        private bool IsNewSupplierValid()
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
            String companyName = txtCompanyName.Text;

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
            String phone = txtPhone.Text;
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
            SupplierDAO supplierDAO = new SupplierDAO();
            bool isDuplicated = supplierDAO.IsEmailSupplierExisting(txtEmail.Text);
            if (isDuplicated)
            {
                DialogManager.ShowWarningMessageBox("El email ingresado ya existe en el sistema, verifique que no esté duplicando al proveedor.");
            }
            return isDuplicated;
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

        private void TxtPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if(!Validations.IsNumber(e.Text))
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
    }
}
