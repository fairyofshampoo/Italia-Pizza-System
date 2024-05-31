using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class RecipeController
    {
        public bool AddRecipeWithSupplies(Recipe recipe, Product product)
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            return recipeDAO.RegisterRecipeWithSupplies(recipe, product);
        }

        public bool EditRecipe(Recipe recipe)
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            return recipeDAO.EditRecipe(recipe);
        }

        public bool ModifyRecipeSupplies(int recipeID, List<RecipeSupply> recipeSupplyList)
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            return recipeDAO.RegisterRecipeSupplies(recipeID, recipeSupplyList);
        }

        public bool IsRecipeDuplicated(string recipeName)
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            return recipeDAO.AlreadyExistRecipe(recipeName);
        }

        public bool ValidateSuppliesActives(List<RecipeSupply> recipeSupplyList)
        {
            bool isActive = true;

            foreach (var supply in recipeSupplyList)
            {
                if (supply.Supply.status != true)
                {
                    isActive = false;
                }
            }
            return isActive;
        }
    }
}
