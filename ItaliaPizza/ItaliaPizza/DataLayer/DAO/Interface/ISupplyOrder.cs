﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.DataLayer.DAO.Interface
{
    internal interface ISupplyOrder
    {
        int AddSupplierOrder(SupplierOrder supplierOrder);
        bool AddSupplyToOrder(string supplyName, int orderId, decimal quantityOrdered);
        bool DeleteSupplierOrder(int supplierOrderId);
        bool IncreaseSupplyAmountInOrder(Supply supply, int orderCode);
        bool IsSupplyAlreadyInOrder(string supplyName, int orderId);
        List<Supply> GetSuppliesByOrderId(int supplierOrderId);
        decimal GetOrderedQuantityBySupplierOrderId(int supplierOrderId, string supplyId);
        bool UpdateStatusOrderAndPayment(int supplierOrderId,byte status, decimal totalPayment);
        bool AreAnySuppliesInOrder(int supplierOrderId);
        bool DeleteSupplyFromOrder(string supplyName, int orderId);
        bool UpdateSupplyAmountInOrder(decimal  amount, int orderCode, string supplyName);
    }
}
