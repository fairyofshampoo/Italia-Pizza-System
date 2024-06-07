using ItaliaPizzaData.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizzaData.DataLayer;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Net;

namespace ItaliaPizzaData.DataLayer.DAO
{
    public class AddressDAO : IAddress
    {
        public bool AddNewAddress(Address newAddress)
        {
            bool successfullOperation = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    databaseContext.Addresses.Add(newAddress);
                    databaseContext.SaveChanges();
                    successfullOperation = true;
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

            return successfullOperation;
        }

        public bool DisableAddress(int id)
        {
            bool operationStatus = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var addressDisabled = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId == id);

                    if (addressDisabled != null)
                    {
                        addressDisabled.status = 0;
                        operationStatus = true;
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

            return operationStatus;
        }

        public bool EditAddress(Address newAddress)
        {
            bool operationStatus = false;
            try
            {
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

            return operationStatus;
        }

        public bool EnableAddress(int id)
        {
            bool operationStatus = false;
            try
            {
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

            return operationStatus;
        }

        public List<Address> GetAddressesByClient(string emailClient)
        {
            List<Address> addresses = new List<Address>();
            try
            {
                using(var databaseContext = new ItaliaPizzaDBEntities())
                {
                    addresses = databaseContext.Addresses
                                               .Where(address => address.clientId == emailClient)
                                               .ToList();   
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

            return addresses;
        }

        public Address GetAddressById(int id)
        {
            Address addressFounded = null;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var address = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.addressId == id);
                    if (address != null)
                    {
                        addressFounded = address;
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

            return addressFounded;
        }

        public List<Address> GetAddressByStatus(int status, string emailClient)
        {
            List<Address> addresses = new List<Address>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    addresses = databaseContext.Addresses
                                               .Where(address => address.status == status && address.clientId == emailClient)
                                               .ToList();
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

            return addresses;
        }

        public Address GetClientAddress(string email)
        {
            Address addressClient = new Address();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var address = databaseContext.Addresses.FirstOrDefault(addressDB => addressDB.clientId == email);
                        if (address != null)
                        {
                            addressClient = address;
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

            return addressClient;
        }

        public List<string> GetColonias(string postalCode)
        {
            List<string> colonias = new List<string>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var coloniasDB = databaseContext.ColonyCatalogs
                                                    .Where(catalog => catalog.code == postalCode)
                                                    .Select(catalog => catalog.settlement)
                                                    .ToList();
                    if (coloniasDB != null)
                    {
                        foreach (var catalog in coloniasDB)
                        {
                            colonias.Add(catalog);
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

            return colonias;
        }

        public List<string> GetPostalCodes()
        {
            List<string> postalCodes = new List<string>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var postalCodesDb = databaseContext.ColonyCatalogs
                                                       .Where(catalog => catalog.municipality == "Xalapa")
                                                       .Select(catalog => catalog.code)
                                                       .Distinct()
                                                       .ToList();
                    if (postalCodesDb != null)
                    {
                        foreach (var catalog in postalCodesDb)
                        {
                            postalCodes.Add(catalog);
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

            return postalCodes;
        }
    }
}
