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

        public bool IncreaseSupplyAmountInOrder(Supply supply, int orderCode)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var supplyOrderDB = databaseContext.SupplyOrders
                                              .Where(supplyOrder => supplyOrder.supplierOrderId == orderCode && supplyOrder.supplyId == supply.name)
                                              .FirstOrDefault();
                if (supplyOrderDB != null)
                {
                    supplyOrderDB.quantityOrdered += 1;
                    databaseContext.SaveChanges();
                    operationStatus = true;
                }
            }
            return operationStatus;
        }

        public decimal GetOrderedQuantityBySupplierOrderId(int supplierOrderId, string supplyId)
        {
            decimal orderedQuantity = 0;

            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                var supplyOrder = dbContext.SupplyOrders
                                           .FirstOrDefault(s => s.supplierOrderId == supplierOrderId && s.supplyId == supplyId);

                if (supplyOrder != null)
                {
                    orderedQuantity = supplyOrder.quantityOrdered ?? 0;
                }
            }

            return orderedQuantity;
        }


        public bool AddSupplyToOrder(string supplyName, int orderId, decimal quantityOrdered)
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
                            supplierOrderId = orderId,
                            quantityOrdered = quantityOrdered
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

        public bool IsSupplyAlreadyInOrder(string supplyName, int orderId)
        {
            bool result = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var register = databaseContext.SupplyOrders
                                              .Where(supplyOrder => supplyOrder.supplyId == supplyName && supplyOrder.supplierOrderId == orderId)
                                              .FirstOrDefault();
                if (register != null)
                {
                    result = true;
                }
            }
            return result;
        }

        public List<Supply> GetSuppliesByOrderId(int supplierOrderId)
        {
            List<Supply> supplies = new List<Supply>();

            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                var orderSupplies = dbContext.SupplyOrders
                                             .Where(so => so.supplierOrderId == supplierOrderId)
                                             .ToList();

                foreach (var orderSupply in orderSupplies)
                {
                    var supply = dbContext.Supplies.FirstOrDefault(s => s.name == orderSupply.supplyId);
                    if (supply != null)
                    {
                        supplies.Add(supply);
                    }
                }
            }

            return supplies;
        }

    }
}
