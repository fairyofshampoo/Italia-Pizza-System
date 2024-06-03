using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class SearchProductTest
    {
        private readonly ProductController _productController = new ProductController();

        [TestMethod]
        public void SearchProductByName_ValidSearch_ReturnsProducts()
        {
            string searchText = "Sprite 1L";

            var product = new Product
            {
                name = "Sprite 1L",
                productCode = "SPT002",
                price = 25,
                description = "Product for search product by name test",
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

            _productController.AddProductExternal(product, supply);
            
            List<Product> products = _productController.SearchProductByName(searchText);

            Assert.IsNotNull(products, "La lista de productos no debería ser nula.");
            Assert.IsTrue(products.Count > 0, "Debería haber al menos un producto que coincida con el nombre especificado.");
        }

        [TestMethod]
        public void SearchProductByName_NoMatches_ReturnsEmptyList()
        {
            string searchText = "Inexistence product name";           
            List<Product> products = _productController.SearchProductByName(searchText);

            Assert.AreEqual(0, products.Count, "La lista de productos debería estar vacía cuando no hay coincidencias en la búsqueda.");
        }

        [TestMethod]
        public void SearchProductByType_ValidSearch_ReturnProducts()
        {
            var product1 = new Product
            {
                name = "Pepsi 1L",
                productCode = "PPS002",
                price = 25,
                description = "Product for search product by type test",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 25,
            };

            var supply1 = new Supply
            {
                name = product1.name,
                amount = product1.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product1.productCode,
                status = true
            };

            var product2 = new Product
            {
                name = "Sprite 2L",
                productCode = "SPT003",
                price = 40,
                description = "Product for search product by type test",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.EXTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 25,
            };

            var supply2 = new Supply
            {
                name = product2.name,
                amount = product2.amount,
                category = Constants.EXTERNAL_PRODUCT_SUPPLY_AREA_ID,
                measurementUnit = "Unidad",
                productCode = product2.productCode,
                status = true
            };

            _productController.AddProductExternal(product1, supply1);
            _productController.AddProductExternal(product2, supply2);

            List<Product> products = _productController.SearchProductByType(Constants.EXTERNAL_PRODUCT);
            Assert.IsNotNull(products, "La lista de productos no debería ser nula.");
            Assert.IsTrue(products.Count > 0, "Debería haber al menos un producto que coincida con el tipo de producto especificada.");
            Assert.IsTrue(products.Exists(p => p.name == "Pepsi 1L"), "La lista de productos debería incluir 'Pepsi 1L'.");
            Assert.IsTrue(products.Exists(p => p.name == "Sprite 2L"), "La lista de productos debería incluir 'Sprite 2L'.");
        }

        [TestMethod]
        public void SearchProductByType_NoMatches_RwturnsEmptyList()
        {
            var product = new Product
            {
                name = "Jarritos sabor fresa 1L",
                productCode = "JRRT002",
                price = 25,
                description = "Product for search product by type test",
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

            _productController.AddProductExternal(product, supply);

            List<Product> products = _productController.SearchProductByType(Constants.INTERNAL_PRODUCT);
            Assert.AreEqual(0, products.Count, "La lista de productos debería estar vacía cuando no hay coincidencias en la búsqueda por tipo.");
        }
    }
}

