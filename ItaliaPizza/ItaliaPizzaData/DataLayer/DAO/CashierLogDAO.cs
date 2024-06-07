using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO
{
    public class CashierLogDAO
    {
        public List<CashierLog> GetCashierLogsByStatus(byte status)
        {
            List<CashierLog> activeCashierLogs = new List<CashierLog>();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    activeCashierLogs = databaseContext.CashierLogs
                        .Where(cl => cl.status == status)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los registros de cajero activos: {ex.Message}");
            }

            return activeCashierLogs;
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
                        .Where(cl => cl.openingDate >= startDate && cl.openingDate <= endDate)
                        .ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar los registros de cajero: {ex.Message}");
            }

            return cashierLogs;
        }

        public void AddCashierLog(CashierLog newCashierLog)
        {
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    databaseContext.CashierLogs.Add(newCashierLog);
                    databaseContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al agregar el registro de cajero: {ex.Message}");
            }
        }

        public CashierLog GetActiveCashierLog()
        {
            CashierLog activeCashierLog = null;

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    activeCashierLog = databaseContext.CashierLogs
                        .FirstOrDefault(cl => cl.status == 1);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al recuperar el corte de caja activo: {ex.Message}");
            }

            return activeCashierLog;
        }

        public bool CloseActiveCashierLog(decimal finalBalanceReal, DateTime closingDate, string email)
        {
            bool result = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    string query = "EXEC CloseCashierLog @finalBalance, @closingDate, @employeeEmail";
                    int rowsAffected = databaseContext.Database.ExecuteSqlCommand(query,
                        new SqlParameter("@finalBalance", finalBalanceReal),
                        new SqlParameter("@closingDate", closingDate),
                        new SqlParameter("@employeeEmail", email)
                    );

                    if (rowsAffected > 0)
                    {
                        result = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cerrar el corte de caja activo: {ex.Message}");
            }

            return result;
        }
    }
}
