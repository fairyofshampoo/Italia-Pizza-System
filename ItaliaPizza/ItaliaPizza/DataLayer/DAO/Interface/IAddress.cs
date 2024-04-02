using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IAddress
    {
        Address GetClientAddress(string email);

        List<string> GetPostalCodes();

        List<string> GetColonias(string postalCode);

        bool AddNewAddress(Address newAddress);

    }
}
