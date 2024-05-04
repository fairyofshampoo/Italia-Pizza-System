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
            int availableProducts = GetNumberOfProducts(GetRecipeByProduct());
            Console.WriteLine("SE pueden crear:" + availableProducts);
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
    }
}
