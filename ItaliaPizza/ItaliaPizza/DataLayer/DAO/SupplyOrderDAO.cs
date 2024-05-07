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
            bool result = false;
            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var orderToDelete = dbContext.SupplierOrders.FirstOrDefault(order => order.orderCode == supplierOrderId);
                    if (orderToDelete != null)
                    {
                        dbContext.SupplierOrders.Remove(orderToDelete);
                        dbContext.SaveChanges();
                        result = true;
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al eliminar el pedido de proveedor: " + ex.Message);
                    result = false;
                }
            }

            return result;
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

        public bool UpdateStatusOrderAndPayment(int supplierOrderId, byte status, decimal totalPayment)
        {
            bool isUpdated = false;

            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var order = dbContext.SupplierOrders.FirstOrDefault(o => o.orderCode == supplierOrderId);

                    if (order != null)
                    {
                        order.status = status;
                        order.total = totalPayment;
                        dbContext.SaveChanges();

                        isUpdated = true;
                    }
                }
                catch (Exception ex)
                {
                    isUpdated = false;
                    Console.WriteLine("Error al actualizar el estado del pedido y el pago: " + ex.Message);
                }
            }

            return isUpdated;
        }

        public bool AreAnySuppliesInOrder(int supplierOrderId)
        {
            bool anySupplies = false;

            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    anySupplies = dbContext.SupplyOrders.Any(s => s.supplierOrderId == supplierOrderId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error al verificar si hay suministros en el pedido: " + ex.Message);
                }
            }

            return anySupplies;
        }

        public bool DeleteSupplyFromOrder(string supplyName, int orderId)
        {
            bool result = false;
            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                var supplyOrder = dbContext.SupplyOrders
                                           .FirstOrDefault(so => so.supplyId == supplyName && so.supplierOrderId == orderId);

                if (supplyOrder != null)
                {
                    dbContext.SupplyOrders.Remove(supplyOrder);
                    dbContext.SaveChanges();
                    result = true;
                }
            }

            return result;
        }

        public bool UpdateSupplyAmountInOrder(decimal amount, int orderCode, string supplyName)
        {
            bool result = false;
            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                var supplyOrder = dbContext.SupplyOrders
                                           .FirstOrDefault(so => so.supplierOrderId == orderCode && so.supplyId == supplyName);

                if (supplyOrder != null)
                {
                    supplyOrder.quantityOrdered = amount;
                    dbContext.SaveChanges();
                    result = true;
                }
                else
                {
                    result = false;
                }
            }

            return result;
        }

    }
}
