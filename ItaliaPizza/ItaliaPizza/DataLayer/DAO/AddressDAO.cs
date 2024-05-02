using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

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

        public bool DisableAddress(int id)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var addressDisabled = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId ==  id);

                if(addressDisabled != null)
                {
                    addressDisabled.status = 0;
                    operationStatus = true;
                    databaseContext.SaveChanges();
                }
            }
                return operationStatus;
        }

        public bool EditAddress(Address newAddress)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var addressEdited = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId == newAddress.addressId);
                if (addressEdited != null)
                {
                    addressEdited.street = newAddress.street;
                    addressEdited.postalCode = newAddress.postalCode;
                    addressEdited.colony = newAddress.colony;
                    operationStatus = true;
                    databaseContext.SaveChanges();
                }
            }
            return operationStatus;
        }

        public bool EnableAddress(int id)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var addressDisabled = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId == id);

                if (addressDisabled != null)
                {
                    addressDisabled.status = 1;
                    operationStatus = true;
                    databaseContext.SaveChanges();
                }
            }
            return operationStatus;
        }

        public Address GetAddressById(int id)
        {
            Address addressFounded = null;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var address = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId == id);
                if (address != null)
                {
                    addressFounded = address;
                }
            }
                return addressFounded;
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
