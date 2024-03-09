using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Shapes;

namespace ItaliaPizza.DataLayer
{
    internal class ClientDAO
    {
        public bool AddClient(Client client)
        {
            bool successfulRegistration = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var newClient = new 
                {

                };
            }


            return successfulRegistration;
        }

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingEmail = databaseContext.Client.FirstOrDefault(emailexist => emailexist.email == email);
                if (existingEmail != null)
                {
                    isEmailExisting = true;
                }
            }
            return isEmailExisting;
        }

    }
}
