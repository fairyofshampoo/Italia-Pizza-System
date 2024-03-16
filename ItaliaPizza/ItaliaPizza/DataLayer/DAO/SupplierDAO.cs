using ItaliaPizza.DataLayer.DAO.Interface;
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


        public bool IsEmailSupplierExisting(string email)
        {
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                return databaseContext.Suppliers.Any(s => s.email == email);
            }
        }
    }
}
