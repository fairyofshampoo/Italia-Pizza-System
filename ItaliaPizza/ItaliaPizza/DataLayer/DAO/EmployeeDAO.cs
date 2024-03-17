using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

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

        public bool ModifyEmployee(Employee updateEmployee, Account updateAccount)
        {
            bool succesfulUpdate = false;

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

                    succesfulUpdate = true;

                } catch (SqlException sQLException)
                {
                    throw sQLException;
                }
            }

            return succesfulUpdate;
        }

        public bool ChangeStatus(string user, int newStatus)
        {
            bool succesfullChange = false;

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
                    succesfullChange = true;

                } catch (ArgumentException argumentException)
                {
                    throw argumentException;
                }
            }

            return succesfullChange;
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
    }
}
