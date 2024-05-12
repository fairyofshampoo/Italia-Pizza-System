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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{

    public partial class ShowProductsByOrderView : Page
    {
        private int rowAdded = 0;
        private int columnsAdded = 0;

        public string internalOrderCode;

        public ShowProductsByOrderView()
        {
            InitializeComponent();
            List<InternalOrderProduct> productsByOrder = GetProductsByOrder();
            ShowProducts(productsByOrder);
        }

        private void ShowProducts(List<InternalOrderProduct> products)
        {
            rowAdded = 0;
            columnsAdded = 0;
            ProductsGrid.Children.Clear();
            ProductsGrid.RowDefinitions.Clear();
            ProductsGrid.ColumnDefinitions.Clear();

            if (products.Any())
            {
                foreach (InternalOrderProduct product in products)
                {
                    for (int index = 0; index < 4; index++)
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

                    AddProductsByOrder(product);
                }
            }
        }

        private void AddProductsByOrder(InternalOrderProduct product)
        {
            ProductByOrderUC productCard = new ProductByOrderUC();
            Grid.SetRow(productCard, rowAdded);
            Grid.SetColumn(productCard, columnsAdded);
            productCard.SetData(product);
            ProductsGrid.Children.Add(productCard);
            columnsAdded++;
        }

        private List<InternalOrderProduct> GetProductsByOrder()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<InternalOrderProduct> products = internalOrderDAO.GetAllInternalProductsByOrden(internalOrderCode);
            return products;
        }

        private void BtnChangeStatusToInPreparation_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusOrder(2);
            btnChangeStatusToInPreparation.Visibility = Visibility.Collapsed;
            btnChangeStatusToFinished.Visibility = Visibility.Visible;
        }

        private void BtnChangeStatusToFinished_Click(object sender, RoutedEventArgs e)
        {
            ChangeStatusOrder(3);
            btnChangeStatusToFinished.IsEnabled = false;
        }

        private void ChangeStatusOrder(int status)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            if (internalOrderDAO.ChangeOrderStatus(status, internalOrderCode))
            {
                //Mostrar mensaje que se ha cambiado el estado
            }
            else
            {
                //Mostrar mensaje que ha ocurrido un error
            }
        }
    }
}
