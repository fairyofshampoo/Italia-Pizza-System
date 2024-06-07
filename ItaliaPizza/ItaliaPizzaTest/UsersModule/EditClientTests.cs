using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class EditClientTests
    {
        private readonly ClientController _clientController = new ClientController();

        [TestMethod]
        public void EditClientTest_Success()
        {
            //Arrange
            var client = new Client
            {
                name = "Mac Miller",
                phone = "2281822391",
                email = "lalito@gmail.com"
            };

            List<string> data = new List<string>();
            data.Add("Mac Miller");
            data.Add("2281822391");

            //Act
            bool result = false;
            if (_clientController.IsDataValid(data))
            {
                result = _clientController.EditClient(client);
            }

            //Assert
            Assert.IsTrue(result, "La edición debería ser exitosa");
        }


        [TestMethod]
        public void EditClientTest_Failure() 
        {
            //Arrange
            var client = new Client
            {
                name = " %3 ",
                phone = "",
                email = "lalito@gmail.com"
            };

            List<string> data = new List<string>();
            data.Add("%3");
            data.Add("");

            //Act
            bool result = false;
            if (_clientController.IsDataValid(data))
            {
                result = _clientController.EditClient(client);
            }

            //Assert
            Assert.IsFalse(result, "La edición no debería ser exitosa");
        }

        [TestMethod]
        public void ChangeClientStatus_Success()
        {
            //Arrange
            String email = "star3oy@hotmail.com";
            int status = 0;

            //Act
            bool result = _clientController.ChangeStatusClient(email, status);

            //Assert
            Assert.IsTrue(result, "La edición del estatus del cliente debería ser exitosa");
        }
    }
}
