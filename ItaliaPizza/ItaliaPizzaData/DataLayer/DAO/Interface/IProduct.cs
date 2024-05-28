using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface IProduct
    {
        bool IsCodeExisting(string code);
        bool AddProduct(Product product);
        bool AddProductExternal(Product product, Supply supply);
        List<Product> GetLastProductsRegistered();
        List<Product> SearchProductByName(string name);
        List<Product> SearchProductByType(int type);
        bool ModifyProduct(Product product, string code);
        bool ChangeStatus(Product product, int newStatus);
        Product GetProductByCode(string code);
        List<Product> GetAllAvailableProducts();
        bool UpdateProductAmount(string productCode, int newAmount);
    }
}
