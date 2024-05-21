using ItaliaPizza.ApplicationLayer;
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

        public RegisterOrderView RegisterInternalOrderView { get; set; }

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
                DialogManager.ShowWarningMessageBox("No hay suficientes ingredientes para agregar este producto.");
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
                DialogManager.ShowWarningMessageBox("No hay suficientes ingredientes para agregar este producto.");
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
                ProductData.amount = 1;
                RegisterInternalOrderView.AddProduct(ProductData, this);
            }
            else
            {
                DialogManager.ShowErrorMessageBox("No se ha podido agregar el produto a la orden. Intente nuevamente");
            }
        }

        private void IncreaseAmount()
        {
            OrderDAO orderDAO = new OrderDAO();
            orderDAO.IncreaseAmount(ProductData.productCode, InternalOrderCode);
            RegisterInternalOrderView.IncreaseProductAmount(ProductData);
        }

        private bool IsRegisterInDB ()
        {
            OrderDAO orderDAO = new OrderDAO();
            bool isRegister = orderDAO.IsRegisterInDatabase(ProductData.productCode, InternalOrderCode);
            return isRegister;
        }

        private InternalOrderProduct CreateInternalOrderProduct()
        {
            InternalOrderProduct newOrderProduct = new InternalOrderProduct
            {
                amount = 1,
                isConfirmed = 0, 
                internalOrderId = InternalOrderCode, 
                productId = ProductData.productCode,
            };

            return newOrderProduct;
        }


    }
}
