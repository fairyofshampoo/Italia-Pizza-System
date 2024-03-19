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

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class ClientsUC : UserControl
    {
        private string userEmail;

        public ClientsUC()
        {
            InitializeComponent();
        }


        public void setDataCards(Client client)
        {
            lblFullName.Content = client.name;
            lblPhoneNumber.Content = client.phone;
            AddressDAO addressDAO = new AddressDAO();
            // Address fullAddress = addressDAO.GetClientAddress(client.email);
            userEmail = client.email;
        }

        private void BtnEditClient_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
