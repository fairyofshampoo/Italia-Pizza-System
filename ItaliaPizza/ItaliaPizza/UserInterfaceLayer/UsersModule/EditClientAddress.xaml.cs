using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
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

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class EditClientAddress : Page
    {
        private Address fullAddress;
        public EditClientAddress() //Añadir al constructor el id
        {
            InitializeComponent();
            ShowAddressInformation(6); //Cambiar el id por el que llega a  en el constructor
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
                //Mostrar mensaje de error
            }
        }

        private void GetAddressById(int addressId)
        {
            AddressDAO addressDAO = new AddressDAO();
            fullAddress = addressDAO.GetAddressById(addressId);
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
            List<string> postalCodes = addressDAO.GetPostalCodes();
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
            List<string> colonias = addressDAO.GetColonias(postalCode);
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
                if (addressDAO.EditAddress(address))
                {
                    //Mostrar mensaje de éxito
                }
                else
                {
                    //Mostrar mensaje de error
                }
            }
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            AddressDAO addressDAO = new AddressDAO();
            if (addressDAO.DisableAddress(fullAddress.addressId))
            {
                //Mostrar mensaje de éxito
            } else
            {
                //Mostrar mensaje de error
            }
            ShowDisableAddressButtoms();
        }

        private void BtnEnable_Click(object sender, RoutedEventArgs e)
        {
              AddressDAO addressDAO = new AddressDAO();
                if (addressDAO.EnableAddress(fullAddress.addressId))
                {
                    //Mostrar mensaje de éxito
                }
                else
                {
                    //Mostrar mensaje de error
                }
            ShowEnableAddressButtoms();
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
