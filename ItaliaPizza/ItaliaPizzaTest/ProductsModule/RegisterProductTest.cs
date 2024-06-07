using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class RegisterProductTest
    {
        private readonly ProductController _productController = new ProductController();

        [TestMethod]
        public void AddExternalProduct_ValidData_Success()
        {
            var product = new Product
            {
                name = "Dr Pepper 355ml",
                productCode = "DP001",
                price = 15,
                description = "Dr Pepper de 355 ml. Una bebida gaseosa con un sabor único y complejo, " +
                              "que combina 23 sabores diferentes. Popular por su distintivo sabor, " +
                              "es una excelente opción para quienes buscan algo diferente.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 25,
            };

            var supply = new Supply
            {
                name = product.name,
                amount = product.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product.productCode,
                status = true
            };

            bool result = false;

            if (_productController.IsNewExternalProductValid(product) &&
               !_productController.IsProductCodeDuplicated(product.productCode))
            {
                if (_productController.AddProductExternal(product, supply))
                {
                    result = true;
                }
            }

            Assert.IsTrue(result, "El registro del producto debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddExternalProduct_InvalidData_Failure()
        {
            var product = new Product
            {
                name = "Fanta 355ml",
                productCode = "FA001",
                price = -15, // Invalid price
                description = "Fanta de 355 ml. Una bebida gaseosa con un vibrante sabor a naranja, " +
                              "popular entre los jóvenes y adultos por igual. Perfecta para aquellos " +
                              "que buscan una bebida afrutada y refrescante.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 0, // Invalid amount
            };

            var supply = new Supply
            {
                name = product.name,
                amount = product.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product.productCode,
                status = true
            };

            bool result = false;

            if (_productController.IsNewExternalProductValid(product) &&
               !_productController.IsProductCodeDuplicated(product.productCode))
            {
                if (_productController.AddProductExternal(product, supply))
                {
                    result = true;
                }
            }

            Assert.IsFalse(result, "El registro del producto no debería ser exitoso con datos inválidos.");
        }

        [TestMethod]
        public void AddExternalProduct_DuplicatedProduct_Failure()
        {
            var product = new Product
            {
                name = "Dr Pepper 650ml",
                productCode = "DP001",
                price = 15,
                description = "Dr Pepper de 650 ml. Una bebida gaseosa con un sabor único y complejo, " +
                              "que combina 23 sabores diferentes. Popular por su distintivo sabor, " +
                              "es una excelente opción para quienes buscan algo diferente.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 20,
            };

            var supply = new Supply
            {
                name = product.name,
                amount = product.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product.productCode,
                status = true
            };

            bool result = false;

            if (_productController.IsNewExternalProductValid(product) &&
               !_productController.IsProductCodeDuplicated(product.productCode))
            {
                if (_productController.AddProductExternal(product, supply))
                {
                    result = true;
                }
            }

            Assert.IsFalse(result, "El registro del producto no debería ser exitoso si el producto ya existe.");
        }
    }
}
