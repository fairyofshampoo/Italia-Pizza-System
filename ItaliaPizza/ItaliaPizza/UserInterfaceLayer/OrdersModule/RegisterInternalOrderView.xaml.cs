using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
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
    public partial class RegisterInternalOrderView : Page
    {

        private int rowAdded = 0;
        private int columnsAdded = 0;

        public RegisterInternalOrderView()
        {
            InitializeComponent();
            List<Product> products = GetProducts();
            if(products.Any())
            {
                ShowProducts(products);
            }else
            {

            }
        }

        private void ShowProducts(List<Product> products)
        {
            rowAdded = 0;
            columnsAdded = 0;
            ProductsGrid.Children.Clear();
            ProductsGrid.RowDefinitions.Clear();
            ProductsGrid.ColumnDefinitions.Clear();

            if(products.Any())
            {
                foreach(Product product in products)
                {
                    for (int i = 0; i < 3; i++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        ProductsGrid.ColumnDefinitions.Add(column);
                    }

                    if (columnsAdded == 3)
                    {
                        columnsAdded = 0;
                        rowAdded++;
                        RowDefinition row = new RowDefinition();
                        ProductsGrid.RowDefinitions.Add(row);
                    }

                    AddProduct(product);
                }
            }

        }

        private void AddProduct (Product product)
        {
            ProductUC productCard = new ProductUC();
            Grid.SetRow(productCard, rowAdded);
            Grid.SetColumn(productCard, columnsAdded);
            productCard.SetProductData(product);
            ProductsGrid.Children.Add(productCard);
            columnsAdded++;
        }

        private List<Product> GetProducts()
        {
            List<Product> products = new List<Product>();
            ProductDAO productDAO = new ProductDAO();
            products = productDAO.GetAllProducts();
            return products;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void txtSearchBarChanged(object sender, TextChangedEventArgs e)
        {

        }
    }
}
