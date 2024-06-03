﻿using ItaliaPizzaData.DataLayer.DAO.Interface;
using ItaliaPizzaData.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using ItaliaPizzaData.Utilities;

namespace ItaliaPizzaData.DataLayer.DAO
{
    public class ProductDAO : IProduct
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

        public bool AddProductExternal(Product product, Supply supply)
        {
            bool successfulRegistration = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                using (var transaction = databaseContext.Database.BeginTransaction())
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

                        var newSupply = new Supply
                        {
                            name = supply.name,
                            amount = supply.amount,
                            category = supply.category,
                            measurementUnit = supply.measurementUnit,
                            status = supply.status,
                            productCode = product.productCode,
                        };

                        databaseContext.Supplies.Add(newSupply);
                        databaseContext.SaveChanges();

                        transaction.Commit();
                        successfulRegistration = true;
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw ex;
                    }
                }
            }

            return successfulRegistration;
        }

        public List<Product> GetLastProductsRegistered()
        {
            List<Product> lastProducts = new List<Product>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var lastProductsRegistered = databaseContext.Products
                                            .OrderBy(product => product.name)
                                            .Take(10)
                                            .Select(product => new ProductDTO
                                            {
                                                Name = product.name,
                                                Status = product.status,
                                                ProductCode = product.productCode,
                                                Price = product.price
                                            })
                                            .ToList();

                if (lastProductsRegistered != null)
                {
                    foreach (var dto in lastProductsRegistered)
                    {
                        lastProducts.Add(new Product
                        {
                            name = dto.Name,
                            status = (byte)dto.Status,
                            productCode = dto.ProductCode,
                            price = dto.Price
                        });
                    }
                }
            }
            return lastProducts;
        }

        public List<Product> SearchProductByName(string name)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var products = databaseContext.Products
                                              .Where(p => p.name.StartsWith(name))
                                              .Take(5)
                                              .Select(p => new ProductDTO
                                              {
                                                  Name = p.name,
                                                  Status = p.status,
                                                  ProductCode = p.productCode,
                                                  Price = p.price
                                              })
                                              .ToList();

                return products.Select(dto => new Product
                {
                    name = dto.Name,
                    status = (byte)dto.Status,
                    productCode = dto.ProductCode,
                    price = dto.Price
                }).ToList();
            }
        }

        public List<Product> SearchProductByType(int type)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var products = databaseContext.Products
                                              .Where(p => p.isExternal == type)
                                              .Select(p => new ProductDTO
                                              {
                                                  Name = p.name,
                                                  Status = p.status,
                                                  ProductCode = p.productCode,
                                                  Price = p.price
                                              })
                                              .ToList();

                return products.Select(dto => new Product
                {
                    name = dto.Name,
                    status = (byte)dto.Status,
                    productCode = dto.ProductCode,
                    price = dto.Price
                }).ToList();
            }
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

        public bool ChangeStatus(Product product, int newStatus)
        {
            bool successfulChange = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                using (var transaction = databaseContext.Database.BeginTransaction())
                {
                    try
                    {
                        var modifyProduct = databaseContext.Products.First(a => a.productCode == product.productCode);
                        if (modifyProduct != null)
                        {
                            modifyProduct.status = Convert.ToByte(newStatus);
                        }
                        databaseContext.SaveChanges();

                        if (product.isExternal == Constants.EXTERNAL_PRODUCT)
                        {
                            var modifySupply = databaseContext.Supplies.First(s => s.productCode == product.productCode);
                            if (modifySupply != null)
                            {
                                if (newStatus == Constants.INACTIVE_STATUS)
                                {
                                    modifySupply.status = false;
                                }
                                else
                                {
                                    modifySupply.status = true;
                                }
                            }
                            databaseContext.SaveChanges();
                        }
                        transaction.Commit();
                        successfulChange = true;
                    }
                    catch (ArgumentException argumentException)
                    {
                        transaction.Rollback();
                        throw argumentException;
                    }
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

        public List<Product> GetAllAvailableProducts()
        {
            List<Product> products = new List<Product>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var productsDB = databaseContext.Products
                                                .Where(p => p.status == Constants.ACTIVE_STATUS)
                                                .Select(p => new ProductDTO
                                                {
                                                    ProductCode = p.productCode,
                                                    Name = p.name,
                                                    Price = p.price,
                                                    Description = p.description,
                                                    IsExternal = p.isExternal,
                                                    Status = p.status,
                                                })
                                                .ToList();

                if (productsDB != null)
                {
                    products = productsDB.Select(dto => new Product
                    {
                        name = dto.Name,
                        status = (byte)dto.Status,
                        productCode = dto.ProductCode,
                        price = dto.Price
                    }).ToList();
                }
            }
            return products;
        }

        public bool UpdateProductAmount(string productCode, int newAmount)
        {
            bool success = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var product = databaseContext.Products.FirstOrDefault(p => p.productCode == productCode);
                    if (product != null)
                    {
                        product.amount = newAmount;
                        databaseContext.SaveChanges();
                        success = true;
                    }
                    else
                    {
                        success = false;
                    }
                }
                catch (SqlException)
                {
                    success = false;
                }
            }
            return success;
        }

        public byte[] GetImageByProduct(string productCode)
        {
            byte[] image = null;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var product = databaseContext.Products.FirstOrDefault(p => p.productCode == productCode);
                if (product != null)
                {
                    image = product.picture;
                }
            }
            return image;
        }
    }
}