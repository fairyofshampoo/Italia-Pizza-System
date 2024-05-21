using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class SupplyDAO : ISupply
    {
        public bool AddSupply(Supply supply)
        {
            bool successfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newSupply = new Supply
                    {
                        name = supply.name,
                        amount = supply.amount,
                        measurementUnit = supply.measurementUnit,
                        category = supply.category,
                        status = supply.status
                    };

                    databaseContext.Supplies.Add(newSupply);
                    databaseContext.SaveChanges();

                    successfulRegistration = true;
                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public bool ChangeSupplyStatus(Supply supply, int status)
        {
            bool successfulChange = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                using (var transaction = databaseContext.Database.BeginTransaction())
                {
                    try
                    {
                        var modifySupply = databaseContext.Supplies.First(a => a.name == supply.name);
                        if (modifySupply != null)
                        {
                            if (status == Constants.INACTIVE_STATUS)
                            {
                                modifySupply.status = false;
                            }
                            else
                            {
                                modifySupply.status = true;
                            }
                        }
                        databaseContext.SaveChanges();

                        if (supply.SupplyArea.area_name == "Producto externo")
                        {
                            var modifyProduct = databaseContext.Products.First(p => p.productCode == supply.productCode);
                            if (modifyProduct != null)
                            {
                                modifyProduct.status = Convert.ToByte(status);
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

        public bool IsSupplyNameExisting(string name)
        {
            bool isSupplyNameExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingName = databaseContext.Supplies.FirstOrDefault(n => n.name == name);
                if (existingName != null)
                {
                    isSupplyNameExisting = true;
                }
            }
            return isSupplyNameExisting;
        }

        public bool ExistsSupplyInRecipe(string supplyName)
        {
            bool existsSupplyInRecipe = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingInRecipe = databaseContext.RecipeSupplies.FirstOrDefault(n => n.supplyId == supplyName);
                if (existingInRecipe != null)
                {
                    var recipeActive = databaseContext.Recipes.FirstOrDefault(r => r.recipeCode == existingInRecipe.recipeID && r.status == Constants.ACTIVE_STATUS);
                    if (recipeActive != null)
                    {
                        existsSupplyInRecipe = true;
                    }
                }
            }
            return existsSupplyInRecipe;
        }

        public bool ModifySupply(Supply supply, string name)
        {
            var successfulUpdate = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifySupply = databaseContext.Supplies.First(s => s.name == supply.name);

                    if (modifySupply != null)
                    {
                        modifySupply.amount = supply.amount;
                        modifySupply.name = supply.name;
                        modifySupply.measurementUnit = supply.measurementUnit;
                        modifySupply.category = supply.category;
                    }

                    databaseContext.SaveChanges();
                    successfulUpdate = true;
                } 
                catch (SqlException sqlException)
                {
                    throw sqlException;
                }
            }

            return successfulUpdate;
        }
        
        public List<Supply> GetSuppliesByStatus(bool status)
        {
            List<Supply> suppliesDB = new List<Supply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var supplies = databaseContext.Supplies
                                              .Where(s => s.status == status)
                                              .Include(s => s.SupplyArea)
                                              .ToList();
                if (supplies != null)
                {
                    foreach (var supply in supplies)
                    {
                        suppliesDB.Add(supply);
                    }
                }
            }
            return suppliesDB;            
        }

        public List<Supply> GetAllSupplies()
        {
            List<Supply> suppliesDB = new List<Supply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var supplies = databaseContext.Supplies
                                              .Include(s => s.SupplyArea)
                                              .ToList();
                if (supplies != null)
                {
                    foreach (var supply in supplies)
                    {
                        suppliesDB.Add(supply);
                    }
                }
            }
            return suppliesDB;
        }

        public List<Supply> SearchSupplyByName(string name)
        {
            List<Supply> supplies = new List<Supply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliesDB = databaseContext.Supplies.ToList();

                var filteredSupplies = suppliesDB.Where(s => DiacriticsUtilities.RemoveDiacritics(s.name).ToUpper().Contains(DiacriticsUtilities.RemoveDiacritics(name).ToUpper()))
                                                 .Take(10)
                                                 .ToList();

                if (filteredSupplies != null)
                {
                    foreach (var supply in filteredSupplies)
                    {
                        databaseContext.Entry(supply)
                            .Reference(s => s.SupplyArea)
                            .Load();
                        supplies.Add(supply);
                    }
                }
            }
            return supplies;
        }

        public List<object> GetAllSuppliesAndExternalProducts()
        {
            byte trueByte = 1;
            List<object> supplyAndExternalProducts = new List<object>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var supplies = databaseContext.Supplies
                                              .Include(s => s.SupplyArea)
                                              .ToList<object>();

                var externalProducts = databaseContext.Products
                                                       .Where(p => p.isExternal == trueByte)
                                                       .ToList<object>();
                supplyAndExternalProducts.AddRange(supplies);
                supplyAndExternalProducts.AddRange(externalProducts);
            }
            return supplyAndExternalProducts;
        }


        public List<Supply> GetRecipeSupplies(int idRecipe)
        {
            List<Supply> suppliesDB = new List<Supply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var recipeSupplies = databaseContext.RecipeSupplies
                                              .Where(r => r.recipeID == idRecipe)
                                              .ToList();
                if (recipeSupplies != null)
                {
                    foreach (var recipeSupply in recipeSupplies)
                    {
                        Supply supply = new Supply
                        {
                            name = recipeSupply.supplyId,
                            category = recipeSupply.Supply.category,
                            status = recipeSupply.Supply.status,
                            amount = recipeSupply.supplyAmount,
                            measurementUnit = recipeSupply.Supply.measurementUnit,
                        };
                        suppliesDB.Add(supply);
                    }
                }
            }
            return suppliesDB;
        }

        public List<object> SearchSupplyOrExternalProductByName(string name)
        {
            List<object> suppliesAndExternalProducts = new List<object>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliesQuery = databaseContext.Supplies
                                                   .Include(s => s.SupplyArea)
                                                   .ToList()
                                                   .Where(s => DiacriticsUtilities.RemoveDiacritics(s.name).ToUpper().Contains(DiacriticsUtilities.RemoveDiacritics(name).ToUpper()))
                                                   .Select(s => (object)s);

                var productsQuery = databaseContext.Products
                                                    .ToList()
                                                    .Where(p => DiacriticsUtilities.RemoveDiacritics(p.name).ToUpper().Contains(DiacriticsUtilities.RemoveDiacritics(name).ToUpper()) && p.isExternal == 1)
                                                    .Select(p => (object)p);

                var combinedQuery = suppliesQuery.Union(productsQuery)
                                                 .Take(10)
                                                 .ToList();

                suppliesAndExternalProducts.AddRange(combinedQuery);
            }
            return suppliesAndExternalProducts;
        }

        public List<Supply> SearchActiveSupplyByName(string name)
        {
            List<Supply> supplies = new List<Supply>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliesDB = databaseContext.Supplies.ToList();

                var filteredSupplies = suppliesDB.Where(s => DiacriticsUtilities.RemoveDiacritics(s.name).ToUpper().Contains(DiacriticsUtilities.RemoveDiacritics(name).ToUpper()) && s.status == true)
                                         .Take(10)
                                         .ToList();

                if (filteredSupplies != null)
                {
                    foreach (var supply in filteredSupplies)
                    {
                        databaseContext.Entry(supply)
                            .Reference(s => s.SupplyArea)
                            .Load();
                        supplies.Add(supply);
                    }
                }
            }
            return supplies;
        }

        public bool ModifySupplyAmount(string name, decimal newAmount)
        {
            bool changesSaved = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var supply = databaseContext.Supplies.FirstOrDefault(s => s.name == name);
                    if (supply != null)
                    {
                        supply.amount = newAmount;
                    }
                    var product = databaseContext.Products.FirstOrDefault(p => p.productCode == supply.productCode);
                    if (product != null)
                    {
                        product.amount = (int)supply.amount;
                    }

                    int rowsAffected = databaseContext.SaveChanges();
                    if (rowsAffected > 0)
                    {
                        changesSaved = true;
                    }
                }
                catch (SqlException)
                {
                    changesSaved = false;
                }
            }
            return changesSaved;
        }

        public bool UpdateInventoryFromOrder(List<Supply> orderSupplies)
        {
            bool changesSaved = false;

            using (var dbContext = new ItaliaPizzaDBEntities())
            {
                foreach (var orderSupply in orderSupplies)
                {
                    var supply = dbContext.Supplies.FirstOrDefault(s => s.name == orderSupply.name);
                    if (supply != null)
                    {
                        supply.amount += orderSupply.amount;
                    }

                    var product = dbContext.Products.FirstOrDefault(p => p.productCode == supply.productCode);
                    if(product != null)
                    {
                        product.amount = (int)supply.amount;
                    }
                }
                int rowsAffected = dbContext.SaveChanges();
                if (rowsAffected > 0)
                {
                    changesSaved = true;
                }
            }

            return changesSaved;
        }

    }
}
