using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface IRecipe
    {
        bool AlreadyExistRecipe(string name);
        bool RegisterRecipe(Recipe recipe, string productId);
        int GetIdRecipe(string name);
        bool RegisterRecipeSupplies(int recipeId, List<RecipeSupply> recipeSupplies);
        List<Recipe> GetRecipes();
        bool ChangeStatus(Recipe recipe, int status);
        bool EditRecipe(Recipe recipe);
        bool DeleteRecipeSupplies(int idRecipe);
    }
}
