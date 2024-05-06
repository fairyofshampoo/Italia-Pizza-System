using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface ISupplyOrder
    {
        int AddSupplierOrder(SupplierOrder supplierOrder);
        bool AddSupplyToOrder(string supplyName, int orderId);
        bool DeleteSupplierOrder(int supplierOrderId);
    }
}
