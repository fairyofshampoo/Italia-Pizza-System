using ItaliaPizzaData.DataLayer.DAO.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizzaData.DataLayer.DAO
{
    public class EmployeeDAO : IEmployee
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
                        phone = employee.phone,
                        email = employee.email,
                        role = employee.role,
                    };

                    databaseContext.Employees.Add(newEmployee);
                    databaseContext.SaveChanges();

                    var newAccount = new Account
                    {
                        user = account.user,
                        password = account.password,
                        email = employee.email,
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
                        modifyEmployee.phone = updateEmployee.phone;
                        modifyEmployee.role = updateEmployee.role;
                    }

                    databaseContext.SaveChanges();

                    if (updateAccount.password != null)
                    {

                        var modifyAccount = databaseContext.Accounts.First(a => a.user == updateAccount.user);
                        if (modifyAccount != null)
                        {
                            modifyAccount.password = updateAccount.password;
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

        public bool IsUserExisting(string user)
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

        public List<Employee> GetLastEmployeesRegistered()
        {
            List<Employee> lastEmployees = new List<Employee>();
            using (var databseContext = new ItaliaPizzaDBEntities())
            {
                var lastEmployeesRegistered = databseContext.Employees
                                                           .OrderByDescending(e => e.name)
                                                           .Take(10)
                                                           .ToList();
                if (lastEmployeesRegistered != null)
                {
                    foreach (var employee in lastEmployeesRegistered)
                    {
                        lastEmployees.Add(employee);
                    }
                }
            }
            return lastEmployees;
        }

        public List<Employee> GetEmployeesByStatus(int status)
        {
            List<Employee> employeesDB = new List<Employee>();

            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var employees = (from employee in databaseContext.Employees
                                 join account in databaseContext.Accounts
                                 on employee.email equals account.email
                                 where account.status == status
                                 select employee)
                                .ToList();

                if (employees != null)
                {
                    foreach (var product in employees)
                    {
                        employeesDB.Add(product);
                    }
                }
            }
            return employeesDB;
        }

        public List<Employee> GetEmployeesByName(string name)
        {
            List<Employee> employeesDB = new List<Employee>();
            using (var databaseContext = new ItaliaPizzaDBEntities())
            {
                var employees = databaseContext.Employees
                                              .Where(e => e.name.StartsWith(name))
                                              .Take(5)
                                              .ToList();
                if (employees != null)
                {
                    foreach (var employee in employees)
                    {
                        employeesDB.Add(employee);
                    }
                }
            }
            return employeesDB;
        }
        
        public string GetEmployeeNameByEmail(string email)
        {
            string employeeName = null;
            using(var databaseContext = new ItaliaPizzaDBEntities())
            {
                var employeeNameDB = databaseContext.Employees
                                                    .Where(employee => employee.email == email)
                                                    .Select(employee => employee.name)
                                                    .FirstOrDefault();

                if(employeeNameDB != null)
                {
                    employeeName = employeeNameDB;
                }
            }
            return employeeName;
        }
    }
}
