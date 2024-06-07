using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ItaliaPizzaTest.KitchenModule
{
    [TestClass]
    public class SearchRecipeTest
    {
        private readonly RecipeController _recipeController = new RecipeController();
        private readonly SupplyController _supplyController = new SupplyController();

        [TestMethod]
        public void SearchRecipes_ReturnRecipes()
        {
            var supplies = new List<Supply>();

            // Create supplies
            for (int i = 0; i < 5; i++)
            {
                var supply = new Supply
                {
                    name = "Supply for recipe search test success" + i,
                    amount = i + 1,
                    measurementUnit = "Kilogramo",
                    status = true,
                    category = Constants.VEGETABLES_SUPPLY_AREA_ID
                };

                supplies.Add(supply);

                _supplyController.AddSupply(supply);
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
                name = "Pizza Tocino",
                productCode = "PTCN001",
                price = 250,
                description = "Product for search recipe test",
                picture = new byte[] { 0x12, 0x34, 0x56 },
                isExternal = Constants.INTERNAL_PRODUCT,
                status = Constants.ACTIVE_STATUS,
                amount = 1,
            };

            var recipe = new Recipe
            {
                description = "recipe to search search",
                status = Constants.ACTIVE_STATUS,
                name = product.name,
                ProductId = product.productCode,
                RecipeSupplies = recipeSupplies,
            };

            _recipeController.AddRecipeWithSupplies(recipe, product);
            List<Recipe> recipes = _recipeController.GetRecipes();

            Assert.IsNotNull(recipes, "La lista de recetas no debería ser nula.");
            Assert.IsTrue(recipes.Count > 0, "Debería haber al menos una receta.");
        }
    }
}
