﻿using ItaliaPizza.DataLayer;
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
        public string FirstLastName { get; set; }
        public string SecondLastName { get; set; }
        public string Email { get; set; }
        public byte IsAdmin { get; set; }

        private UserSingleton() { }

        public void Initialize(Account account)
        {
            Username = account.user;
            Name = account.Employee.name;
            FirstLastName = account.Employee.firstLastName;
            SecondLastName = account.Employee.secondLastName;
            Email = account.email;
            IsAdmin = account.isAdmin;

        }

        public void Clear()
        {
            Username = null;
            Name = null;
            FirstLastName = null;
            SecondLastName = null;
            Email = null;
            IsAdmin = 0;
        }


        public static UserSingleton Instance => instance;
    }
}