using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Controls;
using ItaliaPizza.ApplicationLayer;
using System.Xml.Linq;
using System;
using System.Windows.Media;
using ItaliaPizza.UserInterfaceLayer.Controllers;

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class SupplierRegisterView : Page
    {
        public ObservableCollection<SupplyAreaViewModel> SupplyAreas { get; set; }
        public SupplierController SupplierController = new SupplierController();

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
            Supplier supplier = new Supplier
            {
                manager = txtManagerName.Text,
                email = txtEmail.Text,
                companyName = txtCompanyName.Text,
                SupplyAreas = GetSupplyAreas(),
                phone = txtPhone.Text

            };

            if (ValidateSupplier(supplier))
            {
                if (SupplierController.IsEmailDuplicated(txtEmail.Text))
                {
                    DialogManager.ShowWarningMessageBox("El email ingresado ya existe en el sistema, verifique que no esté duplicando al proveedor.");
                } else
                {
                    if (SupplierController.RegisterSupplier(supplier))
                    {
                        DialogManager.ShowSuccessMessageBox("Proveedor registrado exitosamente");
                        GoBack();
                    }
                }
            }
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

        private ICollection<SupplyArea> GetSupplyAreas()
        {
            var selectedAreas = SupplyAreas.Where(area => area.IsSelected).Select(area => new SupplyArea { area_name = area.AreaName });

            return selectedAreas.ToList();
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

        private void BtnGoBack_Click(object sender, System.Windows.RoutedEventArgs e)
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