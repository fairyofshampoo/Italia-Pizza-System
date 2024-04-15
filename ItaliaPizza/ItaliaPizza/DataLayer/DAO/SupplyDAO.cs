using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class SupplyDAO : ISupply
    {
        public bool AddSupply(Supply supply)
        {
            bool successfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newSupply = new Supply
                    {
                        name = supply.name,
                        amount = supply.amount,
                        measurementUnit = supply.measurementUnit,
                        category = supply.category,
                        expiryDate = supply.expiryDate,
                    };

                    databaseContext.Supplies.Add(newSupply);
                    databaseContext.SaveChanges();

                    successfulRegistration = true;
                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public bool IsSupplyNameExisting(string name)
        {
            bool isSupplyNameExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingName = databaseContext.Supplies.FirstOrDefault(n => n.name == name);
                if (existingName != null)
                {
                    isSupplyNameExisting = true;
                }
            }
            return isSupplyNameExisting;
        }
    }
}
