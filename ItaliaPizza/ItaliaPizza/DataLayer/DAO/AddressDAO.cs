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
        public bool AddNewAddress(Address newAddress)
        {
            bool successfullOperation = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                databaseContext.Addresses.Add(newAddress);
                databaseContext.SaveChanges();
                successfullOperation = true;                                                
            }
            return successfullOperation;
        }

        public Address GetClientAddress(string email)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var address = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.clientId == email);
                return address;
            }
        }

        public List<string> GetColonias(string postalCode)
        {
            List<string> colonias = new List<string>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var coloniasDB = databaseContext.ColonyCatalogs
                                                .Where(catalog => catalog.code ==  postalCode)
                                                .Select(catalog => catalog.settlement)
                                                .ToList();
                if(coloniasDB != null)
                {
                    foreach(var catalog in coloniasDB)
                    {
                        colonias.Add(catalog);
                    }
                }
            }
            return colonias;
        }

        public List<string> GetPostalCodes()
        {
            List<string> postalCodes = new List<string>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var postalCodesDb = databaseContext.ColonyCatalogs
                                                   .Where(catalog => catalog.municipality == "Xalapa")
                                                   .Select(catalog => catalog.code)
                                                   .Distinct()
                                                   .ToList();
                if(postalCodesDb != null)
                {
                    foreach(var catalog in postalCodesDb)
                    {
                        postalCodes.Add(catalog);
                    }
                }
            }
            return postalCodes;
        }
    }
}
