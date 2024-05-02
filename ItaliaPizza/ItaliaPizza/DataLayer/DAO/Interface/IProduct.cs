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
        List<Product> GetLastProductsRegistered();
        List<Product> SearchProductByName(string name);
        List<Product> SearchProductByType(int type);
        bool ModifyProduct(Product product, string code);
        bool ChangeStatus(string code, int newStatus);
        Product GetProductByCode(string code);
        List<Product> GetAllProducts();
    }
}
