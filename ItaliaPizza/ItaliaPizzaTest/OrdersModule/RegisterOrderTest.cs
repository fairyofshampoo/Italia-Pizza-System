using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;


namespace ItaliaPizzaTest.OrdersModule
{
    [TestClass]
    public class RegisterOrderTest
    {
        OrderController _orderController = new OrderController();


        [TestMethod]
        public void RegisterInternalOrder_Success()
        {
            string internalOrderCode;
            //Arrange
            string orderCode = _orderController.CreateOrderCode();
            string waiterEmail = "monita@gmail.com";

            //Act
            bool result = false;
            if (_orderController.RegisterInternalOrder(orderCode, waiterEmail))
            {
                result = true;
                internalOrderCode = orderCode;
            }

            //Assert
            Assert.IsTrue(result, "El registro de la orden debería ser exitosa");
        }

        [TestMethod]
        public void RegisterHomeOrder_Success()
        {
            OrderDAO orderDAO = new OrderDAO();
            //Arrange
            string orderCode = _orderController.CreateOrderCode();
            string email = "michelini@gmail.com";

            DateTime currentDate = DateTime.Now;
            var newHomeOrder = new InternalOrder
            {
                internalOrderId = orderCode,
                status = 0,
                date = currentDate,
                total = 0,
                clientEmail = email,
            };

            //Act
            bool result = _orderController.RegisterOrder(newHomeOrder) && orderDAO.UpdateInternalOrderAddress(orderCode, 2);
            //Assert
            Assert.IsTrue(result, "El registro de la orden debería ser exitosa");
        }

        [TestMethod]
        public void AddInternalProduct_Success()
        {
            //Arrange
            int recipeProduct = 19;
            OrderDAO internalOrderDAO = new OrderDAO();
            int productsPosibles = _orderController.GetNumberOfProducts(recipeProduct);

            //Act
            bool result = false;
            if(productsPosibles > 0)
            {
                var newProductOrder = new InternalOrderProduct()
                {
                    amount = 1, 
                    isConfirmed = 0, 
                    internalOrderId = "020624-89C6",
                    productId = "PM001"
                };

                result = _orderController.AddProductToOrder(newProductOrder);
            }

            //Assert
            Assert.IsTrue(result, "El resultado de agregar un nuevo producto debería ser válido");
        }

        [TestMethod]
        public void AddProductToHomeOrder_Success()
        {
            OrderDAO _orderDAO = new OrderDAO();
            string orderCode = "040624-2RD9";
            //Arrange
            int recipeProduct = 19;
            int productsPosibles = _orderController.GetNumberOfProducts(recipeProduct);

            //Act
            bool result = false;
            if (productsPosibles > 0)
            {
                var newProductOrder = new InternalOrderProduct()
                {
                    amount = 1,
                    isConfirmed = 0,
                    internalOrderId = orderCode,
                    productId = "PM001"
                };

                result = _orderController.AddProductToOrder(newProductOrder);
            }

            _orderDAO.SaveInternalOrder(orderCode);

            //Assert
            Assert.IsTrue(result, "El resultado de agregar un nuevo producto debería ser válido");
        }

        [TestMethod]
        public void AddExternalProduct_Success()
        {
            //Arrange
            string productCode = "CHEETOS123";

            //Act
            bool result = false;
            int totalProducts = _orderController.GetCountOfProduct(productCode);
            if (totalProducts > 0)
            {
                var newProductOrder = new InternalOrderProduct()
                {
                    amount = 1,
                    isConfirmed = 0,
                    internalOrderId = "020624-89C6",
                    productId = productCode
                };
                result = _orderController.AddProductToOrder(newProductOrder);
            }

            //Assert
            Assert.IsTrue(result, "El añadir un producto externo debería ser exitoso");
        }

    }
}
