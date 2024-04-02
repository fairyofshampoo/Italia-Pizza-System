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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml.Linq;

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
            lblFullName.Content = client.name;
            lblPhoneNumber.Content = client.phone;
            AddressDAO addressDAO = new AddressDAO();
            if (client.status == 0)
            {
                btnNewOrder.Visibility = Visibility.Collapsed;
                lblFullName.Foreground = Brushes.Red;
            }
            // Address fullAddress = addressDAO.GetClientAddress(client.email);
            ClientData = client;
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {
            EditClientView editClientView = new EditClientView(ClientData);
            TellerScreenView.NavigationService.Navigate(editClientView);
        }
    }
}
