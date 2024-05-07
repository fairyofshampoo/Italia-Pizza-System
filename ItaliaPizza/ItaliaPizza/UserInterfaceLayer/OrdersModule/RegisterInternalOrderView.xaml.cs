using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
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

        private string orderCode;
        private int rowAdded = 0;
        private int columnsAdded = 0;
        private string waiterEmailClass = "lalocel09@gmail.com"; //Cambiar por singleton

        public RegisterInternalOrderView(bool isAEdition, string orderCode)
        {
            InitializeComponent();
            List<Product> products = GetProducts();
            if (products.Any())
            {
                if(isAEdition)
                {
                    this.orderCode = orderCode;
                    btnCancel.Visibility = Visibility.Collapsed;
                    ShowProducts(products);
                } 
                else
                {
                    RegisterInternalOrder(products);
                }
            }
            else
            {
                //Mostrar mensaje de que hubo un error al recuperar los productos
            }
    
        }

        private List<Product> GetProducts()
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> products = productDAO.GetAllProducts();
            return products;
        }

        private void RegisterInternalOrder(List<Product> products)
        {
            CreateOrderCode();
            InternalOrder internalOrder = CreateInternalOrder();
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            if (internalOrderDAO.AddInternalOrder(internalOrder))
            {
                ShowProducts(products);
            }
            else
            {
                //Mostrar mensaje avisando que ha ocurrido un problema sql
            }
        }

        private void CreateOrderCode()
        {
            DateTime date = DateTime.Today;
            Random random = new Random();
            string randomChar = RandomString(random, 4);
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            string code = string.Empty;
            do
            {
              code = $"{date:ddMMyy}-{randomChar}";
            } while (!internalOrderDAO.IsInternalOrderCodeAlreadyExisting(code));
            orderCode = code;
        }

        private string RandomString(Random rnd, int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            StringBuilder sb = new StringBuilder(length);
            for (int index = 0; index < length; index++)
            {
                sb.Append(chars[rnd.Next(chars.Length)]);
            }
            return sb.ToString();
        }

        private InternalOrder CreateInternalOrder()
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan time = new TimeSpan(dateTime.Hour, dateTime.Minute, dateTime.Second);
            var date = dateTime.Date;

            var newInternalOrder = new InternalOrder
            {
                internalOrderId = orderCode,
                status = 0, 
                date = date,
                time = time,
                total = 0,
                waiterEmail = waiterEmailClass
            };
            return newInternalOrder;
        }

        private void ShowProducts(List<Product> products)
        {
            rowAdded = 0;
            columnsAdded = 0;
            ProductsGrid.Children.Clear();
            ProductsGrid.RowDefinitions.Clear();
            ProductsGrid.ColumnDefinitions.Clear();

            if (products.Any())
            {
                foreach (Product product in products)
                {
                    for (int index = 0; index < 3; index++)
                    {
                        ColumnDefinition column = new ColumnDefinition();
                        ProductsGrid.ColumnDefinitions.Add(column);
                    }

                    if (columnsAdded == 2)
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

        private void AddProduct(Product product)
        {
            ProductUC productCard = new ProductUC();
            productCard.RegisterInternalOrderView = this;
            productCard.InternalOrderCode = orderCode;
            Grid.SetRow(productCard, rowAdded);
            Grid.SetColumn(productCard, columnsAdded);
            productCard.SetProductData(product);
            ProductsGrid.Children.Add(productCard);
            columnsAdded++;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            internalOrderDAO.CancelInternalOrder(orderCode);
        }

        private void BtnSaveInternalOrder_Click(object sender, RoutedEventArgs e)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            int status = internalOrderDAO.SaveInternalOrder(orderCode);
            if (status == 1)
            {
                Console.WriteLine("Se ha guardao");
            }
            else
            {
                Console.WriteLine("No se ha guardao");
            }
        }

        private void BtnDeleteProduct_Click(object sender, RoutedEventArgs e)
        {

        }
    }
    
}
