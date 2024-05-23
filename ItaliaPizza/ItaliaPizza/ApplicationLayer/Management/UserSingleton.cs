using ItaliaPizza.DataLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.ApplicationLayer
{
    internal class UserSingleton
    {
        private static readonly UserSingleton instance = new UserSingleton();
        public string Username { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Role { get; set; }

        private UserSingleton() { }

        public void Initialize(Account account)
        {
            Username = account.user;
            Name = account.Employee.name;
            Email = account.email;
            Role = account.Employee.role;

        }

        public void Clear()
        {
            Username = null;
            Name = null;
            Email = null;
            Role = null;
        }


        public static UserSingleton Instance => instance;
    }
}
