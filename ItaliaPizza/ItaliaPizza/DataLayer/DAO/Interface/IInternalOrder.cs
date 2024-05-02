using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IInternalOrder
    {
        List<InternalOrder> GetInternalOrdersByStatus(string status, string waiterEmail);

        InternalOrder GetInternalOrdersByNumber(int numberOrder, string waiterEmail);
    }
}
