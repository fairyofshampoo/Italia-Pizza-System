using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizzaData.DataLayer.DAO.Interface;
using System.Data.SqlClient;
using System.Data.Entity.Core;

namespace ItaliaPizzaData.DataLayer.DAO
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
                throw new Exception("Error al obtener las áreas de suministro");
            }
            catch (EntityException)
            {
                throw new Exception("Error al obtener las áreas de suministro");
            }

            return supplyAreas;
        }

        public int GetSupplyAreaIdByName(string name)
        {
            int supplyAreaId = -1;

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    SupplyArea supplyArea = databaseContext.SupplyAreas.FirstOrDefault(s => s.area_name == name);

                    if (supplyArea != null)
                    {
                        supplyAreaId = supplyArea.area_id;
                    }
                }
            }
            catch (SqlException)
            {
                throw new Exception("Error al obtener el id del área de suministro");
            }
            catch (EntityException)
            {
                throw new Exception("Error al obtener el id del área de suministro");
            }
            return supplyAreaId;
        }

    }
}
