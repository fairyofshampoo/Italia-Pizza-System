using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface IEmployee
    {
        bool AddEmployee(Employee employee, Account acount);
        bool IsEmailExisting(string  email);
        bool IsUserExisting(string user);
        Account GetAccountByUsername(string username);
        bool AuthenticateAccount(string username, string password);
        List<Employee> GetLastEmployeesRegistered();
        List<Employee> GetEmployeesByStatus(int status);
        List<Employee> GetEmployeesByName(string name);      
        string GetEmployeeNameByEmail(string email);
    }
}
