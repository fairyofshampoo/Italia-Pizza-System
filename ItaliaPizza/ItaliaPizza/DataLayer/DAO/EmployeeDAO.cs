using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class EmployeeDAO : IEmployee
    {
        public bool AddEmployee(Employee employee, Account account)
        {
            bool succesfulRegistration = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var newEmployee = new Employee
                    {
                        name = employee.name,
                        firstLastName = employee.firstLastName,
                        secondLastName = employee.secondLastName,
                        phone = employee.phone,
                        email = employee.email,
                        role = employee.role,
                    };
                    databaseContext.Employees.Add(newEmployee);
                    databaseContext.SaveChanges();

                    var newAccount = new Account
                    {
                        user = account.user,
                        password = Encription.ToSHA2Hash(account.password),
                        email = employee.email,
                        isAdmin = 0,
                        status = 1
                    };
                    databaseContext.Accounts.Add(newAccount);
                    databaseContext.SaveChanges();

                    succesfulRegistration = true;

                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return succesfulRegistration;
        }

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = true;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingEmail = databaseContext.Employees.FirstOrDefault(emailexist => emailexist.email == email);
                if (existingEmail != null)
                {
                    isEmailExisting = false;
                }
            }
            return isEmailExisting;
        }

        public bool IsUserExisting(String user)
        {
            bool isUsernameExisting = true;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingUsername = databaseContext.Accounts.FirstOrDefault(userexist => userexist.user == user);
                if (existingUsername != null)
                {
                    isUsernameExisting = false;
                }
            }
            return isUsernameExisting;
        }
    }
}
