using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class RegisterSupplyTest
    {
        private readonly SupplyController _supplyController = new SupplyController();

        [TestMethod]
        public void AddSupply_ValidData_Success()
        {
            var supply = new Supply
            {
                name = "Pimiento verde",
                amount = 3,
                measurementUnit = "Kilogramo",
                status = true,
                category = Constants.VEGETABLES_SUPPLY_AREA_ID
            };

            bool result = false;

            if (_supplyController.IsNewSupplyValid(supply) && 
                !_supplyController.IsSupplyNameDuplicated(supply.name))
            {
                if (_supplyController.AddSupply(supply))
                {
                    result = true;
                }
            }

            Assert.IsTrue(result, "El registro del insumo debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddSupply_InvalidData_Failure()
        {
            var invalidSupply = new Supply
            {
                name = "Pimiento rojo", // Invalid supply name
                amount = 0, // Invalid supply amount 
                measurementUnit = "", //Invalid supply measurement unit
                status = true,
                category = Constants.VEGETABLES_SUPPLY_AREA_ID
            };

            bool result = false;

            if (_supplyController.IsNewSupplyValid(invalidSupply) &&
                !_supplyController.IsSupplyNameDuplicated(invalidSupply.name))
            {
                if (_supplyController.AddSupply(invalidSupply))
                {
                    result = true;
                }
            }

            Assert.IsTrue(result, "El registro del insumo no debería ser exitoso con datos inválidos.");
        }

        [TestMethod]
        public void AddSupply_DuplicatedSupply_Failure()
        {
            var supply = new Supply
            {
                name = "Aceituna negra",
                amount = 2,
                measurementUnit = "Kilogramo",
                status = true,
                category = Constants.VEGETABLES_SUPPLY_AREA_ID
            };

            bool result = false;

            if (_supplyController.IsNewSupplyValid(supply) &&
                !_supplyController.IsSupplyNameDuplicated(supply.name))
            {
                if (_supplyController.AddSupply(supply))
                {
                    result = true;
                }
            }

            Assert.IsFalse(result, "El registro del insumo no debería ser exitoso si el insumo ya existe.");
        }
    }
}
