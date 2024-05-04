﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface ISupply
    {
        bool IsSupplyNameExisting(string name);
        bool AddSupply(Supply supply);
        bool ChangeStatus(string name, int status);
        bool ModifySupply(Supply supply, string name);
        Supply GetSupplyByName(string name);


    }
}
