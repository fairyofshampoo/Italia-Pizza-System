using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IClient
    {
        bool AddClient(Client client);

        bool IsEmailExisting(string email);
    }
}
