using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class InventoryController
    {
        SupplyDAO supplyDAO = new SupplyDAO();

        ProductDAO productDAO = new ProductDAO();
        public List<Supply> GetActiveSupplies()
        {
            return supplyDAO.GetSuppliesByStatus(true);
        }

        public void UpdateProductAmount(string supplyName, string productCode, decimal amount)
        {
            supplyDAO.ModifySupplyAmount(supplyName, amount);
            productDAO.UpdateProductAmount(productCode, Convert.ToInt32(amount));
        }
    }
}
