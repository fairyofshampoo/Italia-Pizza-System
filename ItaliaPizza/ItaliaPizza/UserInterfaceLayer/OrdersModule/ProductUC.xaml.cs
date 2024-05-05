using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{

    public partial class ProductUC : UserControl
    {
        private Product ProductData;

        private int ProductCount;

        public RegisterInternalOrderView RegisterInternalOrderView { get; set; }

        public string InternalOrderCode;

        public ProductUC()
        {
            InitializeComponent();
        }

        public void SetProductData(Product product)
        {
            lblProductName.Content = product.name;
            ProductData = product;
        }

        private void BtnAddProduct_Click(object sender, RoutedEventArgs e)
        {
            int productCounter = GetCountOfProduct();

            int productLimit = GetNumberOfProducts(GetRecipeByProduct());
            int productsOnHold = GetNumberOfProductsOnHold();
            int productsAvailable = productLimit - productsOnHold;
            if(productsAvailable > 0)
            {
                RegisterProductToOrder();
            } else
            {
                Console.WriteLine("Ya no se pueden meter más productos"); 
                //Esto debe ser cambiado por un mensaje diciendo que no hay ingredientes
            }
        }

        private int GetRecipeByProduct()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            int recipeId = internalOrderDAO.GetRecipeIdByProduct(ProductData.productCode);
            return recipeId;
        }

        private int GetNumberOfProducts(int recipeId)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            int numberOfProducts = internalOrderDAO.GetMaximumProductsPosible(recipeId);  
            return numberOfProducts;
        }

        private int GetNumberOfProductsOnHold()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            int productsOnHold = internalOrderDAO.GetNumberOfProductsOnHold(ProductData.productCode);
            return productsOnHold;
        }

        private int GetCountOfProduct()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            int countProduct = internalOrderDAO.GetCounterOfProduct(ProductData.productCode);
            return countProduct;
        }

        private void RegisterProductToOrder()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            InternalOrderProduct internalOrderProduct = CreateInternalOrderProduct();
            if (internalOrderDAO.AddInternalOrderProduct(internalOrderProduct))
            {
                //Meterlo a la tabla
            } 
            else
            {
                //Mostrar mensaje de erro
            }
        }

        private InternalOrderProduct CreateInternalOrderProduct()
        {
            InternalOrderProduct newInternalOrderProuct = new InternalOrderProduct
            {
                amount = 1, //esto se va a quitar después
                isConfirmed = 0, 
                internalOrderId = InternalOrderCode, 
                productId = ProductData.productCode,
            };

            return newInternalOrderProuct;
        }


    }
}
