using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IProduct
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
        List<Product> GetAllProducts();
        List<Product> GetAllExternalProducts();
        bool UpdateProductAmount(string productCode, int newAmount);
    }
}
