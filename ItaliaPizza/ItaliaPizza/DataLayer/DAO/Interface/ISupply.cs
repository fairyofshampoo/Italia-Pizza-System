using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface ISupply
    {
        bool IsSupplyNameExisting(string name);
        bool AddSupply(Supply supply);
        bool ChangeSupplyStatus(string name, int status);
        bool ModifySupply(Supply supply, string name);
        List<Supply> GetSuppliesByStatus(bool status);
        List<Supply> GetRecipeSupplies(int idRecipe);
        List<Supply> SearchSupplyByName(string name);
        List<Supply> SearchActiveSupplyByName(string name);
        List<Supply> GetAllSupplies();
        bool ExistsSupplyInRecipe(string supplyName);
    }
}
