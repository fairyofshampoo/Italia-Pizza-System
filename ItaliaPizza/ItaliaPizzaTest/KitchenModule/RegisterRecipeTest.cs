using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer.DAO.Interface;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.KitchenModule
{
    [TestClass]
    public class RegisterRecipeTest
    {
        private readonly RecipeController _recipeController = new RecipeController();
        private readonly SupplyController _supplyController = new SupplyController();

        [TestMethod]
        public void AddRecipe_ValidData_Success()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe test success" + i,
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
                name = "Pizza Margherita",
                productCode = "PM001",
                price = 150,
                description = "Pizza Margherita con salsa de tomate, mozzarella fresca, albahaca y un toque de aceite de oliva. " +
                              "Una opción clásica y deliciosa, perfecta para quienes prefieren sabores simples y auténticos.",
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

            bool registerResult = false;

            if (!_recipeController.IsRecipeDuplicated(recipe.name) && 
                _recipeController.ValidateSuppliesActives((List<RecipeSupply>)recipe.RecipeSupplies))
            {
                if (_recipeController.AddRecipeWithSupplies(recipe, product))
                {
                    registerResult = true; 
                }
            }

            Assert.IsTrue(registerResult, "El registro de la receta debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddRecipe_InvalidData_Failure()
        {
            var supplies = new List<Supply>();

            // Create supplies desactivates
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe test fail" + i,
                    amount = i + 1,
                    measurementUnit = "Kilogramo",
                    status = false,
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
                name = "Pizza Cuatro Quesos",
                productCode = "PQ001",
                price = 160,
                description = "Pizza Cuatro Quesos con una mezcla de mozzarella, cheddar, parmesano y gorgonzola. " +
                              "Una opción rica y cremosa, ideal para los amantes del queso.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe with invalid data",
                status = Constants.ACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };

            bool registerResult = false;

            if (!_recipeController.IsRecipeDuplicated(recipe.name) &&
                _recipeController.ValidateSuppliesActives((List<RecipeSupply>)recipe.RecipeSupplies))
            {
                if (_recipeController.AddRecipeWithSupplies(recipe, product))
                {
                    registerResult = true;
                }
            }

            Assert.IsFalse(registerResult, "El registro de la receta no debería ser exitoso con datos válidos.");
        }

        [TestMethod]
        public void AddRecipe_DuplicatedRecipe_Success()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe duplicated test" + i,
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
                name = "Pizza Margherita",
                productCode = "PM001",
                price = 150,
                description = "Pizza Margherita con salsa de tomate, mozzarella fresca, albahaca y un toque de aceite de oliva. " +
                              "Una opción clásica y deliciosa, perfecta para quienes prefieren sabores simples y auténticos.",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe duplicated",
                status = Constants.ACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };

            bool registerResult = false;

            if (!_recipeController.IsRecipeDuplicated(recipe.name) &&
                _recipeController.ValidateSuppliesActives((List<RecipeSupply>)recipe.RecipeSupplies))
            {
                if (_recipeController.AddRecipeWithSupplies(recipe, product))
                {
                    registerResult = true;
                }
            }

            Assert.IsFalse(registerResult, "El registro de la receta no debería ser exitoso si la receta ya existe.");
        }


    }
}
