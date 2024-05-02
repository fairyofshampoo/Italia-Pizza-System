using ItaliaPizza.ApplicationLayer;
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

        public bool ChangeStatus(string name, int status)
        {
            bool successfulChange = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifySupply = databaseContext.Supplies.First(a => a.name == name);
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
                    successfulChange = true;
                }
                catch (ArgumentException argumentException)
                {
                    throw argumentException;
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

        public bool ModifySupply(Supply supply, string name)
        {
            var successfulUpdate = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifySupply = databaseContext.Supplies.First(s => s.name == name);

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

        public Supply GetSupplyByName(string name)
        {
            Supply supplyFound = new Supply();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    Supply supply = databaseContext.Supplies.Find(name);

                    if (supply != null)
                    {
                        supplyFound.name = supply.name;
                        supplyFound.amount = supply.amount;
                        supply.measurementUnit = supply.measurementUnit;
                        supplyFound.category = supply.category;
                        supplyFound.status = supply.status;
                    }

                    databaseContext.SaveChanges();
                }
            }
            catch (ArgumentException argumentException)
            {
                throw argumentException;
            }
            return supplyFound;
        }        
    }
}
