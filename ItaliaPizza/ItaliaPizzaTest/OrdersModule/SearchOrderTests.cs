using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace ItaliaPizzaTest.OrdersModule
{
    [TestClass]
    public class SearchOrderTests
    {
        readonly OrderController _orderController = new OrderController();

        [TestMethod]
        public void ShowOrdersByWaiter()
        {
            //Arrange
            string waiterEmail = "monita@gmail.com";
            int status = 2;
            string internalOrderCode = "020624-89C6";

            //Act
            bool result = false;
            List<InternalOrder> internalOrders = _orderController.GetInternalOrderByStatus(waiterEmail, status);
            if (internalOrders.First().internalOrderId.Equals(internalOrderCode))
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result, "La orden debería ser igual");  
        }

        [TestMethod]
        public void ShowOrdersByOrderStatus()
        {
            //Arrange
            string waiterEmail = "monita@gmail.com";
            int status = 3;
            string internalOrderCode = "020624-89C7";

            //Act
            bool result = false;
            List<InternalOrder> internalOrders = _orderController.GetInternalOrderByStatus(waiterEmail, status);
            if (internalOrders.First().internalOrderId.Equals(internalOrderCode))
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result, "La orden debería ser igual");
        }


    }
}
