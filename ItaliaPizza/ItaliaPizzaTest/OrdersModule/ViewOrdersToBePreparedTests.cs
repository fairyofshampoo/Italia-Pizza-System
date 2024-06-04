using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.OrdersModule
{ 
    [TestClass]
    public class ViewOrdersToBePreparedTests
    {
        OrderController _orderController = new OrderController();

        [TestMethod]
        public void ViewOrdersToBePrepared_Success()
        {
            //Arrange
            string internalOrderId = "020624-89C6";
            List<InternalOrder> ordersDB = _orderController.GetOrderToBePrepared();

            //Act
            bool result = false;
            if (internalOrderId.Equals(ordersDB.First().internalOrderId))
            {
                result = true;
            }

            //Assert
            Assert.IsTrue(result, "El id debería ser el mismo");
        }

        [TestMethod]
        public void ViewProductsByOrder_Success()
        {
            //Arrange
            string firstProductId = "PM001";
            string secondProductId = "CHEETOS123";
            string orderId = "020624-89C6";

            //Act
            bool result = false;
            List<Product> productsByOrder = _orderController.GetProductsByOrder(orderId);

            if (secondProductId.Equals(productsByOrder.First().productCode) && firstProductId.Equals(productsByOrder[1].productCode))
            {
                result = true;   
            }

            //Asserts
            Assert.IsTrue(result, "Deberían ser dos productos");
        }

        [TestMethod]
        public void ChangeOrder_Success()
        {
            //Arrange
            int status = 2;
            string orderId = "020624-89C6";

            //Act
            bool result = _orderController.ChangeOrderStatus(status, orderId);

            //Assert
            Assert.IsTrue(result, "El cambio de estado debería ser exitoso");
        }

    }
}
