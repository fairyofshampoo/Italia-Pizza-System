using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
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

        public AddressByClientView()
        {
            InitializeComponent();
            ValidateAdresses();
        }

        private void ValidateAdresses()
        {
            List<Address> addresses = GetAddressByClient();
            if (addresses.Any()) 
            {
                ShowAddresses(addresses);
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

        }


        private List<Address> GetAddressByClient()
        {
            AddressDAO addressDAO = new AddressDAO();
            List<Address> addresses = addressDAO.GetAddressByClient(clientEmail);
            return addresses;
        }

        private void BtnEnabled_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnAddNewAddress_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
