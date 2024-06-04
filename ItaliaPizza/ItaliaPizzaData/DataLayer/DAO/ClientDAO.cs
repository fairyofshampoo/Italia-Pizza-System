using ItaliaPizzaData.DataLayer.DAO.Interface;
using ItaliaPizzaData.Utilities;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;

namespace ItaliaPizzaData.DataLayer.DAO
{
    public class ClientDAO : IClient
    {
        public bool AddClient(Client client)
        {
            bool successfulRegistration = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
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
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
            return successfulRegistration;
        }

        public bool ChangeStatusClient(string email, int status)
        {
            bool successfulOperation = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var clientDisable = databaseContext.Clients
                                                       .FirstOrDefault(clientDB => clientDB.email == email);
                    if (clientDisable != null)
                    {
                        clientDisable.status = (byte)status;
                        successfulOperation = true;
                        databaseContext.SaveChanges();
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return successfulOperation;
        }

        public bool EditDataClient(Client client)
        {
            bool successfulOperation = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var clientEdited = databaseContext.Clients
                                                      .FirstOrDefault(clientDB => clientDB.email == client.email);
                    if (clientEdited != null)
                    {
                        clientEdited.name = client.name;
                        clientEdited.phone = client.phone;
                        successfulOperation = true;
                        databaseContext.SaveChanges();
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return successfulOperation;
        }



        public List<Client> GetClientsByAddress(string address)
        {
            List<Client> clients = new List<Client>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var clientsByAddress = databaseContext.Addresses
                                                          .Where(fullAddress => fullAddress.street.StartsWith(address))
                                                          .Take(5)
                                                          .ToList();
                    if (clientsByAddress != null)
                    {
                        foreach (var clientAddress in clientsByAddress)
                        {
                            var client = databaseContext.Clients
                                                        .Where(clientId => clientId.email == clientAddress.clientId)
                                                        .FirstOrDefault();
                            if (client != null)
                            {
                                clients.Add(client);
                            }
                        }
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return clients;
        }

        public List<Client> GetClientsByName(string fullName)
        {
            List<Client> clients = new List<Client>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var clientsDB = databaseContext.Clients.ToList();
                    var clientByName = clientsDB
                        .Where(client => DiacriticsUtilities.RemoveDiacritics(client.name).ToUpper().Contains(DiacriticsUtilities.RemoveDiacritics(fullName).ToUpper()))
                        .Take(5).ToList();

                    if (clientByName != null)
                    {
                        foreach (var client in clientByName)
                        {
                            clients.Add(client);
                        }
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return clients;
        }

        public List<Client> GetClientsByPhone(string phone)
        {
            List<Client> clientsDB = new List<Client>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var clientsByPhone = databaseContext.Clients
                                                        .Where(client => client.phone.StartsWith(phone))
                                                        .Take(5)
                                                        .ToList();
                    if (clientsByPhone != null)
                    {
                        foreach (var client in clientsByPhone)
                        {
                            clientsDB.Add(client);
                        }
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return clientsDB;
        }

        public List<Client> GetLastClientsRegistered()
        {
            List<Client> lastClients = new List<Client>(); 
            try
            {
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
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }

            return lastClients;
        }

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = true;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var existingEmail = databaseContext.Clients.FirstOrDefault(emailexist => emailexist.email == email);
                    if (existingEmail != null)
                    {
                        isEmailExisting = false;
                    }
                }
            }
            catch (SqlException sQLException)
            {
                throw sQLException;
            }
            catch (DbUpdateException dbUpdateException)
            {
                throw dbUpdateException;
            }
            catch (EntityException entityException)
            {
                throw entityException;
            }
            catch (InvalidOperationException invalidOperationException)
            {
                throw invalidOperationException;
            }
            return isEmailExisting;
        }

        public string GetClientName(string clientEmail)
        {
            string clientName = string.Empty;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var client = databaseContext.Clients.FirstOrDefault(c => c.email == clientEmail);
                if (client != null)
                {
                    clientName = client.name;
                }
            }

            return clientName;
        }

    }
}
