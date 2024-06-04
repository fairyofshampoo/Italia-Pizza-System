using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class ClientController
    {
        public bool RegisterClient(Client client)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.AddClient(client);
        }

        public bool IsDataValid(List<String> data)
        {
            bool isDataValid = true;
            List<int> errors = ApplicationLayer.Validations.ValidationClientData(data);
            if (errors.Any())
            {
                isDataValid = false;
            }
            return isDataValid;
        }

        public bool IsEmailExisting(String email)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.IsEmailExisting(email);
        }

        public List<Client> SearchClientByPhoneNumber(String phoneNumber)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.GetClientsByPhone(phoneNumber);
        }

        public List<Client> SearchClientByAddress(String address) 
        { 
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.GetClientsByAddress(address);
        }

        public List<Client> SearchClientByName(String name) 
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.GetClientsByName(name);
        }

        public bool EditClient(Client client)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.EditDataClient(client);
        }

        public bool ChangeStatusClient(String emailClient, int status)
        {
            ClientDAO clientDAO = new ClientDAO();
            return clientDAO.ChangeStatusClient(emailClient, status);
        }
    }
}
