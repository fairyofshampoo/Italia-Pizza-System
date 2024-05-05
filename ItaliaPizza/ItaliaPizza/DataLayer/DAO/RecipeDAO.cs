using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class RecipeDAO : IRecipe
    {
        public bool AlreadyExistRecipe(string name)
        {
            bool alreadyExistRecipe = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingRecipe = databaseContext.Recipes.FirstOrDefault(r => r.name == name);
                if (existingRecipe != null)
                {
                    alreadyExistRecipe = true;
                }
            }
            return alreadyExistRecipe;
        }

        public int GetIdRecipe(string name)
        {
            int idRecipe = 0;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var foundRecipe = (from recipe in databaseContext.Recipes
                                   where recipe.name.Equals(name)
                                   select recipe).FirstOrDefault();
                if (foundRecipe != null)
                {
                    idRecipe = foundRecipe.recipeCode;
                }
            }
            return idRecipe;
        }

        public List<Recipe> GetRecipes()
        {
            List<Recipe> recipes = new List<Recipe>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var recipesDB = databaseContext.Recipes
                                                .ToList();

                if (recipesDB != null)
                {
                    foreach (var product in recipesDB)
                    {
                        recipes.Add(product);
                    }
                }
            }
            return recipes;
        }

        public bool RegisterRecipe(Recipe recipe, string productId)
        {
            bool successfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newRecipe = new Recipe
                    {
                        description = recipe.description,
                        status = recipe.status,
                        name = recipe.name,
                        ProductId = productId,
                    };
                    databaseContext.Recipes.Add(newRecipe);
                    databaseContext.SaveChanges();
                    successfulRegistration = true;
                }
                catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public bool RegisterRecipeSupplies(Recipe recipe)
        {
            bool successfulRegistration = false;
            List<RecipeSupply> supplies = new List<RecipeSupply>();

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    foreach (var supply in recipe.RecipeSupplies)
                    {
                        var recipeSupply = new RecipeSupply
                        {
                            recipeID = recipe.recipeCode,
                            supplyId = supply.Supply.name,
                            supplyAmount = supply.supplyAmount,
                        };
                        supplies.Add(recipeSupply);
                    }
                    databaseContext.RecipeSupplies.AddRange(supplies);
                    databaseContext.SaveChanges();
                    successfulRegistration = true;
                }
                catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public bool RegisterRecipeWithSupplies(Recipe recipe, Product product) //MÉTODO CON ROLLBACK
        {
            bool successfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                using (var transaction = databaseContext.Database.BeginTransaction())
                {
                    try
                    {
                        ProductDAO productDAO = new ProductDAO();
                        productDAO.AddProduct(product);

                        // Registrar la receta
                        var newRecipe = new Recipe
                        {
                            description = recipe.description,
                            status = recipe.status,
                            name = recipe.name,
                            ProductId = product.productCode,
                        };
                        databaseContext.Recipes.Add(newRecipe);
                        databaseContext.SaveChanges();

                        // Registrar los suministros de la receta
                        foreach (var supply in recipe.RecipeSupplies)
                        {
                            var recipeSupply = new RecipeSupply
                            {
                                recipeID = newRecipe.recipeCode,
                                supplyId = supply.Supply.name,
                                supplyAmount = supply.supplyAmount,
                            };
                            databaseContext.RecipeSupplies.Add(recipeSupply);
                        }
                        databaseContext.SaveChanges();

                        // Commit de la transacción si todo se realizó correctamente
                        transaction.Commit();
                        successfulRegistration = true;

                    }
                    catch (SqlException ex)
                    {
                        // Rollback de la transacción en caso de error
                        transaction.Rollback();
                        // Manejar la excepción o relanzarla según sea necesario
                        throw ex;
                    }
                }
            }
            return successfulRegistration;
        }

    }
}
