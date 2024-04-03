﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class SupplierDAO : ISupplier
    {
        public bool AddSupplier(Supplier supplier)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var existingSupplyAreas = databaseContext.SupplyAreas.ToList();
                    var selectedSupplyAreas = existingSupplyAreas
                        .Where(area => supplier.SupplyAreas.Any(selected => selected.area_name == area.area_name))
                        .ToList();
                    var newSupplier = new Supplier
                    {
                        email = supplier.email,
                        phone = supplier.phone,
                        companyName = supplier.companyName,
                        status = supplier.status,
                        SupplyAreas = selectedSupplyAreas,
                        manager = supplier.manager
                    };

                    databaseContext.Suppliers.Add(newSupplier);
                    databaseContext.SaveChanges();

                    return true;
                }
                catch (SqlException)
                {
                    return false;
                }
            }
        }

        public List<Supplier> GetLastSuppliersRegistered()
        {
            List<Supplier> lastSuppliers = new List<Supplier>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var lastSuppliersDB = databaseContext.Suppliers
                                                   .OrderByDescending(supplier => supplier.manager)
                                                   .Take(10)
                                                   .ToList();
                if (lastSuppliersDB != null)
                {
                    foreach (var supplier in lastSuppliersDB)
                    {
                        databaseContext.Entry(supplier)
                            .Collection(s => s.SupplyAreas)
                            .Load();

                        lastSuppliers.Add(supplier);
                    }
                }
            }
            return lastSuppliers;
        }

        public List<Supplier> SearchProductByName(string name)
        {
            List<Supplier> suppliers = new List<Supplier> ();
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliersDB = databaseContext.Suppliers
                    .Where(s => s.manager.StartsWith(name))
                    .Take(10)
                    .ToList();

                if(suppliersDB != null)
                {
                    foreach(var supplier in suppliersDB)
                    {
                        databaseContext.Entry(supplier)
                            .Collection(s => s.SupplyAreas)
                            .Load();
                        suppliers.Add(supplier);
                    }
                }
            }
            return suppliers;
        }

        public List<Supplier> SearchProductByArea(string area)
        {
            List<Supplier> suppliers = new List<Supplier>();

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var suppliersDB = databaseContext.Suppliers
                    .Where(s => s.SupplyAreas.Any(sa => sa.area_name == area))
                    .ToList();

                foreach (var supplier in suppliersDB)
                {
                    databaseContext.Entry(supplier)
                        .Collection(s => s.SupplyAreas)
                        .Load();
                    suppliers.Add(supplier);
                }
            }

            return suppliers;
        }

        public bool ChangeStatus(string email, int newStatus)
        {
            bool succesfulChange = false;
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifySupplier = databaseContext.Suppliers.First(a => a.email == email);
                    if(modifySupplier != null)
                    {
                        modifySupplier.status = Convert.ToByte(newStatus);
                    }

                    databaseContext.SaveChanges();
                    succesfulChange = true;
                } catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }
            return succesfulChange;
        }

        public bool ModifySupplier(Supplier supplierUpdated, string email)
        {
            bool successfulUpdate = false;

            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifySupplier = databaseContext.Suppliers.First(s => s.email == email);
                    if( modifySupplier != null)
                    {
                        modifySupplier.email = supplierUpdated.email;
                        modifySupplier.phone = supplierUpdated.phone;
                        modifySupplier.companyName = supplierUpdated.companyName;
                        modifySupplier.manager = supplierUpdated.manager;
                        modifySupplier.SupplyAreas = supplierUpdated.SupplyAreas;
                    }

                    if(databaseContext.SaveChanges() == Constants.SUCCESSFUL_RESULT)
                    {
                        successfulUpdate = true;
                    }

                } catch (SqlException sqlException)
                {
                    throw sqlException;
                }
            }

            return successfulUpdate;
        }

        public Supplier GetSupplierByEmail (string email)
        {
            Supplier supplierFound = new Supplier();
            try
            {
                using(var databaseContext = new ItaliaPizzaDBEntities())
                {
                    Supplier supplier = databaseContext.Suppliers.Find(email);
                    if ( supplier != null)
                    {
                        databaseContext.Entry(supplier)
                            .Collection(s => s.SupplyAreas)
                            .Load();
                        supplierFound = supplier;
                    }
                }
            }
            catch (ArgumentException argumentException)
            {
                throw argumentException;
            }

            return supplierFound;
        }

        public bool IsEmailSupplierExisting(string email)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                return databaseContext.Suppliers.Any(s => s.email == email);
            }
        }
    }
}
