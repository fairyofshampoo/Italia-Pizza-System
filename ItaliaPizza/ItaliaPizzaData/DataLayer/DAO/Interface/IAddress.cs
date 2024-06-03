﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizzaData.DataLayer.DAO.Interface
{
    public interface IAddress
    {
        Address GetClientAddress(string email);

        List<string> GetPostalCodes();

        List<string> GetColonias(string postalCode);

        bool AddNewAddress(Address newAddress);

        Address GetAddressById(int id);

        bool EditAddress(Address address);

        bool DisableAddress(int id);

        bool EnableAddress(int id);

        List<Address> GetAddressesByClient(string emailClient);

        List<Address> GetAddressByStatus(int status, string emailClient);
    }
}