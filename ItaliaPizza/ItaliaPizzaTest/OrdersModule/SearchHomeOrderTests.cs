using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizzaTest.OrdersModule
{
    [TestClass]
    public class SearchHomeOrderTests
    {
        private OrderDAO _orderDAO = new OrderDAO();

        [TestMethod]
        public void GetHomeOrdersByStatus_PendingPreparation_Success()
        {
            // Act
            List<InternalOrder> pendingOrders = _orderDAO.GetHomeOrdersByStatus(Constants.ORDER_STATUS_PENDING_PREPARATION);

            // Assert
            Assert.IsTrue(pendingOrders.All(order => order.status == Constants.ORDER_STATUS_PENDING_PREPARATION),
              "GetHomeOrdersByStatus should only return pending preparation orders.");
        }

        [TestMethod]
        public void GetHomeOrdersByStatus_Preparing_Success()
        {
            // Act
            List<InternalOrder> preparingOrders = _orderDAO.GetHomeOrdersByStatus(Constants.ORDER_STATUS_PREPARING);

            // Assert
            Assert.IsTrue(preparingOrders.All(order => order.status == Constants.ORDER_STATUS_PREPARING),
              "GetHomeOrdersByStatus should only return preparing orders.");
        }


        [TestMethod]
        public void GetHomeOrdersByStatus_Prepared_Success()
        {
            // Act
            List<InternalOrder> preparedOrders = _orderDAO.GetHomeOrdersByStatus(Constants.ORDER_STATUS_PREPARED);

            // Assert
            Assert.IsTrue(preparedOrders.All(order => order.status == Constants.ORDER_STATUS_PREPARED),
              "GetHomeOrdersByStatus should only return prepared orders.");
        }


        [TestMethod]
        public void GetHomeOrdersByStatus_Sent_Success()
        {
            // Act
            List<InternalOrder> sentOrders = _orderDAO.GetHomeOrdersByStatus(Constants.ORDER_STATUS_SENT);

            // Assert
            Assert.IsTrue(sentOrders.All(order => order.status == Constants.ORDER_STATUS_SENT),
              "GetHomeOrdersByStatus should only return sent orders.");
        }

        [TestMethod]
        public void GetHomeOrdersByStatus_Delivered_Success()
        {
            // Act
            List<InternalOrder> deliveredOrders = _orderDAO.GetHomeOrdersByStatus(Constants.ORDER_STATUS_DELIVERED);

            // Assert
            Assert.IsTrue(deliveredOrders.All(order => order.status == Constants.ORDER_STATUS_DELIVERED),
              "GetHomeOrdersByStatus should only return delivered orders.");
        }
    }
}
