using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class ProductDAO : IProduct
    {
        public bool IsCodeExisting(string code)
        {
            bool isCodeExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingCode = databaseContext.Products.FirstOrDefault(c => c.productCode == code);
                if (existingCode != null)
                {
                    isCodeExisting = true;
                }
            }
            return isCodeExisting;
        }
    }
}
