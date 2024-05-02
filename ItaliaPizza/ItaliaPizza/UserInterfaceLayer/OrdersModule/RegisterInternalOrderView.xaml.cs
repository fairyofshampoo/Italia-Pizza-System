using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        /*
        private int rowAdded = 0;
        private int columnsAdded = 0;
        public List<Product> productsSelected;
        private string orderCode;
        private string waiterName; //Esto debe ser con el singleton

        public RegisterInternalOrderView()
        {
            InitializeComponent();
            List<Product> products = GetProducts();
            if(products.Any())
            {
                RegisterInternalOrder(products);
            }else
            {
                //Mostrar mensaje de que no hay prodcutos registrados en la base
            }
        }

        private void RegisterInternalOrder(List<Product> products)
        {
            CreateOrderCode();
            InternalOrder internalOrder = CreateInternalOrder (orderCode);
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            if(internalOrderDAO.AddInternalOrder(internalOrder))
            {
                ShowProducts(products);
            } else
            {
                //Mostrar mensaje avisando que ha ocurrido un problema sql
            }
        }

        private InternalOrder CreateInternalOrder(string orderCode)
        {
            DateTime dateTime = DateTime.Now;
            TimeSpan time = dateTime.TimeOfDay;
            var newInternalOrder = new InternalOrder
            {
                internalOrderId = orderCode, //Cambiar en la base de datos el tipo 
                status = "Open",
                date = dateTime,
                time = time,
                total = 0,
                waiterName = waiterName
            };
            return newInternalOrder;
        }

        private void CreateOrderCode()
        {
            DateTime date = DateTime.Now;
            Random random = new Random();
            string randomChar = RandomString(random, 4);
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            string code = string.Empty;
            do
            {
                code = $"{date:yyyyMMddHHmmss}-{randomChar}";
            } while (internalOrderDAO.IsInternalOrderCodeAlreadyExisting(code));
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
            productCard.RegisterInternalOrderView = this;
            productCard.InternalOrderCode = orderCode;
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
            //Elimina el internalOrder
            //Elimina todos los productos que ya se hayan registrado
            //Aumenta el supply con los ingredientes que fueron reducidos
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            //Modifica el total del InternalOrder

        }

        private void txtSearchBarChanged(object sender, TextChangedEventArgs e)
        {

        }

        public void OpenDialogForAddProduct(string productId)
        {
            Window mainWindow = Window.GetWindow(this);
            RegisterProductToOrderView registerProduct = new RegisterProductToOrderView();
            registerProduct.orderCode = orderCode;
            registerProduct.productId = productId;
            registerProduct.Owner = mainWindow;
            registerProduct.WindowStartupLocation = WindowStartupLocation.CenterOwner;
            registerProduct.RegisterInternalOrderView = this;
            registerProduct.ShowDialog();
        }
        */
    }
    
}
