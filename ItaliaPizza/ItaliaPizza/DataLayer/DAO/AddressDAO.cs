using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class AddressDAO : IAddress
    {
        public Address GetClientAddress(string email)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var address = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.clientId == email);
                return address;
            }
        }
    }
}
