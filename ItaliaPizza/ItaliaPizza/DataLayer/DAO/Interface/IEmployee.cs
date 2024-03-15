using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface IEmployee
    {
        bool AddEmployee(Employee employee, Account acount);
        bool IsEmailExisting(string  email);
        bool IsUserExisting(String user);
    }
}
