using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Security.Policy;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class EditProductTest
    {
        private readonly ProductController _productController = new ProductController();

        [TestMethod]
        public void EditProduct_ValidData_Success()
        {
            var product = new Product
            {
                name = "Sprite 355ml",
                productCode = "SPT001",
                price = 20,
                description = "Sprite de 345 ml. Una bebida carbonatada con sabor a limón y lima, libre de cafeína. " +
                              "Conocida por su sabor refrescante y burbujeante, ideal para acompañar comidas ligeras" +
                              " o disfrutar sola.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 10,
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

            // Register product and supply first
            bool initialResult = _productController.AddProductExternal(product, supply);
            Assert.IsTrue(initialResult, "El registro inicial del producto externo debería ser exitoso.");

            // Simulate edit
            product.description = "Sprite de 355 ml. Una bebida carbonatada con sabor a limón y lima, libre de cafeína. " +
                              "Conocida por su sabor refrescante y burbujeante, ideal para acompañar comidas ligeras" +
                              " o disfrutar sola.";
            product.price = 25;

            bool editResult = false;

            if (_productController.IsModificatedExternalProductValid(product))
            {
                if (_productController.ModifyProduct(product, product.productCode))
                {
                    editResult = true;
                }
            }
            Assert.IsTrue(editResult, "La modificación del producto debería ser exitosa con datos válidos");
        }

        [TestMethod]
        public void EditProduct_InvalidData_Failure()
        {
            var product = new Product
            {
                name = "Coca-Cola 355ml",
                productCode = "CC001",
                price = 25,
                description = "Una de las bebidas carbonatadas más populares en todo el mundo, conocida por " +
                              "su sabor único y refrescante. Ideal para acompañar cualquier comida.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 10,
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

            // Register product and supply first
            bool initialResult = _productController.AddProductExternal(product, supply);
            Assert.IsTrue(initialResult, "El registro inicial del producto externo debería ser exitoso.");

            // Simulate edit
            product.description = "";
            product.price = -34;

            bool editResult = false;

            if (_productController.IsModificatedExternalProductValid(product))
            {
                if (_productController.ModifyProduct(product, product.productCode))
                {
                    editResult = true;
                }
            }
            Assert.IsFalse(editResult, "La modificación del producto no debería ser exitosa con datos inválidos");
        }

        [TestMethod]
        public void DesactivateProduct_Success()
        {
            var product = new Product
            {
                name = "Pepsi 355ml",
                productCode = "PP001",
                price = 25,
                description = "Pepsi de 355 ml. Una bebida gaseosa con un sabor dulce y refrescante, perfecta para " +
                              "disfrutar en cualquier ocasión. Conocida por su sabor único y su capacidad para complementar " +
                              "una gran variedad de alimentos.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 30,
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

            // Register product and supply first
            bool initialResult = _productController.AddProductExternal(product, supply);
            Assert.IsTrue(initialResult, "El registro inicial del producto externo debería ser exitoso.");

            bool desactivateResult = _productController.ChangeProductStatus(product, Constants.INACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación del producto debería ser exitosa.");
        }

        [TestMethod]
        public void ActivateProduct_Success()
        {
            var product = new Product
            {
                name = "Fanta 355ml",
                productCode = "FA001",
                price = 25,
                description = "Fanta de 355 ml. Una bebida gaseosa con un vibrante sabor a naranja, popular entre los jóvenes " +
                              "y adultos por igual. Perfecta para aquellos que buscan una bebida afrutada y refrescante.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.INACTIVE_STATUS,
                amount = 30,
            };

            var supply = new Supply
            {
                name = product.name,
                amount = product.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product.productCode,
                status = false
            };

            // Register product and supply first
            bool initialResult = _productController.AddProductExternal(product, supply);
            Assert.IsTrue(initialResult, "El registro inicial del producto externo debería ser exitoso.");

            bool desactivateResult = _productController.ChangeProductStatus(product, Constants.ACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación del producto debería ser exitosa.");
        }
    }
}
