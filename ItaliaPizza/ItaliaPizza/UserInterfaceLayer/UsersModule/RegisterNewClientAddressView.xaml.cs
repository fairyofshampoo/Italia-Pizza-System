using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
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
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{

    public partial class RegisterNewClientAddressView : Page
    {
        private string emailClient;

        public RegisterNewClientAddressView(string clientEmail)
        {
            this.emailClient = clientEmail;
            InitializeComponent();
            FillPostalCodeComboBox();
        }

        private void FillPostalCodeComboBox()
        {
            List<string> postalCodes = GetPostalCodes();
            while (postalCodes.Any())
            {
                cbPostalCode.Items.Add(postalCodes.First());
                postalCodes.Remove(postalCodes.First());
            }
        }

        private List<string> GetPostalCodes()
        {
            AddressDAO addressDAO = new AddressDAO();
            List<string> postalCodes = addressDAO.GetPostalCodes();
            return postalCodes;
        }

        private void FillColoniasComboBox(string postalCode)
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            ClearErrorMessage();
            if (IsDataValid())
            {
                var newAddress = new Address
                {
                    street = txtAddress.Text,
                    postalCode = cbPostalCode.SelectedItem.ToString(),
                    colony = cbColony.SelectedItem.ToString(),
                    status = 1, 
                    clientId = this.emailClient
                };
                AddressDAO addressDAO = new AddressDAO();
                if (addressDAO.AddNewAddress(newAddress))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha registrado la dirección correctamente");
                    NavigationService.GoBack();
                } else
                {
                    ApplicationLayer.DialogManager.ShowErrorMessageBox("No se ha podido registrar la dirección, inténtelo más tarde");
                }
            }
        }

        private void CbPostalCodeSelected(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = sender as ComboBox;
            string selectedItem = comboBox.SelectedItem.ToString();
            FillColoniasComboBox(selectedItem);
        }

        private void ClearColonyComboBox()
        {
            while(cbColony.Items.Count > 0)
            {
                cbColony.Items.RemoveAt(0);
            }
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
            if(!addressValid) 
            {
                lblAddressError.Visibility = Visibility.Visible;
            }
            return addressValid;
        }

        private bool AreComboBoxSelected()
        {
            bool comboBoxSelected = true;
            if(cbColony.SelectedIndex == -1 )
            {
                comboBoxSelected = false;
                lblColonyError.Visibility = Visibility.Visible;
            }

            if(cbPostalCode.SelectedIndex == -1)
            {
                comboBoxSelected = false;
                lblPostalCodeError.Visibility = Visibility.Visible;
            }
            return comboBoxSelected;
        }

        private void ClearErrorMessage()
        {
            lblAddressError.Visibility = Visibility.Collapsed;
            lblColonyError.Visibility= Visibility.Collapsed;
            lblPostalCodeError.Visibility= Visibility.Collapsed;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
