using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using iText.Layout.Borders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class SupplierOrderController
    {
        private readonly SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
        public int CreateSupplierOrder(string supplierEmail)
        {
            return supplyOrderDAO.AddSupplierOrder(SetNewSupplierOrder(supplierEmail));
        }

        private SupplierOrder SetNewSupplierOrder(string supplierEmail)
        {
            DateTime currentDateTime = DateTime.Now;
            SupplierOrder newSupplierOrder = new SupplierOrder()
            {
                status = Constants.INACTIVE_STATUS,
                date = currentDateTime,
                total = 0,
                modificationDate = currentDateTime,
                supplierId = supplierEmail,
            };

            return newSupplierOrder;
        }

        public bool ValidateTotalPayment(string totalPayment)
        {
            return Validations.IsTotalPaymentValid(totalPayment);
        }

        public List<Supply> GetSuppliesInOrder(int orderId)
        {
            return supplyOrderDAO.GetSuppliesByOrderId(orderId);
        }

        public decimal GetAmountOfSupplyInOrder(string supplyName, int orderId)
        {
            return supplyOrderDAO.GetOrderedQuantityBySupplierOrderId(orderId, supplyName);
        }

        public bool UpdateSuppliesInOrder(string supplyName, int orderId, decimal amount)
        {
            bool result = true;
            if (supplyOrderDAO.IsSupplyAlreadyInOrder(supplyName, orderId))
            {
                supplyOrderDAO.UpdateSupplyAmountInOrder(amount, orderId, supplyName);

            }
            else
            {
                if (!AddSupplyToOrder(supplyName, orderId, amount))
                {
                    result = false;
                }
            }

            return result;
        }

        public bool AddSupplyToOrder(string supplyName, int orderId, decimal amount)
        {
            return supplyOrderDAO.AddSupplyToOrder(supplyName, orderId, amount);
        }

        public bool UpdateRemovingSuppliesInOrder(string supplyName, int orderId)
        {
            return supplyOrderDAO.DeleteSupplyFromOrder(supplyName, orderId);
        }

        public bool RegisterPayment(decimal totalPayment, int orderId)
        {
            SupplyOrderDAO supplyOrderDAO = new SupplyOrderDAO();
            return supplyOrderDAO.UpdateStatusOrderAndPayment(orderId, Constants.ACTIVE_STATUS, totalPayment);
        }

        public bool DeleteSupplierOrder(int orderId)
        {
            return supplyOrderDAO.DeleteSupplierOrder(orderId);
        }
    }
}
