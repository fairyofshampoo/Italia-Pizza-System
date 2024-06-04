using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    public partial class RegisterOrderView : Page
    {

        public string orderCode;
        public bool isEdition = false;
        private bool isHomeOrder;
        private int rowAdded = 0;
        private int columnsAdded = 0;
        private string waiterEmail = UserSingleton.Instance.Email;
        private Dictionary<string, ProductRemoveUC> productsDictionary = new Dictionary<string, ProductRemoveUC>();
        private List<Product> productsInDB = new List<Product>();
        private decimal totalAmount;
        private Client clientData = new Client();
        private InternalOrdersUC internalOrdersUC;

        public RegisterOrderView(bool isHomeOrder)
        {
            this.isHomeOrder = isHomeOrder;
            InitializeComponent();

            
            SetResumeOrderTitle();
            if (!isHomeOrder)
            {
                List<Product> products = GetProducts();
                SetProductsInPage(products);
            }
        }

        public void SetEditionData(InternalOrder order)
        {
            isEdition = true;
            OrderDAO orderDAO = new OrderDAO();
            orderCode = order.internalOrderId;
            orderDAO.ChangeOrderStatus(Constants.ORDER_STATUS_IN_CAPTURE, orderCode);
            totalAmount = order.total;
            txtTotalPayment.Text = order.total.ToString();
            SetOrderProductsInResume();
            ShowProducts(GetProducts());

            if (isHomeOrder)
            {
                AddressDAO addressDAO = new AddressDAO();
                ClientDAO clientDAO = new ClientDAO();
                addressComboBox.Visibility = Visibility.Visible;
                addressComboBox.SelectedValue = addressDAO.GetAddressById(order.addressId ?? 0);
                addressComboBox.IsEnabled = false;
                lblName.Text = clientDAO.GetClientName(order.clientEmail);
            } else
            {
                addressComboBox.Visibility = Visibility.Collapsed;
                lblName.Text = UserSingleton.Instance.Name;
            }
        }

        private void SetOrderProductsInResume()
        {
            OrderDAO orderDAO = new OrderDAO();
            productsInDB = orderDAO.GetProductsByOrderId(this.orderCode);

            foreach (Product product in productsInDB)
            {
                product.amount = orderDAO.GetOrderedQuantityByProductOrderId(orderCode, product.productCode);
                AddProductToResume(product);
            }
        }

        private void AddProductToResume(Product product)
        {
            if (!productsDictionary.ContainsKey(product.productCode))
            {
                ProductRemoveUC productCard = new ProductRemoveUC();
                productsDictionary.Add(product.productCode, productCard);
                productCard.RegisterOrderView = this;
                productCard.SetProductData(product);
                orderListView.Items.Add(productCard);
            }
        }

        public void SetClientData (Client clientData)
        {
            List<Product> products = GetProducts();
            this.clientData = clientData;
            SetProductsInPage(products);
        }

        private void SetHomeOrderElements()
        {
            lblName.Text = clientData.name;
            lblOrderCode.Content = "Pedido " + orderCode;
            SetAddressInComboBox();
            addressComboBox.Visibility = Visibility.Visible;
        }
        private void SetResumeOrderTitle()
        {
            orderListView.Items.Clear();
            ProductRemoveUC productCard = new ProductRemoveUC();
            productCard.RegisterOrderView = this;
            productCard.SetTitleData();
            orderListView.Items.Add(productCard);
        }

        private void SetAddressInComboBox()
        {
            AddressDAO addressDAO = new AddressDAO();
            List<Address> addresses = addressDAO.GetAddressesByClient(clientData.email);

            if (addresses.Any())
            {
                addressComboBox.ItemsSource = addresses;
                addressComboBox.DisplayMemberPath = "street";
            } else
            {
                DialogManager.ShowErrorMessageBox("No hay direcciones registradas");
            }
        }

        private List<Product> GetProducts()
        {
            ProductDAO productDAO = new ProductDAO();
            List<Product> products = productDAO.GetAllAvailableProducts();
            return products;
        }

        private void SetProductsInPage(List<Product> products)
        {
            CreateOrderCode();
            InternalOrder order = new InternalOrder();

            if (isHomeOrder)
            {
                SetHomeOrderElements();
                order = CreateHomeOrder();
            } else
            {
                SetInternalOrderElements();
                order = CreateInternalOrder();
            }

            OrderDAO orderDAO = new OrderDAO();
            if (orderDAO.AddOrder(order))
            {
                ShowProducts(products);
            }
            else
            {
                DialogManager.ShowDataBaseErrorMessageBox();
            }
        }

        private void SetInternalOrderElements()
        {
            lblOrderCode.Content = "Pedido " + orderCode;
            lblName.Text = UserSingleton.Instance.Name;
            addressComboBox.Visibility = Visibility.Hidden;
        }

        private void CreateOrderCode()
        {
            DateTime date = DateTime.Today;
            Random random = new Random();
            string randomChar = RandomString(random, 4);
            OrderDAO orderDAO = new OrderDAO();
            string code = string.Empty;

            do
            {
              code = $"{date:ddMMyy}-{randomChar}";
            } while (!orderDAO.IsInternalOrderCodeAlreadyExisting(code));
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
            DateTime currentDate = DateTime.Now;
            var newInternalOrder = new InternalOrder
            {
                internalOrderId = orderCode,
                status = 0,
                date = currentDate,
                total = 0,
                waiterEmail = this.waiterEmail
            };
            return newInternalOrder;
        }
        private InternalOrder CreateHomeOrder()
        {
            DateTime currentDate = DateTime.Now;
            var newInternalOrder = new InternalOrder
            {
                internalOrderId = orderCode,
                status = 0,
                date = currentDate,
                total = 0,
                clientEmail = clientData.email,
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
                        column.Width = new GridLength(335);
                        ProductsGrid.ColumnDefinitions.Add(column);
                    }

                    if (columnsAdded == 2)
                    {
                        columnsAdded = 0;
                        rowAdded++;
                        RowDefinition row = new RowDefinition();
                        row.Height = new GridLength(245);
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
            CancelOrder();
        }

        private void CancelOrder()
        {
            OrderDAO orderDAO = new OrderDAO();
            orderDAO.CancelOrder(orderCode);
            NavigationService.GoBack();
        }
        private void SaveOrder()
        {
            OrderDAO orderDAO = new OrderDAO();
            int status = orderDAO.SaveInternalOrder(orderCode);
            if (status == 1)
            {
                DialogManager.ShowSuccessMessageBox("Se ha registrado exitosamente su pedido");
            }
            else
            {
                DialogManager.ShowErrorMessageBox("No se ha podido registrar su pedido. Intente nuevamente.");
            }
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {
            SaveOrder();
            NavigationService.GoBack();
        }

        public void RemoveProduct(Product product, ProductRemoveUC productRemoveUC)
        {
            OrderDAO orderDAO = new OrderDAO();
            if (isEdition)
            {
                if (orderDAO.RemoveProductFromOrderInEdition(product.productCode, orderCode))
                {
                    productsDictionary.Remove(product.productCode);
                    orderListView.Items.Remove(productRemoveUC);
                    SubtractFromTotal(product);
                }
            } else
            {
                if (orderDAO.RemoveProductFromOrder(product.productCode, orderCode))
                {
                    productsDictionary.Remove(product.productCode);
                    orderListView.Items.Remove(productRemoveUC);
                    SubtractFromTotal(product);
                }
            }
        }

        public void AddProduct(Product product, ProductUC productUC)
        {
            if (!productsDictionary.ContainsKey(product.productCode))
            {
                ProductRemoveUC productRemoveUC = new ProductRemoveUC();
                productsDictionary.Add(product.productCode, productRemoveUC);
                productRemoveUC.RegisterOrderView = this;
                productRemoveUC.SetProductData(product);
                orderListView.Items.Add(productRemoveUC);
                SumToTotal(product);
            }
        }

        public void IncreaseProductAmount(Product product)
        {
            ProductRemoveUC productRemoveUC = new ProductRemoveUC();
            productRemoveUC = productsDictionary[product.productCode];
            productRemoveUC.ProductData.amount = product.amount;
            product.amount += 1;
            SumToTotal(product);
            productRemoveUC.SetProductData(product);
        }

        private void SumToTotal(Product product)
        {
            decimal unit = 1;
            decimal subtotal = product.price * unit;
            totalAmount += subtotal;
            txtTotalPayment.Text = totalAmount.ToString();
        }

        private void SubtractFromTotal(Product product)
        {
            decimal subtotal = product.price * product.amount ?? 0;

            totalAmount -= subtotal;
            txtTotalPayment.Text = totalAmount.ToString();
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            if (isEdition)
            {
                NavigationService.GoBack();
            } else
            {
                CancelOrder();
            }
        }
    }

}
