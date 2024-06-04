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
    public class RegisterSupplierOrderTest
    {
        private readonly SupplierOrderController _supplierOrderController = new SupplierOrderController();
        private SupplyDAO _supplyDAO = new SupplyDAO();
        private ProductDAO _productDAO = new ProductDAO();
        private Supplier _testSupplier;
        private Supply _testSupply;
        private Product _testProduct;

        [TestMethod]
        public void RegisterSupplyOrder_NormalFlow_Success()
        {
            // Arrange
            // Crear un proveedor de prueba
            _testSupplier = new Supplier
            {
                email = "test@gmail.com",
                companyName = "Test Company",
                manager = "Test Manager",
                SupplyAreas = new List<SupplyArea>
                {
                    new SupplyArea { area_name = "Bebidas" }
                }
            };

            _testProduct = new Product
            {
                name = "Test Product",
                price = 100.00M,
                status = Constants.ACTIVE_STATUS,
                productCode = "TEST01",
                description = "Test Description",
                amount = 100,
                isExternal = Constants.ACTIVE_STATUS

            };
            _productDAO.AddProduct(_testProduct);

            // Crear un suministro de prueba
            _testSupply = new Supply
            {
                name = "Test Supply",
                amount = 100.00M,
                SupplyArea = new SupplyArea { area_name = "Bebidas" },
                category = 6,
                measurementUnit = "kg",
                status = true,
                productCode = _testProduct.productCode
            };

            _supplyDAO.AddSupply(_testSupply);

            // Act
            int orderId = _supplierOrderController.CreateSupplierOrder(_testSupplier.email);
            bool isSupplyAdded = _supplierOrderController.AddSupplyToOrder(_testSupply.name, orderId, 10);
            bool isPaymentRegistered = _supplierOrderController.RegisterPayment(2000.00M, orderId);

            // Assert
            Assert.AreNotEqual(Constants.UNSUCCESSFUL_RESULT, orderId);
            Assert.IsTrue(isSupplyAdded);
            Assert.IsTrue(isPaymentRegistered);
        }

        [TestMethod]
        public void ValidateTotalPayment_ValidPayment_ReturnsTrue()
        {
            // Arrange
            string totalPayment = "100.00";
            bool expectedResult = true;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateTotalPayment_ZeroPayment_ReturnsFalse()
        {
            // Arrange
            string totalPayment = "0.00";
            bool expectedResult = false;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateTotalPayment_NegativePayment_ReturnsFalse()
        {
            // Arrange
            string totalPayment = "-100.00";
            bool expectedResult = false;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateTotalPayment_MoreThanTwoDecimals_ReturnsFalse()
        {
            // Arrange
            string totalPayment = "100.001";
            bool expectedResult = false;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateTotalPayment_NonNumeric_ReturnsFalse()
        {
            // Arrange
            string totalPayment = "abc";
            bool expectedResult = false;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }

        [TestMethod]
        public void ValidateTotalPayment_EmptyString_ReturnsFalse()
        {
            // Arrange
            string totalPayment = "";
            bool expectedResult = false;

            // Act
            bool result = _supplierOrderController.ValidateTotalPayment(totalPayment);

            // Assert
            Assert.AreEqual(expectedResult, result);
        }
    }
}