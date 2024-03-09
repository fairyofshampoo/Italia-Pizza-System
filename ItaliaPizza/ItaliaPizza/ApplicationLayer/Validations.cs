using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ItaliaPizza.ApplicationLayer
{
    internal class Validations
    {
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

    }
}
