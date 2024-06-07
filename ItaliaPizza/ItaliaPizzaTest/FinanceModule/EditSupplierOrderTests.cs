using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class EditSupplierOrderTests
    {
        private readonly SupplierOrderController _supplierOrderController = new SupplierOrderController();
        private readonly SupplyOrderDAO _supplyOrderDAO = new SupplyOrderDAO();


        [TestMethod]
        public void UpdateSuppliesInOrder_UpdatesAmount()
        {
            // Arrange
            int orderId = 4;
            string supplyName = "Test Supply";
            decimal newAmount = 50.0M;

            // Act
            bool result = _supplierOrderController.UpdateSuppliesInOrder(supplyName, orderId, newAmount);

            // Assert
            Assert.IsTrue(result);
            decimal updatedAmount = _supplierOrderController.GetAmountOfSupplyInOrder(supplyName, orderId);
            Assert.AreEqual(newAmount, updatedAmount);
        }

        [TestMethod]
        public void UpdateRemovingSuppliesInOrder_RemovesSupply()
        {
            // Arrange
            int orderId = 4;
            string supplyName = "Test Supply";

            // Act
            bool result = _supplierOrderController.UpdateRemovingSuppliesInOrder(supplyName, orderId);

            // Assert
            Assert.IsTrue(result);
            decimal amount = _supplierOrderController.GetAmountOfSupplyInOrder(supplyName, orderId);
            Assert.AreEqual(0.0M, amount);
        }

        [TestMethod]
        public void DeleteSupplierOrder_ValidOrder_DeletesOrder()
        {
            // Arrange
            int orderId = 5;

            // Act
            bool result = _supplierOrderController.DeleteSupplierOrder(orderId);

            // Assert
            Assert.IsTrue(result);
            List<SupplierOrder> orders = _supplierOrderController.GetOrdersBySupplier("test@gmail.com");
            SupplierOrder order = orders.Find(o => o.orderCode == orderId);
            Assert.IsNull(order);
        }

        [TestMethod]
        public void UpdateInventory_ValidOrder_UpdatesInventoryAndReturnsTrue()
        {
            // Arrange
            int orderCode = 8;

            // Act
            bool result = _supplierOrderController.UpdateInventory(orderCode);

            // Assert
            Assert.IsTrue(result);

            // Additional Assertions if needed
            var updatedOrder = _supplyOrderDAO.GetOrderById(orderCode);
            Assert.AreEqual(Constants.COMPLETE_STATUS, updatedOrder.status);
        }

        [TestMethod]
        public void CancelOrder_ValidOrder_UpdatesStatusToInactive()
        {
            // Arrange
            int orderCode = 9;

            // Act
            bool result = _supplierOrderController.CancelOrder(orderCode);

            // Assert
            Assert.IsTrue(result);

            var canceledOrder = _supplyOrderDAO.GetOrderById(orderCode);
            Assert.AreEqual(Constants.INACTIVE_STATUS, canceledOrder.status);
        }

        [TestMethod]
        public void ChangeOrderToReceived_ValidOrder_UpdatesStatusToComplete()
        {
            // Arrange
            int orderCode = 9;

            // Act
            bool result = _supplierOrderController.ChangeOrderToReceived(orderCode);

            // Assert
            Assert.IsTrue(result);

            var receivedOrder = _supplyOrderDAO.GetOrderById(orderCode);
            Assert.AreEqual(Constants.COMPLETE_STATUS, receivedOrder.status);
        }

    }
}
