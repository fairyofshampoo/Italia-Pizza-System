using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class RegisterClientAddressTests
    {
        AddressController _addressController = new AddressController();

        [TestMethod]
        public void RegisterClientAddress_Success()
        {
            //Arrange
            var address = new Address
            {
                street = "Guadalupe Victoria 33",
                postalCode = "91010",
                colony = "21 de marzo",
                status = 1,
                clientId = "star3oy@hotmail.com"
            };

            //Act
            bool result = false;
            string street = "Guadalupe Victoria 33";
            if (_addressController.ValidateStreet(street))
            {
                result = _addressController.RegisterAddress(address);
            }

            //Assert
            Assert.IsTrue(result, "El registro de la dirección del cliente debería ser exitoso con datos válidos");
        }

        [TestMethod]
        public void RegisterClientAddress_InvalidData_Failure()
        {
            //Arrange
            var address = new Address
            {
                street = "",
                postalCode = "91010",
                colony = "21 de marzo",
                status = 1,
                clientId = "star3oy@hotmail.com"
            };

            //Act
            bool result = false;
            string street = "";
            if (_addressController.ValidateStreet(street))
            {
                result = _addressController.RegisterAddress(address);
            }

            //Assert
            Assert.IsFalse(result, "El registro de la dirección del cliente debería ser erroneo con datos inválidos");
        }
    }
}
