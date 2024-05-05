using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IRecipe
    {
        bool AlreadyExistRecipe(string name);
        bool RegisterRecipe(Recipe recipe, string productId);
        int GetIdRecipe(string name);
        bool RegisterRecipeSupplies(Recipe recipe);
        List<Recipe> GetRecipes();
    }
}
