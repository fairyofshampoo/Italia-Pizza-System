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
            
            if (ProductData.isExternal == 0)
            {
                RegisterInternalProduct();
            }
            else
            {
                RegisterExternalProduct();
            }


        }

        private void RegisterInternalProduct()
        {
            int productsAvailable = 0;
            int productLimit = GetNumberOfProducts(GetRecipeByProduct());
            if (GetCountOfProduct())
            {
                int productsOnHold = GetNumberOfProductsOnHold();
                productsAvailable = productLimit - productsOnHold;
            }
            else
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

        private void RegisterExternalProduct()
        {
            int productsAvailable = 0;
            int totalProducts = GetNumberOfExternalProducts();
            if (GetCountOfProduct())
            {
                int productsOnHold = GetNumberOfProductsOnHold();
                productsAvailable = totalProducts - productsOnHold;
            } else
            {
                productsAvailable = totalProducts;
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

        private int GetNumberOfExternalProducts()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int productsAvailable = internalOrderDAO.GetTotalExternalProduct(ProductData.productCode);
            return productsAvailable;
        }

        private int GetRecipeByProduct()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int recipeId = internalOrderDAO.GetRecipeIdByProduct(ProductData.productCode);
            return recipeId;
        }

        private int GetNumberOfProducts(int recipeId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int numberOfProducts = internalOrderDAO.GetMaximumProductsPosible(recipeId);  
            return numberOfProducts;
        }

        private int GetNumberOfProductsOnHold()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int productsOnHold = internalOrderDAO.GetNumberOfProductsOnHold(ProductData.productCode);
            return productsOnHold;
        }

        private bool GetCountOfProduct()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
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
            OrderDAO internalOrderDAO = new OrderDAO();
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
            OrderDAO internalOrderDAO = new OrderDAO();
            internalOrderDAO.IncreaseAmount(ProductData.productCode, InternalOrderCode);
        }

        private bool IsRegisterInDB ()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
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
