using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    
    internal class InternalOrderDAO : IInternalOrder
    {
        public bool AddInternalOrder(InternalOrder order)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                databaseContext.InternalOrders.Add(order);  
                databaseContext.SaveChanges();
                operationStatus = true;
            }
            return operationStatus;
        }

        public bool AddInternalOrderProduct(InternalOrderProduct internalOrderProduct)
        {
            bool operationStatus = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    databaseContext.InternalOrderProducts
                                   .Add(internalOrderProduct);
                    databaseContext.SaveChanges();
                    operationStatus = true;
                } catch (Exception)
                {
                    operationStatus = false;
                }
            }
            return operationStatus;
        }

        public bool CancelInternalOrder(string internalOrderCode)
        {
            bool operationStatus = false;

            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                using (var transaction = databaseContext.Database.BeginTransaction())
                {
                    try
                    {
                        var InternalOrder = databaseContext.InternalOrders
                                                           .Where(order => order.internalOrderId == internalOrderCode)
                                                           .FirstOrDefault();
                        if (InternalOrder != null)
                        {
                            databaseContext.InternalOrders.Remove(InternalOrder);
                            databaseContext.SaveChanges();
                            operationStatus = true;
                        } else
                        {
                            operationStatus = false;
                        }

                        var InternalOrderProduct = databaseContext.InternalOrderProducts
                                                                  .Where(products => products.internalOrderId == internalOrderCode)
                                                                  .ToList();
                        if(InternalOrderProduct != null)
                        {
                            databaseContext.InternalOrderProducts.RemoveRange(InternalOrderProduct);
                        } else
                        {
                            operationStatus = false;
                        }
                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                    }
                }
            }

            return operationStatus;
        }

        public List<InternalOrderProduct> GetAllInternalProductsByOrden(string internalOrderCode)
        {
            List<InternalOrderProduct> products = new List<InternalOrderProduct>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                products = databaseContext.InternalOrderProducts
                                          .Where(internalProducts => internalProducts.internalOrderId == internalOrderCode)
                                          .ToList();
            }
            return products;
        }

        public bool GetCounterOfProduct(string productId)
        {
            bool  areThereAnyRegister = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                int counter = databaseContext.InternalOrderProducts
                                         .Where(product => product.productId == productId && product.isConfirmed == 0)
                                         .Count();
                if (counter > 0)
                {
                    areThereAnyRegister = true;
                }
            }
            return areThereAnyRegister;
        }

        public InternalOrder GetInternalOrdersByNumber(string numberOrder, string waiterEmail)
        {
            InternalOrder internalOrder = new InternalOrder();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var ordersDB = databaseContext.InternalOrders
                                              .Where(order => order.internalOrderId == numberOrder && order.waiterEmail == waiterEmail)
                                              .FirstOrDefault();
                if(ordersDB != null)
                {
                    internalOrder = ordersDB;
                }
                                             
            }
            return internalOrder;
        }

        public List<InternalOrder> GetInternalOrdersByStatusAndWaiter(int status, string waiterEmail)
        {
           List<InternalOrder> internalOrders = new List<InternalOrder>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                internalOrders = databaseContext.InternalOrders
                                              .Where(order => order.status == status && order.waiterEmail == waiterEmail)
                                              .OrderByDescending(order => order.date)
                                              .ToList();
            }
            return internalOrders;
        }

        public List<Supply> GetInventoryQuantitiesForIngredients(List<string> recipeSupplyList)
        {
            List<Supply> supplies = new List<Supply>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliesDB = databaseContext.Supplies
                                                .Where(ingredient => recipeSupplyList.Contains(ingredient.name))
                                                .ToList();
                if (suppliesDB != null)
                {
                    foreach (var supply in suppliesDB)
                    {
                        supplies.Add(supply);
                    }
                }
            }
            return supplies;
        }

        public int GetMaximumProductsPosible(int recipeId)
        {
            int maximumProductsPosible;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                string query = string.Format("SELECT dbo.VALIDATEINGREDIENTSAMOUNT({0}) AS disp;", recipeId);
                maximumProductsPosible = databaseContext.Database.SqlQuery<int>(query).FirstOrDefault();
            }

            return maximumProductsPosible;
        }

        public int GetNumberOfProductsOnHold(string productId)
        {
            int numberOfProductsOnHold = 0;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                numberOfProductsOnHold = databaseContext.InternalOrderProducts
                                                   .Where(product => product.productId == productId && product.isConfirmed == 0)
                                                   .Sum(product => product.amount);
            }
            return numberOfProductsOnHold;
        }

        public List<InternalOrder> GetInternalOrdersByStatus(int status)
        {
            List<InternalOrder> internalOrders = new List<InternalOrder>();
            using(var databaseContext = new  ItaliaPizzaDBEntities())
            {
                internalOrders = databaseContext.InternalOrders
                                                .Where(order => order.status == status)
                                                .OrderByDescending(order => order.date)
                                                .ToList();
            }
            return internalOrders;
        }

        public int GetRecipeIdByProduct(string productId)
        {
            int recipeId = 0;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var recipeDB = databaseContext.Recipes
                                        .Where(recipe => recipe.ProductId == productId)
                                        .FirstOrDefault();
                if (recipeDB != null)
                {
                    recipeId = recipeDB.recipeCode;
                }
                return recipeId;
            }
        }

        public List<RecipeSupply> GetSupplyForProduct(string productId)
        {
            List<RecipeSupply> supplyForProduct = new List<RecipeSupply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var supplyData = databaseContext.RecipeSupplies
                                                .Where(data => data.supplyId == productId )
                                                .ToList();

                if(supplyData != null)
                {
                    foreach (var supply in supplyData)
                    {
                        supplyForProduct.Add(supply);
                    }
                }
            }
            return supplyForProduct;
        }

        public int GetTotalExternalProduct(string productId)
        {
            int totalExternalProduct = 0;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                totalExternalProduct = (int)databaseContext.Products
                                           .Where(product => product.productCode == productId)
                                           .Select(product => product.amount)
                                           .FirstOrDefault();
            }
            return totalExternalProduct;
        }

        public bool IncreaseAmount(string productId, string internalOrderCode)
        {
            bool operationStatus = false;
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var productDB = databaseContext.InternalOrderProducts
                                              .Where(product => product.internalOrderId == internalOrderCode && product.productId == productId)
                                              .FirstOrDefault();
                if(productDB != null)
                {
                    productDB.amount += 1;
                    databaseContext.SaveChanges();
                    operationStatus = true;
                }
            }
            return operationStatus;
        }

        public bool IsInternalOrderCodeAlreadyExisting(string internalOrderCode)
        {
           bool isExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var exist = databaseContext.InternalOrders
                                           .Where(order => order.internalOrderId == internalOrderCode)
                                           .ToList();
                if (exist != null)
                {
                    isExisting = true;
                }
            }
            return isExisting;
        }

        public bool IsRegisterInDatabase(string productId, string internalOrderCode)
        {
            bool isRegisterInDatabase = false;
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var register = databaseContext.InternalOrderProducts
                                              .Where(product => product.internalOrderId == internalOrderCode && product.productId == productId)
                                              .FirstOrDefault(); 
                if (register != null)
                {
                    isRegisterInDatabase = true;
                }
            }
            return isRegisterInDatabase;
        }

        public int SaveInternalOrder(string internalOrderCode)
        {
            int operationStatus = 0;

                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    string query = "EXEC ReduceIngredientsV10 @internalOrderCode";
                    databaseContext.Database.ExecuteSqlCommand(query, new SqlParameter("@internalOrderCode", internalOrderCode));
                    operationStatus = 1;
                }
        
            return operationStatus;
        }

        public string GetProductName(string productId)
        {
            string nameProduct = string.Empty;
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                nameProduct = databaseContext.Products
                                             .Where(product => product.productCode == productId)
                                             .Select(product => product.name)
                                             .FirstOrDefault();
            }
            return nameProduct;
        }

        public bool ChangeOrderStatus(int status, string internalOrderCode)
        {
            bool updateStatus = false;
            try
            {
                using (var databseContext = new ItaliaPizzaDBEntities())
                {
                    var order = databseContext.InternalOrders
                                              .Where(internalOrder => internalOrder.internalOrderId == internalOrderCode)
                                              .FirstOrDefault();
                    if(order != null)
                    {
                        order.status = status;
                        databseContext.SaveChanges();
                        updateStatus = true;
                    }
                }
            }catch (Exception)
            {
                updateStatus = false;
            }

            return updateStatus;
        }
    }
 
}
