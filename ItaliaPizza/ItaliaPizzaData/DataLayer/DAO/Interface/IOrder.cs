using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface IOrder
    {
        List<InternalOrder> GetInternalOrdersByStatusAndWaiter(int status, string waiterEmail);
        InternalOrder GetInternalOrdersByNumber(string numberOrder, string waiterEmail);
        List<RecipeSupply> GetSupplyForProduct(string productId);
        List<Supply> GetInventoryQuantitiesForIngredients(List<string> recipeSupplyList);
        bool AddInternalOrderProduct(InternalOrderProduct internalOrderProduct);
        bool AddOrder(InternalOrder order);
        bool IsInternalOrderCodeAlreadyExisting(string internalOrderCode);
        int GetMaximumProductsPosible(int recipeId); //Deberá estar en una interface llamada solo order
        int GetRecipeIdByProduct(string productId); //Deberá ir en la interface de Recipe
        int GetNumberOfProductsOnHold(string productId);
        bool GetCounterOfProduct(string productId);
        bool IsRegisterInDatabase(string productId, string internalOrderCode);
        bool IncreaseAmount(string productId, string internalOrderCode);
        bool CancelOrder(string internalOrderCode);
        int SaveInternalOrder(string internalOrderCode);
        int GetTotalExternalProduct(string productId);
        List<InternalOrderProduct> GetAllInternalProductsByOrden(string internalOrderCode);
        List<InternalOrder> GetInternalOrdersByStatus(int status);
        string GetProductName(string productId);
        bool ChangeOrderStatus(int status, string internalOrderCode);
    }
}
