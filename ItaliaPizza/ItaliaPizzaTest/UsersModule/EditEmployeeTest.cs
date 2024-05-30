using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class EditEmployeeTest
    {
        private readonly EmployeeController _employeeController = new EmployeeController();

        [TestMethod]
        public void EditEmployee_ValidData_Success()
        {
            var employee = new Employee
            {
                name = "Diego Camo Quito",
                phone = "2282726786",
                email = "diego123@gmail.com",
                role = "Cocinero"
            };

            var account = new Account()
            {
                user = "camoquito",
                password = "8Elcamo*"
            };

            // Register employee first
            bool initialResult = _employeeController.AddEmployee(employee, account);
            Assert.IsTrue(initialResult, "El registro iniical del empleado debería ser exitoso");

            // Simulate edit
            account.password = "5elCamo*"; // Changed password
            
            bool editResult = false;
            
            if (_employeeController.IsUpdateEmployeeValid(employee, account))
            {
                if (_employeeController.ModifyEmployee(employee, account))
                {
                    editResult = true;
                }
            }

            Assert.IsTrue(editResult, "La modificación del empleado debería ser exitosa con datos válidos");
        }

        [TestMethod]
        public void EditEmployee_InvalidData_Fail()
        {
            var employee = new Employee
            {
                name = "Freddy Mendoza Hernández",
                phone = "2281345627",
                email = "america10@gmail.com",
                role = "Mesero"
            };

            var account = new Account()
            {
                user = "fred",
                password = "1Americ%"
            };

            // Register employee first
            bool initialResult = _employeeController.AddEmployee(employee, account);
            Assert.IsTrue(initialResult, "El registro iniical del empleado debería ser exitoso");

            // Simulate edit with invalid password
            account.password = "Incorrect password";

            bool editResult = false;

            if (_employeeController.IsUpdateEmployeeValid(employee, account))
            {
                if (_employeeController.ModifyEmployee(employee, account))
                {
                    editResult = true;
                }
            }

            Assert.IsFalse(editResult, "La modificación del empleado debería fallar con datos inválidos");
        }

        [TestMethod]
        public void DesactivateEmployee_Success()
        {
            var employee = new Employee
            {
                name = "Alonso Vázquez del Moral",
                phone = "2283721786",
                email = "alonsoV@gmail.com",
                role = "Cocinero"
            };

            var account = new Account()
            {
                user = "alonso",
                password = "*Alons0v"
            };

            // Register employee first
            bool initialResult = _employeeController.AddEmployee(employee, account);
            Assert.IsTrue(initialResult, "El registro iniical del empleado debería ser exitoso");

            // Simulate desactivate
            bool desactivateResult = _employeeController.ModifyEmployeeStatus(account.user, Constants.INACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación del empleado debería ser exitosa.");
        }

        [TestMethod]
        public void ActivateEmployee_Success()
        {
            var employee = new Employee
            {
                name = "Alonso Vázquez del Moral",
                phone = "2283721786",
                email = "alonsoV@gmail.com",
                role = "Cocinero"
            };

            var account = new Account()
            {
                user = "alonso",
                password = "*Alons0v",
            };            

            // Register employee first
            bool initialResult = _employeeController.AddEmployee(employee, account);
            Assert.IsTrue(initialResult, "El registro iniical del empleado debería ser exitoso");

            // Desactivate employee
            bool desactivateResult = _employeeController.ModifyEmployeeStatus(account.user, Constants.INACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación del empleado debería ser exitosa.");

            // Simulate activate
            bool activateResult = _employeeController.ModifyEmployeeStatus(account.user, Constants.ACTIVE_STATUS);
            Assert.IsTrue(activateResult, "La activación del empleado debería ser exitosa");
        }
    }
}
