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

        public bool DisableClient(string email)
        {
            bool successfulOperation = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientDisable = databaseContext.Clients
                                                   .FirstOrDefault(clientDB => clientDB.email == email);
                if(clientDisable != null)
                {
                    clientDisable.status = 0;
                    successfulOperation = true;
                }
            }
            return successfulOperation;
        }

        public bool EditDataClient(Client client)
        {
            bool successfulOperation = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientEdited = databaseContext.Clients
                                                  .FirstOrDefault(clientDB => clientDB.email == client.email);
                if(clientEdited != null)
                {
                    clientEdited.name = client.name;
                    clientEdited.phone = client.phone;
                    successfulOperation = true;
                }
            }
            return successfulOperation;
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
            List<Client> clients = new List<Client>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var clientByName = databaseContext.Clients
                                                  .Where(client => client.name.StartsWith(fullName))
                                                  .Take(5)
                                                  .ToList();
                if(clientByName != null)
                {
                    foreach(var client in clientByName)
                    {
                        clients.Add(client);
                    }
                }
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
