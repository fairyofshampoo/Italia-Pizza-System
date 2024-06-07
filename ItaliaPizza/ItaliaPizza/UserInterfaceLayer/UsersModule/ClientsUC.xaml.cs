using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;

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

            string street = " ";
            try
            {
                Address fullAddress = addressDAO.GetClientAddress(client.email);
                if (fullAddress != null) 
                {
                    street = fullAddress.street;
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

            lblAddress.Content = street;
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
