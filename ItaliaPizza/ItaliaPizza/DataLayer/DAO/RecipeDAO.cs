using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.UserInterfaceLayer.KitchenModule;
using System.Windows;

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

        public bool ChangeStatus(Recipe recipe, int status)
        {
            bool successfulChange = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifyRecipe = databaseContext.Recipes.First(r => r.recipeCode == recipe.recipeCode);
                    if (modifyRecipe != null)
                    {
                        modifyRecipe.status = Convert.ToByte(status);
                    }

                    var modifyProductStatus = databaseContext.Products.First(p => p.productCode == recipe.ProductId);
                    if (modifyProductStatus != null)
                    {
                        modifyProductStatus.status = Convert.ToByte(status);
                    }

                    databaseContext.SaveChanges();
                    successfulChange = true;

                }
                catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }

            return successfulChange;
        }

        public bool DeleteRecipeSupplies(int idRecipe)
        {
            bool successfulChange = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var foundRecipeIngredients = databaseContext.RecipeSupplies.Where(x => x.recipeID.Equals(idRecipe));
                    if (foundRecipeIngredients.FirstOrDefault() != null)
                    {
                        databaseContext.RecipeSupplies.RemoveRange(foundRecipeIngredients);
                        databaseContext.SaveChanges();
                        successfulChange = true;
                    }
                }
                catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }
            return successfulChange;
        }


        public bool EditRecipe(Recipe recipe)
        {
            bool successfulChange = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var foundRecipe = databaseContext.Recipes.FirstOrDefault(r => r.recipeCode == recipe.recipeCode);

                    if (foundRecipe != null)
                    {
                        foundRecipe.description = recipe.description;
                    }

                    databaseContext.SaveChanges();
                    successfulChange = true;
                }
                catch (SqlException ex)
                {
                    throw ex;
                }
            }
            return successfulChange;
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
      
        public bool RegisterRecipeSupplies(int recipeId, List<RecipeSupply> recipeSupplies)
        {
            bool response = false;
            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var recipe = databaseContext.Recipes
                     .Include(r => r.RecipeSupplies)
                     .FirstOrDefault(r => r.recipeCode == recipeId);

                    foreach (var supply in recipeSupplies)
                    {
                        var recipeSupply = new RecipeSupply
                        {
                            recipeID = recipe.recipeCode,
                            supplyId = supply.Supply.name,
                            supplyAmount = supply.supplyAmount,
                        };
                        databaseContext.RecipeSupplies.Add(recipeSupply);
                    }
                    databaseContext.SaveChanges();
                    response = true;
                }
            }
            catch (SqlException ex)
            {
                throw ex;
            }
            return response;
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
