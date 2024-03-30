using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;

namespace ItaliaPizza.ApplicationLayer
{
    internal class Validations
    {
        private static int NAME_ERROR = 1;
        private static int PHONE_ERROR = 2;
        private static int EMAIL_ERROR = 3;    

        public static bool IsNameValid(string name)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(name))
            {
                isValid = false;
            }

            var nameRegex = new Regex("^[\\p{L}\\p{M}\\s]{1,50}",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(name, nameRegex);
        }

        public static bool IsPhoneValid(string phone)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(phone))
            {
                isValid = false;
            }

            var nameRegex = new Regex("^\\d{1,10}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(phone, nameRegex);
        }


        public static bool IsEmailValid(string email)
        {
            bool emailValidation = true;
            int maximumEmailLength = 50;
            int limitTime = 500;
            var emailRegex = new Regex(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));
            if (string.IsNullOrEmpty(email) || email.Length > maximumEmailLength)
            {
                emailValidation = false;
            }
            else
            {
                try
                {
                    var mailAddress = new MailAddress(email);
                }
                catch (FormatException)
                {
                    emailValidation = false;
                }
            }
            return emailValidation && ValidateWithTimeout(email, emailRegex);
        }
        public static bool IsCompanyNameValid(string companyName)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(companyName))
            {
                isValid = false;
            }
            var companyNameRegex = new Regex("^[\\p{L}\\p{M}\\s\\p{S}]{1,30}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(companyName, companyNameRegex);
        }

        public static bool IsUserValid(string user)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(user))
            {
                isValid = false;
            }
            var userRegex = new Regex("^(?=.{2,20}$)(?!.*\\s)[a-zA-Z0-9!@#$%^&*()-_+=|\\\\{}[\\]:;\"'<>,.?/~]*$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(user, userRegex);
        }

        public static bool IsPasswordValid(string password)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
            }
            var passwordRegex = new Regex("^(?=.*[!@#$%^&*()-_+=|\\\\{}[\\]:;\"'<>,.?/~])(?=.*\\d)[a-zA-Z0-9!@#$%^&*()-_+=|\\\\{}[\\]:;\"'<>,.?/~]{8,}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(password, passwordRegex);
        }

        public static bool IsProductNameValid(string nameProduct)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(nameProduct))
            {
                isValid = false;
            }
            var nameProductRegex = new Regex("^(?=.{1,100}$)(?![ .])[a-zA-Z0-9áéíóúÁÉÍÓÚ]+(?: [a-zA-Z0-9áéíóúÁÉÍÓÚ]+)*$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(nameProduct, nameProductRegex);
        }

        public static bool IsProductCodeValid(string code)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(code))
            {
                isValid = false;
            }
            var codeProductRegex = new Regex("^[a-zA-Z0-9]{1,50}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(code, codeProductRegex);
        }

        private static bool ValidateWithTimeout(string input, Regex regex)
        {
            bool isValid;
            try
            {
                isValid = regex.IsMatch(input);
            }
            catch (RegexMatchTimeoutException)
            {
                isValid = false;
            }
            return isValid;
        }

        public static List<int> ValidationClientData(List<String> data)
        {
            List<int> errors = new List<int>();
            Console.WriteLine("Errores");

            Console.WriteLine(data[0]);
            Console.WriteLine(data[1]);

            if (!IsNameValid(data[0]))
            {
                //No los está imprimiendo
                Console.WriteLine(data[0]);
                Console.WriteLine("Pasé por el error de nombre");
                errors.Add(NAME_ERROR);
            }

            if (!IsPhoneValid(data[1]))
            {
                Console.WriteLine(data[1]);
                Console.WriteLine("Pasé por el error de teléfono");
                errors.Add(PHONE_ERROR);
            }

            if (!IsEmailValid(data[2]))
            {
                errors.Add(EMAIL_ERROR);
            }

            return errors;
        }

    }
}
