using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class FinancialMovementsController
    {
        public bool RegisterCashout(Cashout cashout)
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            return cashoutDAO.RegisterCashout(cashout);
        }

        public bool ValidateData(String totalString)
        {
            bool isValid = true;
            try
            {
                decimal total = decimal.Parse(totalString);
            } catch (FormatException) 
            {
                isValid = false;
            }
            return isValid;
        }

    }
}
