using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.SymbolStore;
using System.Security.Principal;

namespace ItaliaPizzaTest.UsersModule
{
    [TestClass]
    public class RegisterEmployeeTest
    {
        private readonly EmployeeController _employeeController = new EmployeeController();

        [TestMethod]
        public void AddEmployee_ValidData_Success()
        {
            var employee = new Employee
            {
                name = "Martín Toral Espejo",
                phone = "2282756789",
                email = "martin123@gmail.com",
                role = "Cocinero"
            };

            var account = new Account()
            {
                user = "martores",
                password = "8martEs*"
            };

            bool result = false;

            if (_employeeController.IsNewEmployeeValid(employee, account) &&
               !_employeeController.IsUserDuplicated(account.user) &&
               !_employeeController.IsEmailDuplicated(employee.email))
            {
                if (_employeeController.AddEmployee(employee, account))
                {
                    result = true;
                }
            }

            Assert.IsTrue(result, "El registro del empleado debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddEmployee_InvalidData_Failure()
        {
            var invalidEmployee = new Employee
            {
                name = "", //Invalid employee name format
                phone = "sdwaftb", //Invalid phone format
                email = "invalidEmail", //Invalid email format
                role = "" //Invalid role
            };

            var invalidAccount = new Account()
            {
                user = "invalid user", //Invalid user format
                password = "12345" //Invalid password format
            };
            bool result = false;

            if (_employeeController.IsNewEmployeeValid(invalidEmployee, invalidAccount) &&
               !_employeeController.IsUserDuplicated(invalidAccount.user) &&
               !_employeeController.IsEmailDuplicated(invalidEmployee.email))
            {
                if (_employeeController.AddEmployee(invalidEmployee, invalidAccount))
                {
                    result = true;
                }
            }

            Assert.IsFalse(result, "El registro del empleado no debería ser exitoso con datos inválidos.");
        }

        [TestMethod]
        public void AddEmployee_DuplicateEmployee_Failure()
        {
            var employee = new Employee
            {
                name = "Camilo Cardone del Moral",
                phone = "2281345627",
                email = "mirinda@gmail.com",
                role = "Mesero"
            };

            var account = new Account()
            {
                user = "camiloesan",
                password = "*1Camilo"
            };

            bool result = false;

            if (_employeeController.IsNewEmployeeValid(employee, account) &&
               !_employeeController.IsUserDuplicated(account.user) &&
               !_employeeController.IsEmailDuplicated(employee.email))
            {
                if (_employeeController.AddEmployee(employee, account))
                {
                    result = true;
                }
            }

            Assert.IsFalse(result, "El registro del empleado no debería ser exitoso si el empleado ya existe.");
        }
    }
      
}
