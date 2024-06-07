using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface ISupplier
    {
        bool AddSupplier(Supplier supplier);
        bool IsEmailSupplierExisting(string email);
    }
}
