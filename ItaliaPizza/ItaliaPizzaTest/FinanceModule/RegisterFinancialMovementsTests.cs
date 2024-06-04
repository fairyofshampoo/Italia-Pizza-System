using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class RegisterFinancialMovementsTests
    {
        FinancialMovementsController _financeMovementsController = new FinancialMovementsController();


        [TestMethod]
        public void RegisterCashout_Success()
        {
            //Arrange
            var cashout = new Cashout
            {
                date = DateTime.Now,
                total = 2000,
                cashoutType = 0
            };

            //Act
            string total = "2000";
            bool result = false;
            if (_financeMovementsController.ValidateData(total))
            {
                result = _financeMovementsController.RegisterCashout(cashout);
            }

            //Assert
            Assert.IsTrue(result, "El registro de la salida de caja debería ser exitoso");

        }

        [TestMethod]
        public void RegisterCashout_InvalidaData_Failure()
        {
            //Arrange
            var cashout = new Cashout
            {
                date = DateTime.Now,
                cashoutType = 0
            };

            //Act
            string total = " ";
            bool result = false;
            if (_financeMovementsController.ValidateData(total))
            {
                result = _financeMovementsController.RegisterCashout(cashout);
            }

            //Assert
            Assert.IsFalse(result, "El registro de la salida de caja debería ser fallida");
        }

    }
}
