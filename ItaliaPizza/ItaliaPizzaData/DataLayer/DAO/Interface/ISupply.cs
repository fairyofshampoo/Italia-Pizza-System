using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface ISupply
    {
        bool IsSupplyNameExisting(string name);
        bool AddSupply(Supply supply);
        bool ChangeSupplyStatus(Supply supply, int status);
        bool ModifySupply(Supply supply, string name);
        List<Supply> GetSuppliesByStatus(bool status);
        List<Supply> GetRecipeSupplies(int idRecipe);
        List<object> SearchSupplyOrExternalProductByName(string name);
        List<Supply> SearchActiveSupplyByName(string name);
        bool ExistsSupplyInRecipe(string supplyName);
        List<Supply> GetAllSupplies();
    }
}
