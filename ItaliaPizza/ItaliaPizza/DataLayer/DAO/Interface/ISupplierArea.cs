using ItaliaPizza.ApplicationLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    public interface ISupplierArea
    {
        List<supplyArea> GetAllSupplyAreas();
    }
}
