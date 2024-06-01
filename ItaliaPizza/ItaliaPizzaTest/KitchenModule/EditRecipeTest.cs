using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.ProductsModule
{
    [TestClass]
    public class EditRecipeTest
    {
        private readonly RecipeController _recipeController = new RecipeController();
        private readonly SupplyController _supplyController = new SupplyController();

        [TestMethod]
        public void EditRecipe_ValidData_Success()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe modify test success" + i,
                    amount = i + 1,
                    measurementUnit = "Kilogramo",
                    status = true,
                    category = Constants.VEGETABLES_SUPPLY_AREA_ID
                };

                supplies.Add(supply);

                // Register supply
                bool resultInitial = _supplyController.AddSupply(supply);
                Assert.IsTrue(resultInitial, "El registro del insumo debería ser exitoso.");
            }

            //Convert to recipe supplies
            List<RecipeSupply> recipeSupplies = new List<RecipeSupply>();

            decimal initialAmount = 0.100m;

            foreach (var supply in supplies)
            {
                RecipeSupply recipeSupply = new RecipeSupply();
                recipeSupply.Supply = supply;
                recipeSupply.supplyAmount = initialAmount;

                recipeSupplies.Add(recipeSupply);

                initialAmount += 0.100m;
            }

            var product = new Product
            {
                name = "Pizza Vegetariana",
                productCode = "PV001",
                price = 130,
                description = "Pizza Vegetariana con salsa de tomate, mozzarella, pimientos, champiñones, cebolla, aceitunas y maíz. " +
                              "Una opción saludable y deliciosa para aquellos que prefieren evitar la carne.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe with valid data",
                status = Constants.ACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };
            bool registerResult = _recipeController.AddRecipeWithSupplies(recipe, product);
            Assert.IsTrue(registerResult, "El registro inicial de la receta debería ser exitoso con datos válidos.");

            recipe.description = "recipe description changed";

            bool editResult = false;

            if (_recipeController.EditRecipe(recipe))
            {
                editResult = true;
            }

            Assert.IsTrue(editResult, "La modificación de la receta debería ser exitosa con datos válidos");
        }

        [TestMethod]
        public void DesactivateRecipe_Success()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe desactivate test success" + i,
                    amount = i + 1,
                    measurementUnit = "Kilogramo",
                    status = true,
                    category = Constants.VEGETABLES_SUPPLY_AREA_ID
                };

                supplies.Add(supply);

                // Register supply
                bool resultInitial = _supplyController.AddSupply(supply);
                Assert.IsTrue(resultInitial, "El registro del insumo debería ser exitoso.");
            }

            //Convert to recipe supplies
            List<RecipeSupply> recipeSupplies = new List<RecipeSupply>();

            decimal initialAmount = 0.100m;

            foreach (var supply in supplies)
            {
                RecipeSupply recipeSupply = new RecipeSupply();
                recipeSupply.Supply = supply;
                recipeSupply.supplyAmount = initialAmount;

                recipeSupplies.Add(recipeSupply);

                initialAmount += 0.100m;
            }

            var product = new Product
            {
                name = "Pizza Pepperoni",
                productCode = "PP002",
                price = 130,
                description = "Pizza clásica de Pepperoni con salsa de tomate, mozzarella y generosas porciones de pepperoni. " +
                              "Perfecta para los amantes de la carne.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe to desactivate",
                status = Constants.ACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };
            bool registerResult = _recipeController.AddRecipeWithSupplies(recipe, product);
            Assert.IsTrue(registerResult, "El registro inicial de la receta debería ser exitoso con datos válidos.");

            recipe.recipeCode = _recipeController.GetRecipeIDByName(recipe.name);

            bool desactivateResult = _recipeController.ChangeStatus(recipe, Constants.INACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La desactivación de la receta debería ser exitosa.");
        }

        [TestMethod]
        public void ActivateRecipe_Success()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe activate test success" + i,
                    amount = i + 1,
                    measurementUnit = "Kilogramo",
                    status = true,
                    category = Constants.VEGETABLES_SUPPLY_AREA_ID
                };

                supplies.Add(supply);

                // Register supply
                bool resultInitial = _supplyController.AddSupply(supply);
                Assert.IsTrue(resultInitial, "El registro del insumo debería ser exitoso.");
            }

            //Convert to recipe supplies
            List<RecipeSupply> recipeSupplies = new List<RecipeSupply>();

            decimal initialAmount = 0.100m;

            foreach (var supply in supplies)
            {
                RecipeSupply recipeSupply = new RecipeSupply();
                recipeSupply.Supply = supply;
                recipeSupply.supplyAmount = initialAmount;

                recipeSupplies.Add(recipeSupply);

                initialAmount += 0.100m;
            }

            var product = new Product
            {
                name = "Pizza Carbonara",
                productCode = "PC008",
                price = 170,
                description = "Pizza Carbonara con salsa blanca, mozzarella, panceta, cebolla y huevo. " +
                              "Una delicia cremosa inspirada en la clásica receta italiana.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.INACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe to activate",
                status = Constants.INACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };
            bool registerResult = _recipeController.AddRecipeWithSupplies(recipe, product);
            Assert.IsTrue(registerResult, "El registro inicial de la receta debería ser exitoso con datos válidos.");

            recipe.recipeCode = _recipeController.GetRecipeIDByName(recipe.name);

            bool desactivateResult = _recipeController.ChangeStatus(recipe, Constants.ACTIVE_STATUS);
            Assert.IsTrue(desactivateResult, "La activación de la receta debería ser exitosa.");
        }

    }
}
