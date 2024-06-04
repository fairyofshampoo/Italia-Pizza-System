using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class LoginTests
    {
        private EmployeeController _employeeController = new EmployeeController();

        [TestMethod]
        public void AuthenticateUser_ReturnsTrue_WhenCredentialsAreValid()
        {
            // Arrange
            string username = "michi";
            string password = "@Michelle1";
            string passwordHashed = Encription.ToSHA2Hash(password);

            // Act
            var result = _employeeController.AuthenticateUser(username, passwordHashed);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void AuthenticateUser_ReturnsFalse_WhenCredentialsAreInvalid()
        {
            // Arrange
            string username = "invalidUser";
            string password = "invalidPassword";

            // Act
            var result = _employeeController.AuthenticateUser(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void VerifyLoginFields_ReturnsTrue_WhenFieldsAreValid()
        {
            // Arrange
            string username = "michi";
            string password = "@Michelle1";

            // Act
            var result = _employeeController.VerifyLoginFields(username, password);

            // Assert
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void VerifyLoginFields_ReturnsFalse_WhenFieldsAreInvalid()
        {
            // Arrange
            string username = "invalidUsername";
            string password = "invalid";

            // Act
            var result = _employeeController.VerifyLoginFields(username, password);

            // Assert
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void GetAccountData_ReturnsAccount_WhenUsernameIsValid()
        {
            // Arrange
            string username = "michi";

            // Act
            var result = _employeeController.GetAccountData(username);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(username, result.user);
        }
    }
}
