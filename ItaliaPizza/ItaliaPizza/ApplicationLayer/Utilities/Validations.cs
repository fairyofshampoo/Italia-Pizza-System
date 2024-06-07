using ItaliaPizzaData.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media.Media3D;
using System.Xml.Linq;

namespace ItaliaPizza.ApplicationLayer
{
    public class Validations
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

        public static bool IsTotalPaymentValid(string totalPayment)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(totalPayment))
            {
                isValid = false;
            }
            else if (!decimal.TryParse(totalPayment, out decimal paymentValue) || paymentValue <= 0)
            {
                isValid = false;
            }

            var totalPaymentRegex = new Regex("^\\d{0,8}(\\.\\d{0,2})?$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(totalPayment, totalPaymentRegex);
        }

        public static bool IsSupplyAmountValid(string amount)
        {
            return IsAmountValid(amount, false);
        }

        public static bool IsSupplyNewAmountValid(string amount)
        {
            return IsAmountValid(amount, true);
        }
        private static bool IsAmountValid(string amount, bool allowZero)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(amount))
            {
                isValid = false;
            }
            else if (!decimal.TryParse(amount, out decimal paymentValue) || (!allowZero && paymentValue <= 0))
            {
                isValid = false;
            }

            var amountRegex = new Regex("^\\d{0,3}(\\.\\d{0,3})?$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(amount, amountRegex);
        }
        public static bool IsProductIntegerValid(string productInt)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(productInt))
            {
                isValid = false;
            }

            var integerRegex = new Regex("^\\d{1,6}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(productInt, integerRegex);
        }

        public static bool IsAddressValid(string address) 
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(address))
            {
                isValid = false;
            }

            var addressRegex = new Regex("^[\\p{L}\\p{M}\\s]{1,60}",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(address, addressRegex);
        }

        public static bool IsPhoneValid(string phone)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(phone))
            {
                isValid = false;
            }

            var nameRegex = new Regex("^\\d{8,10}$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(phone, nameRegex);
        }


        public static bool IsEmailValid(string email)
        {
            bool emailValidation = true;
            int maximumEmailLength = 30;
            int limitTime = 500;
            var emailRegex = new Regex(@"^(?=.{1,30}$)[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$",
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

        public static bool IsNumber(string text)
        {
            int limitTime = 500;
            var numberRegex = new Regex("[0-9]", RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));
            return ValidateWithTimeout(text, numberRegex);
        }

        public static bool IsPasswordValid(string password)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(password))
            {
                isValid = false;
            }
            var passwordRegex = new Regex("^(?=.*[A-Za-z])(?=.*\\d)(?=.*[@$!%*#?&])[A-Za-z\\d@$!%*#?&]{8,20}$",
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
            var nameProductRegex = new Regex("^(?=.{1,100}$)(?![ .])[a-zA-Z0-9áéíóúñÑÁÉÍÓÚ]+(?: [a-zA-Z0-9áéíóúñÑÁÉÍÓÚ]+)*$",
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

        public static bool IsSupplyNameValid(string code)
        {
            int limitTime = 500;
            bool isValid = true;

            if (string.IsNullOrWhiteSpace(code))
            {
                isValid = false;
            }
            var supplyNameRegex = new Regex("^(?=.{1,50}$)(?![ .])[a-zA-Z0-9áéíóúÁÉÍÓÚ]+(?: [a-zA-Z0-9áéíóúÁÉÍÓÚ]+)*$",
                RegexOptions.None, TimeSpan.FromMilliseconds(limitTime));

            return isValid && ValidateWithTimeout(code, supplyNameRegex);
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

            if (!IsNameValid(data[0]))
            {
                errors.Add(NAME_ERROR);
            }

            if (!IsPhoneValid(data[1]))
            {
                errors.Add(PHONE_ERROR);
            }

            if (data.Count > 2)
            {
                if (!IsEmailValid(data[2]))
                {
                    errors.Add(EMAIL_ERROR);
                }
            }

            return errors;
        }

    }
}
