using ItaliaPizza.DataLayer;
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

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{

    public partial class ProductUC : UserControl
    {
        private Product productData;
        public RegisterInternalOrderView RegisterInternalOrderView { get; set; }

        public string InternalOrderCode;

        public ProductUC()
        {
            InitializeComponent();
        }

        public void SetProductData(Product product)
        {
            lblProductName.Content = product.name;
            productData = product;
        }

        private void BtnProductSelected_Click(object sender, RoutedEventArgs e)
        {
 //           RegisterInternalOrderView.OpenDialogForAddProduct(productData.);
        }

    }
}
