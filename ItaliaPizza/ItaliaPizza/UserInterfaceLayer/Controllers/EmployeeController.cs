using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class EmployeeController
    {
        public bool AddEmployee(Employee employee, Account account)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.AddEmployee(employee, account);
        }

        public bool ModifyEmployee(Employee employee, Account account)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.ModifyEmployee(employee, account);
        }

        public bool ModifyEmployeeStatus(string user, int newStatus)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.ChangeStatus(user, newStatus);
        }

        public bool IsEmailDuplicated(string email)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.IsEmailExisting(email);
        }

        public bool IsUserDuplicated(string user)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            return employeeDAO.IsUserExisting(user);
        }

        public bool ValidateName(string name)
        {
            bool isValid = true;

            if (!Validations.IsNameValid(name))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidatePhone(string phone)
        {
            bool isValid = true;

            if (!Validations.IsPhoneValid(phone))
            {
                isValid = false;
            }
            return isValid;
        }

        public bool ValidateEmail(string email)
        {
            bool isValid = true;

            if (!Validations.IsEmailValid(email))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateEmployeeType(string employeeType)
        {
            bool isValid = true;

            if (employeeType == null)
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidateUsername(string username)
        {
            bool isValid = true;

            if (!Validations.IsUserValid(username))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool ValidatePassword(string password)
        {
            bool isValid = true;

            if (!Validations.IsPasswordValid(password))
            {
                isValid = false;
            }

            return isValid;
        }

        public bool IsNewEmployeeValid(Employee employee, Account account)
        {
            bool fullNameDataValid = ValidateName(employee.name);
            bool phoneDataValid = ValidatePhone(employee.phone);
            bool emailDataValid = ValidateEmail(employee.email);
            bool employeeTypeDataValid = ValidateEmployeeType(employee.role);
            bool usernameDataValid = ValidateUsername(account.user);
            bool passwordDataValid = ValidatePassword(account.password);

            return fullNameDataValid && phoneDataValid && emailDataValid &&
                   employeeTypeDataValid && usernameDataValid && passwordDataValid;
        }

        public bool IsUpdateEmployeeValid(Employee employee, Account account)
        {
            bool fullNameDataValid = ValidateName(employee.name);
            bool phoneDataValid = ValidatePhone(employee.phone);
            bool employeeTypeValid = ValidateEmployeeType(employee.role);
            bool passwordDataValid = ValidatePassword(account.password);

            return fullNameDataValid && phoneDataValid && employeeTypeValid && passwordDataValid;
        }
    }
}
