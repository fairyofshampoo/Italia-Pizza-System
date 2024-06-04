using ItaliaPizzaData.DataLayer;
using System.Windows;
using System.Windows.Controls;
namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class AddressUC : UserControl
    {

        public AddressByClientView AddressByClient {  get; set; }

        private int addressId;

        public AddressUC()
        {
            InitializeComponent();
        }

        public void EditDataCard(Address address)
        {
            lblAddress.Content = address.street;
            addressId = address.addressId;
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            EditClientAddress editClientAddress = new EditClientAddress(addressId);
            AddressByClient.NavigationService.Navigate(editClientAddress);
        }
    }
}
