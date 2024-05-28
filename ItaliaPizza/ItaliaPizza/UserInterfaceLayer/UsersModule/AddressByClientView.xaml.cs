using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
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

    public partial class AddressByClientView : Page
    {
        public string clientEmail { get; set; }

        public AddressByClientView(string clientEmail)
        {
            this.clientEmail = clientEmail;
            InitializeComponent();
            List<Address> addresses = GetAddressByClient();
            ValidateAdresses(addresses);
        }

        private void ValidateAdresses(List<Address> addressList)
        {
            if (addressList.Any()) 
            {
                lblAddresssNotFound.Visibility = Visibility.Collapsed;
                ShowAddresses(addressList);
            }
            else
            {
                ShowErrorLabel();
            }
            
        }

        private void ShowAddresses(List<Address> addresses) 
        {
            foreach(var address in addresses)
            {
                AddCards(address);
            }
        }

        private void AddCards(Address address) 
        {
            AddressUC addressCard = new AddressUC();
            addressCard.addressByClient = this;
            addressCard.EditDataCard(address);
            addressListView.Items.Add(addressCard);
        }


        private void ShowErrorLabel()
        {
            lblAddresssNotFound.Visibility = Visibility.Visible;
        }


        private List<Address> GetAddressByClient()
        {
            AddressDAO addressDAO = new AddressDAO();
            List<Address> addresses = addressDAO.GetAddressesByClient(clientEmail);
            return addresses;
        }

        private void BtnEnabled_Click(object sender, RoutedEventArgs e)
        {
            int status = 1;
            GetAddressByStatus(status);
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            int status = 0;
            GetAddressByStatus(status);
        }

        private void GetAddressByStatus(int status)
        {
            AddressDAO addressDAO = new AddressDAO();
            List<Address> addresses = addressDAO.GetAddressByStatus(status, clientEmail);
            ValidateAdresses(addresses);
        }

        private void BtnAddNewAddress_Click(object sender, RoutedEventArgs e)
        {
            RegisterNewClientAddressView registerNewClientAddressView = new RegisterNewClientAddressView(clientEmail);
            NavigationService.Navigate(registerNewClientAddressView);
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
