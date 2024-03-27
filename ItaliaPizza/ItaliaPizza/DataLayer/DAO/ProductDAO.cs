using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

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

        public bool AddProduct(Product product)
        {
            bool successfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newProduct = new Product
                    {
                        productCode = product.productCode,
                        status = product.status,
                        amount = product.amount,
                        description = product.description,
                        isExternal = product.isExternal,
                        name = product.name,
                        price = product.price,
                        picture = product.picture,
                    };

                    databaseContext.Products.Add(newProduct);
                    databaseContext.SaveChanges();

                    successfulRegistration = true;
                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public List<Product> GetLastProductsRegisteres()
        {
            List<Product> lastProducts = new List<Product>();
            using (var databseContext = new ItaliaPizzaDBEntities())
            {
                var lastProductsRegistered = databseContext.Products
                                                           .OrderByDescending(product => product.name)
                                                           .Take(10)
                                                           .ToList();
                if (lastProductsRegistered != null)
                {
                    foreach (var product in lastProductsRegistered)
                    {
                        lastProducts.Add(product);
                    }
                }
            }
            return lastProducts;
        }

        public List<Product> SearchProductByName(string name)
        {
            List<Product> productsDB = new List<Product>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var products = databaseContext.Products
                                              .Where(p => p.name.StartsWith(name))
                                              .Take(5)
                                              .ToList();
                if (products != null)
                {
                    foreach(var product in products)
                    {
                        productsDB.Add(product);
                    }
                }
            }
            return productsDB;
        }

        public List<Product> SearchProductByType(int type)
        {
            List<Product> productsDB = new List<Product>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var products = databaseContext.Products
                                              .Where(p => p.isExternal == type)
                                              .ToList();
                if (products != null)
                {
                    foreach (var product in products)
                    {
                        productsDB.Add(product);
                    }
                }
            }
            return productsDB;
        }
    }
}
