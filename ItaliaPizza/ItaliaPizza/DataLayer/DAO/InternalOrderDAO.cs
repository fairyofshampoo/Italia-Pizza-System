using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class InternalOrderDAO : IInternalOrder
    {
        public InternalOrder GetInternalOrdersByNumber(int numberOrder, string waiterEmail)
        {
            InternalOrder internalOrder = new InternalOrder();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var ordersDB = databaseContext.InternalOrders
                                              .Where(order => order.internalOrderId == numberOrder && order.waiterName == waiterEmail)
                                              .FirstOrDefault();
                if(ordersDB != null)
                {
                    internalOrder = ordersDB;
                }
                                             
            }
            return internalOrder;
        }

        public List<InternalOrder> GetInternalOrdersByStatus(string status, string waiterEmail)
        {
           List<InternalOrder> internalOrders = new List<InternalOrder>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var ordersDB = databaseContext.InternalOrders
                                              .Where(order => order.status == status && order.waiterName == waiterEmail)
                                              .ToList();
                if(ordersDB != null)
                {
                    foreach(var order in ordersDB)
                    {
                        internalOrders.Add(order);
                    }
                }
            }
            return internalOrders;
        }
    }
}
