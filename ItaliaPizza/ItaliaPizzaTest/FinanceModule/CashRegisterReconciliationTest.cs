using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class CashRegisterReconciliationTest
    {
        private CashierReportController _controller = new CashierReportController();

        [TestMethod]
        public void TestCreateAndCloseCashierLog()
        {
            //Get any active CashierLog
            CashierLog activeLog = _controller.GetActiveCashierLog();
            Assert.IsNull(activeLog, "There should be no active CashierLog initially.");

            //Create a new CashierLog
            CashierLog newLog = new CashierLog
            {
                openingDate = DateTime.Now,
                initialBalance = 1000m,
                status = 1
            };
            _controller.CreateCashierLog(newLog);

            //Close the active CashierLog
            decimal finalBalanceReal = 1200m;
            DateTime closingDate = DateTime.Now;
            string email = "monita@gmail.com";
            _controller.CloseCashierLog(finalBalanceReal, closingDate, email);

            // Retrieve the created log to verify
            activeLog = _controller.GetActiveCashierLog();
            Assert.IsNotNull(activeLog, "A new CashierLog should be created.");
            Assert.AreEqual(finalBalanceReal, activeLog.initialBalance);

        }
    }
}
