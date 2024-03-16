using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.DataLayer;
using System.Data.SqlClient;
using System.Data.Entity.Core;

namespace ItaliaPizza.DataLayer.DAO
{
    public class SupplierAreaDAO : ISupplierArea
    { 
        public List<SupplyArea> GetAllSupplyAreas()
        {
            List<SupplyArea> supplyAreas = new List<SupplyArea>();
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    supplyAreas = databaseContext.SupplyAreas.ToList();
                }
            }
            catch (SqlException)
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (EntityException)
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }

            return supplyAreas;
        }

    }
}
