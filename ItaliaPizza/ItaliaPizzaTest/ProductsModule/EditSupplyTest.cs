using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.Utilities;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItaliaPizzaTest.ProductsModule
{
    /// <summary>
    /// Descripción resumida de EditSupplyTest
    /// </summary>
    [TestClass]
    public class EditSupplyTest
    {
        private readonly SupplyController _supplyController = new SupplyController();

        [TestMethod]
        public void EditSupply_ValidData_Success()
        {
            var supply = new Supply
            {
                name = "Queso Cheddar",
                amount = 3,
                measurementUnit = "Kilogramo",
                status = true,
                category = ItaliaPizza.ApplicationLayer.Constants.VEGETABLES_SUPPLY_AREA_ID
            };

            // Register supply first
            bool initialResult = _supplyController.AddSupply(supply);
            Assert.IsTrue(initialResult, "El registro inicial del insumo debería ser exitoso.");

            // Simulate edit
            supply.category = ItaliaPizza.ApplicationLayer.Constants.DAIRY_PRODUCT_SUPPLY_AREA_ID;

            bool editResult = false;

            if (_supplyController.ModifySupply(supply, supply.name))
            {
                editResult = true;
            }

            Assert.IsTrue(editResult, "La modificación del insumo debería ser exitosa con datos válidos");
        }

        [TestMethod]
        public void DesactivateSupply_Success()
        {
            var supply = new Supply
            {
                name = "Queso Parmesano",
                amount = 5,
                measurementUnit = "Kilogramo",
                status = true,
                category = ItaliaPizza.ApplicationLayer.Constants.DAIRY_PRODUCT_SUPPLY_AREA_ID
            };

            // Register supply first
            bool initialResult = _supplyController.AddSupply(supply);
            Assert.IsTrue(initialResult, "El registro inicial del insumo debería ser exitoso.");

            bool desactivateResult = _supplyController.ChangeSupplyStatus(supply, ItaliaPizza.ApplicationLayer.Constants.INACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación del insumo debería ser exitosa.");
        }

        [TestMethod]
        public void ActivateSupply_Success()
        {
            var supply = new Supply
            {
                name = "Queso Mozzarella",
                amount = 5,
                measurementUnit = "Kilogramo",
                status = false,
                category = ItaliaPizza.ApplicationLayer.Constants.DAIRY_PRODUCT_SUPPLY_AREA_ID
            };

            // Register supply first
            bool initialResult = _supplyController.AddSupply(supply);
            Assert.IsTrue(initialResult, "El registro inicial del insumo debería ser exitoso.");

            bool activateResult = _supplyController.ChangeSupplyStatus(supply, ItaliaPizza.ApplicationLayer.Constants.ACTIVE_STATUS);
            Assert.IsTrue(activateResult, "La activación del insumo debería ser exitosa.");
        }
    }

}
