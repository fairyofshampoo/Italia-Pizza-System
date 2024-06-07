using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class AddressController
    {
        public bool RegisterAddress(Address address)
        {
            AddressDAO addressDAO = new AddressDAO();   
            return addressDAO.AddNewAddress(address);
        }

        public bool ValidateStreet(String street)
        {
            bool isValid = ApplicationLayer.Validations.IsAddressValid(street);
            return isValid;
        }

        public bool EditAddressClient(Address address) 
        {
            AddressDAO addressDAO = new AddressDAO();
            return addressDAO.EditAddress(address);
        }
 
        public bool DisableAddressClient(int addressId) 
        {
            AddressDAO addressDAO = new AddressDAO();
            return addressDAO.DisableAddress(addressId);
        }

        public bool EnableAddressClient(int addressId)
        {
            AddressDAO addressDAO = new AddressDAO();
            return addressDAO.EnableAddress(addressId);
        }
    }
}
