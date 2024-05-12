using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.UsersModule;
using System;
using System.Collections.Generic;
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

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Lógica de interacción para ProductUC.xaml
    /// </summary>
    public partial class ProductUC : UserControl
    {
        private Product productData;

        public ProductsView ProductsView { get; set; }

        public ProductUC()
        {
            InitializeComponent();
        }

        public void SetDataCards(Product product)
        {
            lblProductName.Content = product.name;
            lblCode.Content = product.productCode;
            lblPrice.Content = "$" + product.price;

            if (product.status == Constants.ACTIVE_STATUS)
            {
                lblStatus.Content = "Activo";
            }
            else
            {
                lblStatus.Content = "Inactivo";
            }

            productData = product;
        }

        private void BtnEditProduct_Click(object sender, RoutedEventArgs e)
        {
            ProductEditView productEditView = new ProductEditView(productData);
            ProductsView.NavigationService.Navigate(productEditView);
        }
    }
}
