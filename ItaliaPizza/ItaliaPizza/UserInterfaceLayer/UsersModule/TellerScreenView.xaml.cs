using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class TellerScreenView : Page
    {

        private int rowsAdded = 0;
        private int searchFlag = 0;

        public TellerScreenView()
        {
            InitializeComponent();

            menuFrame.Content = new TellerMenu(this);
            SetClientsInPage();
        }

        private void SetClientsInPage()
        {
            List<Client> clients = GetCients();
            if (clients.Any())
            {
                ShowClients(clients);
                txtSearchBar.IsReadOnly = false;
            }
            else
            {
                txtSearchBar.IsReadOnly = true;
                ShowNoClientsMessage();
            }
        }

        private void ShowNoClientsMessage()
        {
            ClientsGrid.Children.Clear();
            ClientsGrid.RowDefinitions.Clear();
            Label lblNoProducts = new Label();
            lblNoProducts.Style = (Style)FindResource("NoClientsLabelStyle");
            lblNoProducts.HorizontalAlignment = HorizontalAlignment.Center;
            lblNoProducts.VerticalAlignment = VerticalAlignment.Center;
            Grid.SetRow(lblNoProducts, rowsAdded);
            ClientsGrid.Children.Add(lblNoProducts);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            ClientsGrid.RowDefinitions.Add(row);
        }

        private void ShowClients(List<Client> clients)
        {
            rowsAdded = 0;
            ClientsGrid.Children.Clear();
            ClientsGrid.RowDefinitions.Clear();

            if (clients.Any())
            {
                lblClientNotFound.Visibility = Visibility.Collapsed;
                foreach (Client client in clients)
                {
                    AddClients(client);
                }
            } else
            {
                lblClientNotFound.Visibility = Visibility.Visible;
            }
        }

        private List<Client> GetCients()
        {
            List<Client> lastClients = new List<Client>();
            ClientDAO clientDAO = new ClientDAO();
            lastClients = clientDAO.GetLastClientsRegistered();
            return lastClients;
        }

        private void AddClients(Client client) 
        {
            ClientsUC clientCard = new ClientsUC();
            clientCard.TellerScreenView = this;
            Grid.SetRow(clientCard, rowsAdded);
            clientCard.SetDataCards(client);
            ClientsGrid.Children.Add(clientCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            ClientsGrid.RowDefinitions.Add(row);
        }

        private void txtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;
            if (searchText.Length > 3) 
            { 
                if(IsANumber(searchText))
                {
                    SearchByPhoneNumber(searchText);
                }
                else
                {
                    if(searchFlag == 0)
                    {
                        SearchByAddress(searchText);
                    } else
                    {
                        SearchByName(searchText);
                    }
                }
            }
        }

        private bool IsANumber(string number)
        {
            bool isANumber = false;
            Regex regex = new Regex(@"^[0-9]+$");
            if (regex.IsMatch(number))
            {
                isANumber = true;
            }
            return isANumber;
        }

        private void SearchByPhoneNumber(string phoneNumber)
        {
            ClientDAO clientDAO = new ClientDAO();
            List<Client> clients = clientDAO.GetClientsByPhone(phoneNumber);
            ShowClients(clients);
        }

        private void SearchByAddress(string address)
        {
            ClientDAO clientDAO = new ClientDAO();
            List<Client> clients = clientDAO.GetClientsByAddress(address);
            if (!clients.Any())
            {
                searchFlag = 1;
            }
            ShowClients(clients);
        }

        private void SearchByName(string name)
        {
            ClientDAO clientDAO = new ClientDAO();
            List<Client> clients = clientDAO.GetClientsByName(name);
            if(!clients.Any())
            {
                searchFlag = 0;
            }
            ShowClients(clients);
        }

        private void BtnAddClient_Click(object sender, RoutedEventArgs e)
        {
            RegisterClientView registerClientView = new RegisterClientView();
            NavigationService.Navigate(registerClientView);
        }
    }
}
