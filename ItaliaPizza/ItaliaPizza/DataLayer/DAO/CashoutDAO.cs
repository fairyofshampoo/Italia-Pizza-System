using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class CashoutDAO : ICashOut
    {
        public bool RegisterCashout(Cashout cashout)
        {
            bool successfulRegistration = true;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    databaseContext.Cashouts.Add(cashout);
                    databaseContext.SaveChanges();
                }
                catch (Exception)
                {
                    successfulRegistration = false;
                }

            }
            return successfulRegistration;
        }
    }
}
