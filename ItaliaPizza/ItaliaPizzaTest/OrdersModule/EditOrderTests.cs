using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;

namespace ItaliaPizzaTest.OrdersModule
{
    [TestClass]
    public class EditOrderTests
    {
        private OrderDAO _orderDAO = new OrderDAO();

        [TestMethod]
        public void RemoveProductFromInternalOrderInEdition_Success()
        {
            // Arrange
            string existingOrderCode = "020624-89C6";

            // 1. Get all products for the order
            List<Product> initialProducts = _orderDAO.GetProductsByOrderId(existingOrderCode);

            // Assert (before removal)
            Assert.IsFalse(initialProducts.Count < 0, "The order should have at least one product before removal.");

            // 2. Identify the first product to remove
            string productCodeToRemove = initialProducts[0].productCode;

            // Act
            bool result = _orderDAO.RemoveProductFromOrderInEdition(productCodeToRemove, existingOrderCode);

            // Assert (after removal)
            Assert.IsTrue(result, "The product should be removed from the order successfully.");
        }

        [TestMethod]
        public void RemoveProductFromHomeOrderInEdition_Success()
        {
            // Arrange
            string existingOrderCode = "040624-2RD9";

            // 1. Get all products for the order
            List<Product> initialProducts = _orderDAO.GetProductsByOrderId(existingOrderCode);

            // Assert (before removal)
            Assert.IsFalse(initialProducts.Count < 0, "The order should have at least one product before removal.");

            // 2. Identify the first product to remove
            string productCodeToRemove = initialProducts[0].productCode;

            // Act
            bool result = _orderDAO.RemoveProductFromOrderInEdition(productCodeToRemove, existingOrderCode);

            // Assert (after removal)
            Assert.IsTrue(result, "The product should be removed from the order successfully.");
        }
    }
}
