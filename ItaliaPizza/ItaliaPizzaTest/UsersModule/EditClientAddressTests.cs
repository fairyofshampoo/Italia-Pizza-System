using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class EditClientAddressTests
    {
        AddressController _addressController = new AddressController();

        [TestMethod]
        public void EditClientAddress_Success()
        {
            //Arrange
            var address = new Address
            {
                addressId = 3,
                street = "Calle Cuautla Morelos",
                postalCode = "91100",
                colony = "Revolución",
                status = 1,
                clientId = "star3oy@hotmail.com"
            };

            //Act
            bool result = false;
            string street = "Calle Cuautla Morelos";
            if (_addressController.ValidateStreet(street))
            {
                result = _addressController.EditAddressClient(address);
            }

            //Assert
            Assert.IsTrue(result, "La edición de la dirección debería ser exitosa");
        }

        [TestMethod]
        public void EditAddressClient_InvalidData_Failure()
        {
            //Arrange
            var address = new Address
            {
                street = "",
                postalCode = "91100",
                colony = "Revolución",
                status = 1,
                clientId = "star3oy@hotmail.com"
            };

            //Act
            bool result = false;
            string street = " ";
            if (_addressController.ValidateStreet(street))
            {
                result = _addressController.EditAddressClient(address);
            }

            //Assert
            Assert.IsFalse(result, "La edición de la dirección debería ser falllido");
        }

        [TestMethod]
        public void DisableAddressClient_Success()
        {
            //Arrange
            int addressId = 1;

            //Act
            bool result = _addressController.DisableAddressClient(addressId);

            //Assert
            Assert.IsTrue(result, "La desactivación de la dirección del cliente debe ser exitosa");
        }

        [TestMethod]
        public void EnableAddressClient_Success()
        {
            //Arrange
            int addressId = 1;

            //Act
            bool result = _addressController.EnableAddressClient(addressId);    

            //Assert
            Assert.IsTrue(result, "La activación de la dirección del cliente debe ser exitosa");
        }

    }
}
