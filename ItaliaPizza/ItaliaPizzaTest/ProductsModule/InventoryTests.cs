using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.Models;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class InventoryTests
    {
        InventoryController _controller = new InventoryController();
        ProductDAO _productDAO = new ProductDAO();
        SupplyDAO _supplyDAO = new SupplyDAO();

        [TestMethod]
        public void TestGetActiveSupplies()
        {
            List<Supply> activeSupplies = _controller.GetActiveSupplies();
            Assert.IsNotNull(activeSupplies, "The active supplies list should not be null.");
            Assert.IsTrue(activeSupplies.Count > 0, "There should be at least one active supply.");
        }

        [TestMethod]
        public void TestUpdateProductAmount()
        {
            // Step 1: Prepare test data
            decimal newAmount = 50m;

            // Step 2: Get active supplies
            List<Supply> activeSupplies = _controller.GetActiveSupplies();

            // Ensure there is at least one active supply for testing
            Assert.IsNotNull(activeSupplies, "Active supplies should not be null.");
            Assert.IsTrue(activeSupplies.Count > 0, "There should be at least one active supply.");

            // Step 3: Update the first active supply
            Supply firstSupply = activeSupplies[0];
            _controller.UpdateProductAmount(firstSupply.name, firstSupply.productCode, newAmount);

            // Step 4: Validate the supply amount is updated
            List<Supply> updatedSupplyList = _supplyDAO.SearchSupplyByName(firstSupply.name);
            Supply updatedSupply = updatedSupplyList[0];
            Assert.IsNotNull(updatedSupply, "The supply should not be null after update.");
            Assert.AreEqual(newAmount, updatedSupply.amount, "The supply amount should be updated.");

            // Step 5: Validate the product amount is updated
            Product updatedProduct = _productDAO.GetProductByCode(firstSupply.productCode);
            Assert.IsNotNull(updatedProduct, "The product should not be null after update.");
            Assert.AreEqual(Convert.ToInt32(newAmount), updatedProduct.amount, "The product amount should be updated.");
        }


    }
}
