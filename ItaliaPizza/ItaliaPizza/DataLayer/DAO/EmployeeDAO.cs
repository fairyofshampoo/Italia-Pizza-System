using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza.DataLayer.DAO
{
    internal class EmployeeDAO : IEmployee
    {
        public bool AddEmployee(Employee employee, Account account)
        {
            bool successfulRegistration = false;

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

                    successfulRegistration = true;

                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }
            return successfulRegistration;
        }

        public bool ModifyEmployee(Employee updateEmployee, Account updateAccount)
        {
            bool successfulUpdate = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifyEmployee = databaseContext.Employees.First(e => e.email == updateEmployee.email);
                    
                    if (modifyEmployee != null)
                    {
                        modifyEmployee.name = updateEmployee.name;
                        modifyEmployee.firstLastName = updateEmployee.firstLastName;
                        modifyEmployee.secondLastName = updateEmployee.secondLastName;
                        modifyEmployee.phone = updateEmployee.phone;
                        modifyEmployee.role = updateEmployee.role;
                    }

                    databaseContext.SaveChanges();

                    if (updateAccount.password != null)
                    {

                        var modifyAccount = databaseContext.Accounts.First(a => a.user == updateAccount.user);
                        if (modifyAccount != null)
                        {
                            modifyAccount.password = Encription.ToSHA2Hash(updateAccount.password);
                        }
                        databaseContext.SaveChanges();
                    }

                    successfulUpdate = true;

                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }

            return successfulUpdate;
        }

        public bool ChangeStatus(string user, int newStatus)
        {
            bool successfulChange = false;
            // successful, te sobró una l, by mich
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    var modifyAccount = databaseContext.Accounts.First(a => a.user == user);
                    if (modifyAccount != null)
                    {
                        modifyAccount.status = Convert.ToByte(newStatus);
                    }

                    databaseContext.SaveChanges();
                    successfulChange = true;

                } catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }

            return successfulChange;
        }

        public Employee GetEmployeeByEmail(string email)
        {
            Employee employeeFound = new Employee();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    Employee employee = databaseContext.Employees.Find(email);

                    if (employee != null)
                    {
                        employeeFound.name = employee.name;
                        employeeFound.firstLastName = employee.firstLastName;
                        employeeFound.secondLastName = employee.secondLastName;
                        employeeFound.phone = employee.phone;
                        employeeFound.email = employee.email;
                        employeeFound.role = employee.role;
                    }
                    databaseContext.SaveChanges();
                }
            } catch (ArgumentException argumentException)
            {
                throw argumentException;
            }
            return employeeFound;
        }

        public Account GetEmployeeAccountByEmail(string email)
        {
            Account accountFound = new Account();

            try
            {
                using (var databaseContext = new ItaliaPizzaDBEntities())
                {
                    var account = databaseContext.Accounts.Where(a => a.email == email).Select(a => new
                    {
                        a.user,
                        a.status
                    })
                    .FirstOrDefault();

                    if (account != null)
                    {
                        accountFound.user = account.user;
                        accountFound.status = account.status;
                    }
                }
            } catch (ArgumentException argumentException)
            {
                throw argumentException;
            }
            return accountFound;
        }

        public bool IsEmailExisting(string email)
        {
            bool isEmailExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingEmail = databaseContext.Employees.FirstOrDefault(emailexist => emailexist.email == email);
                if (existingEmail != null)
                {
                    isEmailExisting = true;
                }
            }
            return isEmailExisting;
        }

        public bool IsUserExisting(String user)
        {
            bool isUsernameExisting = false;
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var existingUsername = databaseContext.Accounts.FirstOrDefault(userexist => userexist.user == user);
                if (existingUsername != null)
                {
                    isUsernameExisting = true;
                }
            }
            return isUsernameExisting;
        }

        public Account GetAccountByUsername(string username)
        {
            Account account = new Account();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                try
                {
                    account = databaseContext.Accounts
                                         .Include(acc => acc.Employee)
                                         .FirstOrDefault(acc => acc.user == username);
                }
                catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }

            return account;
        }

        public bool AuthenticateAccount(string username, string password)
        {
            bool isAuthenticated = false;

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {

                var account = databaseContext.Accounts.FirstOrDefault(acc => acc.user == username && acc.password == password);


                if (account != null)
                    isAuthenticated = true;
            }

            return isAuthenticated;
        }
    }
}
