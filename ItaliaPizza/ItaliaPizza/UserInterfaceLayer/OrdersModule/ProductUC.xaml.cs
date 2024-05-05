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
            int productLimit = GetNumberOfProducts(GetRecipeByProduct());
            int productsAvailable = 0;
            if (GetCountOfProduct())
            {
                int productsOnHold = GetNumberOfProductsOnHold();
                productsAvailable = productLimit - productsOnHold;
            } else
            {
                productsAvailable = productLimit;
            }


            if (productsAvailable > 0)
            {
                AddProduct();
            }
            else
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

        private bool GetCountOfProduct()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            bool areThereAnyRegister = internalOrderDAO.GetCounterOfProduct(ProductData.productCode);
            return areThereAnyRegister;
        }

        private void AddProduct()
        {
            if (!IsRegisterInDB())
            {
                RegisterInternalOrderProduct();
            } 
            else
            {
                IncreaseAmount();
            }
        }

        private void RegisterInternalOrderProduct()
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

        private void IncreaseAmount()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            internalOrderDAO.IncreaseAmount(ProductData.productCode, InternalOrderCode);
        }

        private bool IsRegisterInDB ()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            bool isRegister = internalOrderDAO.IsRegisterInDatabase(ProductData.productCode, InternalOrderCode);
            return isRegister;
        }

        private InternalOrderProduct CreateInternalOrderProduct()
        {
            InternalOrderProduct newInternalOrderProuct = new InternalOrderProduct
            {
                amount = 1,
                isConfirmed = 0, 
                internalOrderId = InternalOrderCode, 
                productId = ProductData.productCode,
            };

            return newInternalOrderProuct;
        }


    }
}
