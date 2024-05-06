using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.UserInterfaceLayer.KitchenModule;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class SupplyOrderDAO : ISupplyOrder
    {
        public int AddSupplierOrder(SupplierOrder supplierOrder)
        {
            int result = Constants.UNSUCCESSFUL_RESULT;
            try
            {
                using (var dbContext = new ItaliaPizzaDBEntities())
                {
                    dbContext.SupplierOrders.Add(supplierOrder);

                    int rowsAffected = dbContext.SaveChanges();
                    if (rowsAffected > 0 )
                    {
                        result = supplierOrder.orderCode;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error al agregar la orden de suministro: " + ex.Message);
                throw ex;
            }

            return result;
        }


        public bool AddSupplyToOrder(string supplyName, int orderId)
        {
            bool supplyAdded = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var supply = databaseContext.Supplies.FirstOrDefault(s => s.name == supplyName);

                    if (supply != null && databaseContext.SupplierOrders.Any(o => o.orderCode == orderId))
                    {

                        var orderSupply = new SupplyOrder
                        {
                            supplyId = supply.name,
                            supplierOrderId = orderId
                        };

                        databaseContext.SupplyOrders.Add(orderSupply);

                        databaseContext.SaveChanges();

                        supplyAdded = true;
                    }
                }
                catch (Exception)
                {
                    supplyAdded = false;
                }
            }

            return supplyAdded;
        }



        public bool DeleteSupplierOrder(int supplierOrderId)
        {
            throw new NotImplementedException();
        }
    }
}
