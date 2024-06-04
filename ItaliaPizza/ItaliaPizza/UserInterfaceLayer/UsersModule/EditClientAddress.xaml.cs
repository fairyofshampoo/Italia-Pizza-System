using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class EditClientAddress : Page
    {
        private Address fullAddress = new Address();
        public EditClientAddress(int idAddress) 
        {
            InitializeComponent();
            ShowAddressInformation(idAddress); 
            ShowButtoms();
        }

        private void ShowButtoms()
        {
            if (fullAddress.status == 1)
            {
                ShowEnableAddressButtoms();
            } 
            else
            {
                ShowDisableAddressButtoms();
            }
        }

        private void ShowAddressInformation(int adressId)
        { 
            GetAddressById(adressId);
            if(fullAddress != null)
            {
                ShowStreetAndNumber(fullAddress.street);
                FillPostalCodeComboBox(fullAddress.postalCode);
                FillColoniasComboBox(fullAddress.postalCode, fullAddress.colony);
            }
            else
            {
                ApplicationLayer.DialogManager.ShowErrorMessageBox("No se ha podido recuperar la dirección");
            }
        }

        private void GetAddressById(int addressId)
        {
            AddressDAO addressDAO = new AddressDAO();
            try
            {
                fullAddress = addressDAO.GetAddressById(addressId);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private void ShowStreetAndNumber(string streetAndNumber)
        {
            txtAddress.Text = streetAndNumber;
        }

        private void FillPostalCodeComboBox(string postalCode)
        {
            List<string> postalCodes = GetPostalCodes();
            while (postalCodes.Any())
            {
                cbPostalCode.Items.Add(postalCodes.First());
                postalCodes.Remove(postalCodes.First());
            }
            cbPostalCode.SelectedItem = postalCode;
        }

        private List<string> GetPostalCodes()
        {
            AddressDAO addressDAO = new AddressDAO();
            List<string> postalCodes = new List<string>();
            try
            {
                postalCodes = addressDAO.GetPostalCodes();
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }
            return postalCodes;
        }

        private void FillColoniasComboBox(string postalCode, string colonia)
        {
            ClearColonyComboBox();
            List<string> colonias = GetColoniasByPostalCode(postalCode);
            while (colonias.Any())
            {
                cbColony.Items.Add(colonias.First());
                colonias.Remove(colonias.First());
            }
            cbColony.SelectedItem = colonia;
        }

        private void FillColoniasComboBoxByPostalCode(string postalCode)
        {
            ClearColonyComboBox();
            List<string> colonias = GetColoniasByPostalCode(postalCode);
            while (colonias.Any())
            {
                cbColony.Items.Add(colonias.First());
                colonias.Remove(colonias.First());
            }
        }

        private List<string> GetColoniasByPostalCode(string postalCode)
        {
            AddressDAO addressDAO = new AddressDAO();
            List<string> colonias = new List<string>();
            try
            {
                colonias = addressDAO.GetColonias(postalCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


            return colonias;
        }

        private void ClearColonyComboBox()
        {
            while (cbColony.Items.Count > 0)
            {
                cbColony.Items.RemoveAt(0);
            }
        }

        private void BtnEdit_Click(object sender, RoutedEventArgs e)
        {
            ClearErrorMessage();
            if (IsDataValid())
            {
                AddressDAO addressDAO = new AddressDAO();
                Address address = new Address();
                address.addressId = fullAddress.addressId;
                address.street = txtAddress.Text;
                address.colony = cbColony.SelectedItem.ToString();
                address.postalCode = cbPostalCode.SelectedItem.ToString();
                try
                {
                    if (addressDAO.EditAddress(address))
                    {
                        ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha editado la dirección del cliente");
                    }
                    else
                    {
                        ApplicationLayer.DialogManager.ShowErrorMessageBox("Se ha ocurrido un error al editar la dirección");
                    }
                }
                catch (SqlException)
                {
                    ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
                }
                catch (DbUpdateException)
                {
                    ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
                }
                catch (EntityException)
                {
                    ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
                }
                catch (InvalidOperationException)
                {
                    ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
                }

            }
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            AddressDAO addressDAO = new AddressDAO();
            try
            {
                if (addressDAO.DisableAddress(fullAddress.addressId))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha desactivado la dirección");
                }
                else
                {
                    ApplicationLayer.DialogManager.ShowErrorMessageBox("Ha ocurrido un problema al desactivar la dirección");
                }
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

            ShowDisableAddressButtoms();
        }

        private void BtnEnable_Click(object sender, RoutedEventArgs e)
        {
              AddressDAO addressDAO = new AddressDAO();
            try
            {
                if (addressDAO.EnableAddress(fullAddress.addressId))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha activado la diección");
                }
                else
                {
                    ApplicationLayer.DialogManager.ShowErrorMessageBox("Ha ocurrido un error al desactivar la dirección");
                }
                ShowEnableAddressButtoms();
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private void ShowDisableAddressButtoms()
        {
            btnEnable.Visibility = Visibility.Visible;
            btnDisable.Visibility = Visibility.Collapsed;
            btnEditAddress.Visibility = Visibility.Collapsed;
        }

        private void ShowEnableAddressButtoms()
        {
            btnDisable.Visibility = Visibility.Visible;
            btnEditAddress.Visibility = Visibility.Visible;
            btnEnable.Visibility = Visibility.Collapsed;
        }

        private void CbPostalCodeSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selectedItem = comboBox.SelectedItem.ToString();
            FillColoniasComboBoxByPostalCode(selectedItem);
        }

        private bool IsDataValid()
        {
            bool addressValid = IsAddressDataValid();
            bool comboBoxesValidation = AreComboBoxSelected();
            return addressValid && comboBoxesValidation;
        }

        private bool IsAddressDataValid()
        {
            bool addressValid = ApplicationLayer.Validations.IsAddressValid(txtAddress.Text);
            if (!addressValid)
            {
                lblAddressError.Visibility = Visibility.Visible;
            }
            return addressValid;
        }

        private bool AreComboBoxSelected()
        {
            bool comboBoxSelected = true;
            if (cbColony.SelectedIndex == -1)
            {
                comboBoxSelected = false;
                lblColonyError.Visibility = Visibility.Visible;
            }

            if (cbPostalCode.SelectedIndex == -1)
            {
                comboBoxSelected = false;
                lblPostalCodeError.Visibility = Visibility.Visible;
            }
            return comboBoxSelected;
        }

        private void ClearErrorMessage()
        {
            lblAddressError.Visibility = Visibility.Collapsed;
            lblColonyError.Visibility = Visibility.Collapsed;
            lblPostalCodeError.Visibility = Visibility.Collapsed;
        }

    }
}
