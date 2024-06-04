using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ItaliaPizzaTest.FinanceModule
{
    [TestClass]
    public class SearchSupplierOrderTest
    {
        private readonly SupplierOrderController _supplierOrderController = new SupplierOrderController();

        [TestMethod]
        public void GetOrdersBySupplier_ValidEmail_ReturnsOrders()
        {
            // Arrange
            string supplierEmail = "test@gmail.com";
            List<SupplierOrder> expectedOrders = new List<SupplierOrder>
            {
                new SupplierOrder { orderCode = 4, supplierId = supplierEmail, status = 1, date = new DateTime(2024, 5, 29, 19, 36, 56, 183), total = 2000.00M },
                new SupplierOrder { orderCode = 5, supplierId = supplierEmail, status = 1, date = new DateTime(2024, 6, 1, 18, 22, 44, 320), total = 2000.00M },
                new SupplierOrder { orderCode = 6, supplierId = supplierEmail, status = 2, date = new DateTime(2024, 5, 29, 19, 36, 56, 183), total = 2000.00M },
                new SupplierOrder { orderCode = 7, supplierId = supplierEmail, status = 0, date = new DateTime(2024, 6, 1, 18, 22, 44, 320), total = 2000.00M }
            };

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetOrdersBySupplier(supplierEmail);

            // Assert
            Assert.AreEqual(expectedOrders.Count, result.Count);
            for (int i = 0; i < expectedOrders.Count; i++)
            {
                Assert.AreEqual(expectedOrders[i].orderCode, result[i].orderCode);
                Assert.AreEqual(expectedOrders[i].supplierId, result[i].supplierId);
                Assert.AreEqual(expectedOrders[i].status, result[i].status);
                Assert.AreEqual(expectedOrders[i].date, result[i].date);
                Assert.AreEqual(expectedOrders[i].total, result[i].total);
            }
        }

        [TestMethod]
        public void GetOrdersBySupplier_InvalidEmail_ReturnsEmptyList()
        {
            // Arrange
            string supplierEmail = "invalid@supplier.com";

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetOrdersBySupplier(supplierEmail);

            // Assert
            Assert.AreEqual(0, result.Count);
        }

        [TestMethod]
        public void GetActiveOrdersBySupplier_ValidEmail_ReturnsActiveOrders()
        {
            // Arrange
            string supplierEmail = "test@gmail.com";
            List<SupplierOrder> expectedOrders = new List<SupplierOrder>
            {
                new SupplierOrder { orderCode = 4, supplierId = supplierEmail, status = 1, date = new DateTime(2024, 5, 29, 19, 36, 56, 183), total = 2000.00M },
                new SupplierOrder { orderCode = 5, supplierId = supplierEmail, status = 1, date = new DateTime(2024, 6, 1, 18, 22, 44, 320), total = 2000.00M }
            };

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetActiveOrdersBySupplier(supplierEmail);

            // Assert
            Assert.AreEqual(expectedOrders.Count, result.Count);
            for (int i = 0; i < expectedOrders.Count; i++)
            {
                Assert.AreEqual(expectedOrders[i].orderCode, result[i].orderCode);
                Assert.AreEqual(expectedOrders[i].supplierId, result[i].supplierId);
                Assert.AreEqual(expectedOrders[i].status, result[i].status);
                Assert.AreEqual(expectedOrders[i].date, result[i].date);
                Assert.AreEqual(expectedOrders[i].total, result[i].total);
            }
        }

        [TestMethod]
        public void GetCompleteOrdersBySupplier_ValidEmail_ReturnsCompleteOrders()
        {
            // Arrange
            string supplierEmail = "test@gmail.com";
            List<SupplierOrder> expectedOrders = new List<SupplierOrder>
            {
                new SupplierOrder { orderCode = 6, supplierId = supplierEmail, status = 2, date = new DateTime(2024, 5, 29, 19, 36, 56, 183), total = 2000.00M }
            };

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetCompleteOrdersBySupplier(supplierEmail);

            // Assert
            Assert.AreEqual(expectedOrders.Count, result.Count);
            for (int i = 0; i < expectedOrders.Count; i++)
            {
                Assert.AreEqual(expectedOrders[i].orderCode, result[i].orderCode);
                Assert.AreEqual(expectedOrders[i].supplierId, result[i].supplierId);
                Assert.AreEqual(expectedOrders[i].status, result[i].status);
                Assert.AreEqual(expectedOrders[i].date, result[i].date);
                Assert.AreEqual(expectedOrders[i].total, result[i].total);
            }
        }

        [TestMethod]
        public void GetCanceledOrdersBySupplier_ValidEmail_ReturnsCanceledOrders()
        {
            // Arrange
            string supplierEmail = "test@gmail.com";
            List<SupplierOrder> expectedOrders = new List<SupplierOrder>
            {
                new SupplierOrder { orderCode = 7, supplierId = supplierEmail, status = 0, date = new DateTime(2024, 6, 1, 18, 22, 44, 320), total = 2000.00M }
            };

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetCanceledOrdersBySupplier(supplierEmail);

            // Assert
            Assert.AreEqual(expectedOrders.Count, result.Count);
            for (int i = 0; i < expectedOrders.Count; i++)
            {
                Assert.AreEqual(expectedOrders[i].orderCode, result[i].orderCode);
                Assert.AreEqual(expectedOrders[i].supplierId, result[i].supplierId);
                Assert.AreEqual(expectedOrders[i].status, result[i].status);
                Assert.AreEqual(expectedOrders[i].date, result[i].date);
                Assert.AreEqual(expectedOrders[i].total, result[i].total);
            }
        }

        [TestMethod]
        public void GetOrdersByDateRange_NoMatchingOrders_ReturnsEmptyList()
        {
            // Arrange
            string supplierEmail = "test@gmail.com";
            DateTime startDate = new DateTime(2023, 1, 1);
            DateTime endDate = new DateTime(2023, 12, 31);

            // Act
            List<SupplierOrder> result = _supplierOrderController.GetOrdersByDateRange(supplierEmail, startDate, endDate);

            // Assert
            Assert.AreEqual(0, result.Count);
        }
    }
}