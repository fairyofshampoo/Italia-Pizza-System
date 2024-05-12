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

        bool EditDataClient(Client client);

        bool ChangeStatusClient(string email, int status);

        List<Client> GetLastClientsRegistered();

        List<Client> GetClientsByPhone(string phone);

        List<Client> GetClientsByAddress(string address);

        List<Client> GetClientsByName(String name);
    }
}
