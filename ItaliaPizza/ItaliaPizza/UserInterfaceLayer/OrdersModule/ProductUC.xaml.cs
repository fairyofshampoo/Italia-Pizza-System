using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.ApplicationLayer.Management;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

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
            SetProductImage();
        }

        private void SetProductImage()
        {
            ProductData.picture = LoadProductImage(ProductData.productCode);
            if(ProductData.picture != null)
            {
                imgProduct.Source = ConvertToBitMapImage(ProductData.picture);
            } 
        }

        public static BitmapImage ConvertToBitMapImage(byte[] bytesChain)
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream(bytesChain))
            {
                stream.Seek(0, SeekOrigin.Begin);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }
        private byte[] LoadProductImage(string productCode)
        {
            ImageCacheManager imageCacheManager = new ImageCacheManager();
            if (imageCacheManager.IsImageCached(productCode))
            {
                return imageCacheManager.LoadImageFromCache(productCode);
            }
            else
            {
                byte[] imageData = LoadImageFromDatabase(productCode);
                imageCacheManager.SaveImageToCache(productCode, imageData);
                return imageData;
            }
        }

        private byte[] LoadImageFromDatabase(string productCode)
        {
            ProductDAO productDAO = new ProductDAO();
            byte[] image = productDAO.GetImageByProduct(productCode);
            return image;
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
            int recipeId = new int();
            try
            { 
                recipeId = internalOrderDAO.GetRecipeIdByProduct(ProductData.productCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

            return recipeId;
        }

        private int GetNumberOfProducts(int recipeId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int numberOfProducts = new int();
            try
            {
                numberOfProducts = internalOrderDAO.GetMaximumProductsPosible(recipeId);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


            return numberOfProducts;
        }

        private int GetNumberOfProductsOnHold()
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            int productsOnHold = new int();
            try
            {
                productsOnHold = internalOrderDAO.GetNumberOfProductsOnHold(ProductData.productCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

            return productsOnHold;
        }

        private bool GetCountOfProduct()
        {
            OrderDAO internalOrderDAO = new OrderDAO();

            bool areThereAnyRegister = false;
            
            try
            {
                areThereAnyRegister = internalOrderDAO.GetCounterOfProduct(ProductData.productCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


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
            
            try
            {
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
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private void IncreaseAmount() //Edición 
        {
            try
            {
                OrderDAO orderDAO = new OrderDAO();
                orderDAO.IncreaseAmount(ProductData.productCode, InternalOrderCode);
                RegisterInternalOrderView.IncreaseProductAmount(ProductData);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

        }

        private bool IsRegisterInDB () //Edición
        {
            OrderDAO orderDAO = new OrderDAO();
            bool isRegister = false;
            try
            {
                isRegister = orderDAO.IsRegisterInDatabase(ProductData.productCode, InternalOrderCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

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
