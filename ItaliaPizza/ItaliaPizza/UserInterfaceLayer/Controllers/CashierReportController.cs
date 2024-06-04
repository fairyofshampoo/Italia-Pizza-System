using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class CashierReportController
    {

        CashierLogDAO cashierLogDAO = new CashierLogDAO();
        public CashierLog GetActiveCashierLog()
        {
            return cashierLogDAO.GetActiveCashierLog();
        }

        public void CreateCashierLog(CashierLog cashierLog)
        {
            CashierLogDAO cashierLogDAO = new CashierLogDAO();
            cashierLogDAO.AddCashierLog(cashierLog);
        }

        public void CloseCashierLog(decimal finalBalanceReal, DateTime closingDate, string email)
        {
            cashierLogDAO.CloseActiveCashierLog(finalBalanceReal, closingDate, email);
        }
    }
}
