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

        public decimal GetTotalCashoutsByDateAndType(int day, int month, int year, byte cashoutType)
        {
            decimal totalCashouts = 0;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    totalCashouts = databaseContext.Cashouts
                        .Where(c => c.date.Day == day && c.date.Month == month && c.date.Year == year && c.cashoutType == cashoutType)
                        .ToList()
                        .Sum(c => c.total);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cashout by Type error" + ex.Message);
                }
            }

            return totalCashouts;
        }


        public List <CashierLog> GetCashierLogs()
        {
            List<CashierLog> cashierLogs = new List<CashierLog>();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    cashierLogs = databaseContext.CashierLogs.ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los registros de cajero: {ex.Message}");
            }

            return cashierLogs;
        }

        public List<CashierLog> GetCashierLogsByDateRange(DateTime startDate)
        {
            DateTime endDate = DateTime.Now;
            List<CashierLog> cashierLogs = new List<CashierLog>();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    cashierLogs = databaseContext.CashierLogs
                        .Where(cl => cl.creationDate >= startDate && cl.creationDate <= endDate)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los registros de cajero: {ex.Message}");
            }

            return cashierLogs;
        }
    }
}
