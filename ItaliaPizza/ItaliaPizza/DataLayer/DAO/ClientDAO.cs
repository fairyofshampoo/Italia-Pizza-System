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
    internal class ClientDAO
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

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingEmail = databaseContext.Clients.FirstOrDefault(emailexist => emailexist.email == email);
                if (existingEmail != null)
                {
                    isEmailExisting = true;
                }
            }
            return isEmailExisting;
        }

    }
}
