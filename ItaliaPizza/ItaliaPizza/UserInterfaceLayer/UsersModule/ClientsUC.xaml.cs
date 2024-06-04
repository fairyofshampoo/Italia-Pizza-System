using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class ClientsUC : UserControl
    {
        private Client ClientData;

        public TellerScreenView TellerScreenView {get; set;}

        public ClientsUC()
        {
            InitializeComponent();
        }

        public void SetDataCards(Client client)
        {
            ClientData = client;
            lblFullName.Content = client.name;
            lblPhoneNumber.Content = client.phone;
            AddressDAO addressDAO = new AddressDAO();
            if (client.status == 0)
            {
                btnNewOrder.Visibility = Visibility.Collapsed;
                lblFullName.Foreground = Brushes.Red;
            }

            Address fullAddress = addressDAO.GetClientAddress(client.email);
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            EditClientView editClientView = new EditClientView(ClientData);
            TellerScreenView.NavigationService.Navigate(editClientView);
        }

        private void BtnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            RegisterOrderView registerOrderView = new RegisterOrderView(true);
            TellerScreenView.NavigationService.Navigate(registerOrderView);
            registerOrderView.SetClientData(ClientData);
        }

        private void BtnAddress_Click(object sender, RoutedEventArgs e)
        {
            AddressByClientView addressByClientView = new AddressByClientView(ClientData.email);
            TellerScreenView.NavigationService.Navigate(addressByClientView);

        }
    }
}
