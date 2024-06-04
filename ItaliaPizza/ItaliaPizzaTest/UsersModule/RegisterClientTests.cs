using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class RegisterClientTests
    {
        private readonly ClientController _clientController = new ClientController();

        [TestMethod]
        public void AddNewClientt()
        {
            //Arrange
            var client = new Client
            {
               email = "star3oy@hotmail.com", 
               name = "Eduardo Carrera Colorado", 
               phone = "2731348124", 
               status = 1
            };

            //Act 
            bool result = false;
            List<String> list = new List<String>();
            list.Add("Eduardo Carrera Colorado");
            list.Add("2731348124");
            list.Add("star3oy@hotmail.com");   
            if(_clientController.IsDataValid(list) && _clientController.IsEmailExisting("star3oy@hotmail.com")){
                if (_clientController.RegisterClient(client))
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsTrue(result, "El registro del cliente debería ser exitoso con datos válidos");
        }

        [TestMethod]
        public void AddNewClient_InvalidData_Failure()
        {
            //Arrange
            var client = new Client
            {
                email = "",
                name = "234",
                phone = "numero",
                status = 1
            };

            //Act 
            bool result = false;
            List<String> list = new List<String>();
            list.Add("234");
            list.Add("numero");
            list.Add("");
            if (_clientController.IsDataValid(list) && _clientController.IsEmailExisting(""))
            {
                if (_clientController.RegisterClient(client))
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsFalse(result, "El registro del cliente no debería ser exitoso");
        }

        [TestMethod]
        public void AddNewClient_InvalidEmail_Failure()
        {
            //Arrange
            var client = new Client
            {
               email = "star3oy@hotmail.com", 
               name = "Oscar Pérez Hernández", 
               phone = "2281563487", 
               status = 1
            };

            //Act 
            bool result = false;
            List<String> list = new List<String>();
            list.Add("Oscar Pérez Hernández");
            list.Add("2281563487");
            list.Add("star3oy@hotmail.com");
            if (_clientController.IsDataValid(list) && _clientController.IsEmailExisting("star3oy@hotmail.com"))
            {
                if (_clientController.RegisterClient(client))
                {
                    result = true;
                }
            }

            //Assert
            Assert.IsFalse(result, "El registro del cliente no debería ser exitoso");
        }

    }
}
