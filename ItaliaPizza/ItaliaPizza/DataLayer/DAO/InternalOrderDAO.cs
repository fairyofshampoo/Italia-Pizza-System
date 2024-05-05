using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
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
                } catch (Exception ex)
                {
                    operationStatus = false;
                }
            }
            return operationStatus;
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

        public List<InternalOrder> GetInternalOrdersByStatus(int status, string waiterEmail)
        {
           List<InternalOrder> internalOrders = new List<InternalOrder>();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var ordersDB = databaseContext.InternalOrders
                                              .Where(order => order.status == status && order.waiterEmail == waiterEmail)
                                              .ToList();
                if(ordersDB != null)
                {
                    foreach(var order in ordersDB)
                    {
                        internalOrders.Add(order);
                    }
                }
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

        public bool InternalOrderTransaction(string internalOrderCode)
        {
            throw new NotImplementedException();
        }

        public bool IsInternalOrderCodeAlreadyExisting(string internalOrderCode)
        {
           bool isExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var exist = databaseContext.InternalOrders
                                           .Where(order => order.internalOrderId == internalOrderCode) //Cambiar en la base de datos el tipo de dato
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
    }
 
}
