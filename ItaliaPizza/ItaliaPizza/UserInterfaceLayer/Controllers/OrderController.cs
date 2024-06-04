using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace ItaliaPizza.UserInterfaceLayer.Controllers
{
    public class OrderController
    {

        private string RandomString(Random rnd, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder(length);
            for (int index = 0; index < length; index++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }
            return sb.ToString();
        }

        public List<InternalOrder> GetOrderToBePrepared()
        {
            OrderDAO orderDAO = new OrderDAO();
            return orderDAO.GetInternalOrdersByStatus(Constants.ORDER_STATUS_PENDING_PREPARATION);
        }

        public List<Product> GetProductsByOrder(string orderId)
        {
            OrderDAO orderDAO = new OrderDAO();
            return orderDAO.GetProductsByOrderId(orderId);
        }

        public bool ChangeOrderStatus(int status, string internalOrderId)
        {
            OrderDAO orderDAO = new OrderDAO();
            return orderDAO.ChangeOrderStatus(status, internalOrderId);
        }

        public string CreateOrderCode()
        {
            DateTime date = DateTime.Today;
            Random random = new Random();
            string randomChar = RandomString(random, 4);
            OrderDAO orderDAO = new OrderDAO();
            string code = string.Empty;
            do
            {
                code = $"{date:ddMMyy}-{randomChar}";
            } while (!orderDAO.IsInternalOrderCodeAlreadyExisting(code));
            return code;
        }

        public bool RegisterInternalOrder(string orderCode, string waiterEmail)
        {
            DateTime currentDate = DateTime.Now;
            var newInternalOrder = new InternalOrder
            {
                internalOrderId = orderCode,
                status = 0,
                date = currentDate,
                total = 0,
                waiterEmail = waiterEmail
            };

            OrderDAO orderDAO = new OrderDAO();
            return orderDAO.AddOrder(newInternalOrder);
        }

        public bool RegisterOrder(InternalOrder newOrder)
        {
            OrderDAO orderDAO = new OrderDAO();
            return orderDAO.AddOrder(newOrder);
        }

        public int GetNumberOfProducts(int recipeId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            return internalOrderDAO.GetMaximumProductsPosible(recipeId);
        }

        public int GetCountOfProduct(string productCode)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            return internalOrderDAO.GetTotalExternalProduct(productCode);
        }

        public bool AddProductToOrder(InternalOrderProduct internalOrderProduct)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            return internalOrderDAO.AddInternalOrderProduct(internalOrderProduct);
        }

        public List<InternalOrder> GetInternalOrderByStatus(string waiterEmail, int status) 
        { 
            OrderDAO internalOrderDAO = new OrderDAO();
            return internalOrderDAO.GetInternalOrdersByStatusAndWaiter(status, waiterEmail);
        }

    }
}
