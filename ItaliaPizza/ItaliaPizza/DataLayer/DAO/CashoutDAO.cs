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

        public decimal GetTotalCashoutsByDateAndType(DateTime date, byte cashoutType)
        {
            decimal totalCashouts = 0;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    totalCashouts = databaseContext.Cashouts
                        .Where(c => c.date >= date && c.cashoutType == cashoutType)
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

        public decimal GetSumOfCashin()
        {
            decimal totalCashin = 0;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    totalCashin = databaseContext.Cashouts
                        .Where(c => c.cashoutType == 1)
                        .ToList()
                        .Sum(c => c.total);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cashin total error" + ex.Message);
                }
            }

            return totalCashin;
        }

        public decimal GetSumOfCashout()
        {
            decimal totalCashout = 0;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    totalCashout = databaseContext.Cashouts
                        .Where(c => c.cashoutType == 0)
                        .ToList()
                        .Sum(c => c.total);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Cashout total error" + ex.Message);
                }
            }

            return totalCashout;
        }
    }
}
