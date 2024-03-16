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
                        status = client.status,
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
