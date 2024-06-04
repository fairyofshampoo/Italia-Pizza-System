using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class SearchExistingClientTests
    {
        private readonly ClientController _clientController = new ClientController();

        [TestMethod]
        public void SearchClientByName_Success()
        {
            //Arrange
            var newClient = new Client
            {
                email = "star3oy@hotmail.com",
                name = "Eduardo Carrera Colorado",
                phone = "2731348124",
                status = 0
            };

            //Act
            bool result = false;
            List<Client> clients = _clientController.SearchClientByName("Eduardo Carrera Colorado");
            if (clients.Any())
            {
                Console.WriteLine("Nombre: " + clients.First().name);
                Console.WriteLine("Email: " + clients.First().email);
                result = true;
            }

            //Assert
            Assert.IsTrue(result, "El usuario es el mismo");
            
        }

        [TestMethod]
        public void SearchClientByPhoneNumber_Success()
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
            List<Client> clients = _clientController.SearchClientByPhoneNumber("2731348124");
            if (client.email.Equals(clients.First().email))
            {
                result = true;
            }
            //Assert
            Assert.IsTrue(result, "El usuario es el mismo");
        }

        [TestMethod]
        public void SearchClientByAddress_Success()
        {
            //Arrange
            var client = new Client
            {
                email = "lalito@gmail.com",
                name = "Eduardo Carrera Colorado",
                phone = "2737057940",
                status = 1
            };

            //Act
            bool result = false;
            List<Client> clients = _clientController.SearchClientByAddress("Guadalupe Victoria 33");
            if (client.email.Equals(clients.First().email))
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result, "El usuario es el mismo");
        }
    }
}
