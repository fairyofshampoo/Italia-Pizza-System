using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

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

        public List<Product> GetLastProductsRegistered()
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

        public bool ModifyProduct(Product updateProduct, string code)
        {
            bool successfulUpdate = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifyProduct = databaseContext.Products.First(c => c.productCode == code);

                    if (modifyProduct != null)
                    {
                        modifyProduct.amount = updateProduct.amount;
                        modifyProduct.price = updateProduct.price;
                        modifyProduct.description = updateProduct.description;
                        modifyProduct.name = updateProduct.name;
                        modifyProduct.picture = updateProduct.picture;
                    }

                    databaseContext.SaveChanges();
                    successfulUpdate = true;
                }
                catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }

            return successfulUpdate;
        }

        public bool ChangeStatus(string code, int newStatus)
        {
            bool successfulChange = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifyProduct = databaseContext.Products.First(a => a.productCode == code);
                    if (modifyProduct != null)
                    {
                        modifyProduct.status = Convert.ToByte(newStatus);
                    }

                    databaseContext.SaveChanges();
                    successfulChange = true;

                }
                catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }

            return successfulChange;
        }

        public Product GetProductByCode(string code)
        {
            Product productFound = new Product();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    Product product = databaseContext.Products.Find(code);

                    if (product != null)
                    {
                        productFound.productCode = product.productCode;
                        productFound.status = product.status;
                        productFound.amount = product.amount;
                        productFound.description = product.description;
                        productFound.price = product.price;
                        productFound.isExternal = product.isExternal;
                        productFound.name = product.name;
                        productFound.picture = product.picture;
                    }

                    databaseContext.SaveChanges();
                }
            }
            catch (ArgumentException argumentException)
            {
                throw argumentException;
            }
            return productFound;
        }

        public List<Product> GetAllProducts()
        {
            List<Product> products = new List<Product>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var productsDB = databaseContext.Products
                                                .ToList();

                if(productsDB != null)
                {
                    foreach (var product in productsDB)
                    {
                        products.Add(product);
                    }
                }
            }
            return products;
        }
    }
}
