using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
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
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    public partial class ProductRemoveUC : UserControl
    {
        public Product ProductData { get; set; }
        public RegisterOrderView RegisterOrderView { get; set; }
        public ProductRemoveUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            int fontSize = 20;
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtPrice.FontWeight = FontWeights.Bold;
            txtAmount.FontSize = fontSize;
            txtName.FontSize = fontSize;
            txtPrice.FontSize = fontSize;
            txtAmount.Visibility = Visibility.Visible;
            btnRemoveProduct.Visibility = Visibility.Hidden;
        }

        public void SetProductData(Product product)
        {
            txtAmount.Visibility = Visibility.Visible;
            ProductData = product;
            txtName.Text = product.name;
            txtAmount.Text = product.amount.ToString();
            txtPrice.Text = product.price.ToString();
        }

        private void BtnRemoveProduct_Click(object sender, RoutedEventArgs e)
        {
            RegisterOrderView.RemoveProduct(ProductData, this);
        }
    }
}
