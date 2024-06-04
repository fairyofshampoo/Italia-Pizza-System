using ItaliaPizza.UserInterfaceLayer.Controllers;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using Microsoft.VisualStudio.TestTools.UnitTesting;


namespace ItaliaPizzaTest.OrdersModule
{
    [TestClass]
    public class RegisterOrderTest
    {
        OrderController _orderController = new OrderController();

        private string internalOrderCode;

        [TestMethod]
        public void RegisterInternalOrder_Success()
        {
            //Arrange
            string orderCode = _orderController.createOrderCode();
            string waiterEmail = "monita@gmail.com";

            //Act
            bool result = false;
            if (_orderController.RegisterOrder(orderCode, waiterEmail))
            {
                result = true;
                internalOrderCode = orderCode;
            }

            //Assert
            Assert.IsTrue(result, "El registro de la orden debería ser exitosa");
        }

        [TestMethod]
        public void AddInternalProduct_Success()
        {
            //Arrange
            int recipeProduct = 19;
            OrderDAO internalOrderDAO = new OrderDAO();
            int productsPosibles = _orderController.GetNumberOfPorducts(recipeProduct);

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
