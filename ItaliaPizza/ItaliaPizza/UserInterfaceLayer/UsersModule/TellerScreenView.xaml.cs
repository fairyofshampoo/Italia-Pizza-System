using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
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

        public TellerScreenView()
        {
            InitializeComponent();
            ShowLastClientsRegistered();
        }

        private void ShowLastClientsRegistered()
        {
            rowsAdded = 0;
            ClientsGrid.Children.Clear();
            ClientsGrid.RowDefinitions.Clear();

            List<Client> lastClients = GetLasCients();  
            if (lastClients.Any())
            {
                foreach (Client client in lastClients)
                {
                    AddClients(client);
                }
            }
        }

        private List<Client> GetLasCients()
        {
            List<Client> lastClients = new List<Client>();
            ClientDAO clientDAO = new ClientDAO();
            lastClients = clientDAO.GetLastClientsRegistered();
            return lastClients;
        }

        private void AddClients(Client client) 
        {
            ClientsUC clientCard = new ClientsUC();
            Grid.SetRow(clientCard, rowsAdded);
            clientCard.setDataCards(client);
            ClientsGrid.Children.Add(clientCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            ClientsGrid.RowDefinitions.Add(row);
        }

        private void txtSearchBarChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;
            if (searchText.Length > 4) 
            { 
                if(IsANumber(searchText))
                {
                    //Búsqueda en los números que coincidan 
                }
                else
                {
                    //Búsqueda primero en los nombres de persona
                    //Si no buscar por dirección
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
    }
}
