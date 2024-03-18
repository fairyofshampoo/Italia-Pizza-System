using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class ClientDAO : IClient
    {
        public bool AddClient(Client client)
        {
            bool successfulRegistration = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newClient = new Client
                    {
                        name = client.name,
                        secondLastName = client.secondLastName,
                        firstLastName = client.firstLastName,
                        email = client.email,
                        phone = client.phone,
                        status = 1
                    };
                    databaseContext.Clients.Add(newClient);
                    databaseContext.SaveChanges();
                    successfulRegistration = true;
                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
                
            }
            return successfulRegistration;
        }

        public List<Client> GetClientsByAddress(string address)
        {
            List<Client> clients = new List<Client>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientsByAddress = databaseContext.Addresses
                                                      .Where(fullAddress => fullAddress.street.StartsWith(address))
                                                      .Take(5)
                                                      .ToList();
            }
            return clients;
        }

        public List<Client> GetClientsByName(string fullName)
        {
            String[]  nameDivided= fullName.Split(' ');
            int numberOfWords = nameDivided.Length;
            List<Client> clients = new List<Client>();

            switch(numberOfWords)
            {
                case 1: clients = SearchClietJustName(fullName);
                        break;

                case 2: clients = SearchClientByNameAndFirstLastName(nameDivided);
                        break;

                case 3: clients = SearchClientByFullName(nameDivided);
                        break;
            }

            
            return clients;
        }

        private List<Client> SearchClietJustName(string fullName)
        {
            List<Client> clients = new List<Client>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientsJustName = databaseContext.Clients
                                                     .Where(client => client.name.StartsWith(fullName))
                                                     .Take(5)
                                                     .ToList();
            }
            return clients;
        }

        private List<Client> SearchClientByNameAndFirstLastName (String[] nameDivided)
        {
            List<Client> clients = new List<Client> ();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientByNameAndFirstLastName = databaseContext.Clients
                                                                  .Where(client => client.name.StartsWith(nameDivided[0]) 
                                                                         && client.firstLastName.StartsWith(nameDivided[1]))
                                                                  .Take(5)
                                                                  .ToList();
            }
            return clients;
        }

        private List<Client> SearchClientByFullName(String[] nameDivided)
        {
            List<Client> clients = new List<Client>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientByNameAndFirstLastName = databaseContext.Clients
                                                                  .Where(client => client.name.StartsWith(nameDivided[0])
                                                                         && client.firstLastName.StartsWith(nameDivided[1])
                                                                         && client.secondLastName.StartsWith(nameDivided[2]))
                                                                  .Take(5)
                                                                  .ToList();
            }
            return clients;
        }

        public List<Client> GetClientsByPhone(string phone)
        {
            List<Client> clientsDB = new List<Client>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientsByPhone = databaseContext.Clients
                                                    .Where(client => client.phone.StartsWith(phone))
                                                    .Take(5)
                                                    .ToList();
                if(clientsByPhone != null)
                {
                    foreach (var client in clientsByPhone)
                    {
                        clientsDB.Add(client);
                    }
                }
            }
            return clientsDB;
        }

        public List<Client> GetLastClientsRegistered()
        {
            List<Client> lastClients = new List<Client>(); 
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var lastClientsDB = databaseContext.Clients
                                                   .OrderByDescending(client => client.name)
                                                   .Take(10)
                                                   .ToList();
                if (lastClientsDB != null)
                {
                    foreach (var client in lastClientsDB)
                    {
                        lastClients.Add(client);
                    }
                }
            }
            return lastClients;
        }

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = true;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingEmail = databaseContext.Clients.FirstOrDefault(emailexist => emailexist.email == email);
                if (existingEmail != null)
                {
                    isEmailExisting = false;
                    ApplicationLayer.DialogManager.ShowErrorMessageBox("El correo electrónico ya ha sido registrado");
                }
            }
            return isEmailExisting;
        }

    }
}
