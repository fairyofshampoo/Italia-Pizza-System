using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.ApplicationLayer.Management;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using iText.Commons.Actions.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    public partial class ProductByOrderUC : UserControl
    {
        private Product ProductData = new Product();
        public ProductByOrderUC()
        {
            InitializeComponent();
        }

        public void SetData(InternalOrderProduct product)
        {
            ProductData.name = GetProductName(product.productId);
            ProductData.productCode = product.productId;
            lblProductName.Content = ProductData.name;
            lblTotalAmount.Content = product.amount + "pzs.";
            SetRecipeButton(GetProductStatus(product.productId));
            SetProductImage();
        }

        private void SetProductImage()
        {
            ProductData.picture = LoadProductImage(ProductData.productCode);
            if (ProductData.picture != null)
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

        public void SetRecipeButton(int isExternal)
        {
            if (isExternal == Constants.EXTERNAL_PRODUCT)
            {
                btnSeeRecipe.IsEnabled = false;
                btnSeeRecipe.Visibility = Visibility.Hidden;
            }
            else
            {
                btnSeeRecipe.IsEnabled = true;
                btnSeeRecipe.Visibility = Visibility.Visible;
            }
        }

        public string GetProductName(string productId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            string name = internalOrderDAO.GetProductName(productId);
            return name;
        }

        public byte GetProductStatus(string productId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            byte status = internalOrderDAO.GetProductIsExternal(productId);
            return status;
        }

        private void BtnSeeRecipe_Click(object sender, RoutedEventArgs e)
        {
            RecipeProcedureView recipeProcedureView = new RecipeProcedureView(ProductData.productCode, ProductData.name);
            recipeProcedureView.Show();
        }
    }
}
