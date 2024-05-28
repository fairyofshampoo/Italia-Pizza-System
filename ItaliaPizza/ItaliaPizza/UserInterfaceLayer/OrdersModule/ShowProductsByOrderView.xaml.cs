using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
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
        private string internalOrderCode;
        private int StatusOrder;

        public ShowProductsByOrderView(string orderCode, int statusOrder)
        {
            InitializeComponent();
            this.internalOrderCode = orderCode;
            List<InternalOrderProduct> productsByOrder = GetProductsByOrder();
            ShowProducts(productsByOrder);
            StatusOrder = statusOrder;
            SetElements();
        }

        private void SetElements()
        {
            if (UserSingleton.Instance.Role == Constants.CHEF_ROLE)
            {
                if(StatusOrder == Constants.ORDER_STATUS_PENDING_PREPARATION)
                {
                    btnChangeStatusToInPreparation.Visibility = Visibility.Visible;
                }
                else if (StatusOrder == Constants.ORDER_STATUS_PREPARING)
                {
                    btnChangeStatusToFinished.Visibility = Visibility.Visible;
                }
            }
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
            OrderDAO internalOrderDAO = new OrderDAO();
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
            OrderDAO internalOrderDAO = new OrderDAO();
            if (internalOrderDAO.ChangeOrderStatus(status, internalOrderCode))
            {
                if (StatusOrder == Constants.ORDER_STATUS_PENDING_PREPARATION)
                {
                    btnChangeStatusToInPreparation.Visibility = Visibility.Visible;
                }
                else if (StatusOrder == Constants.ORDER_STATUS_PREPARING)
                {
                    btnChangeStatusToFinished.Visibility = Visibility.Visible;
                }

                DialogManager.ShowSuccessMessageBox("Se ha cambiado el estado de la orden correctamente");
            }
            else
            {
                DialogManager.ShowSuccessMessageBox("Ha ocurrido un error al actualizar el estado de la orden. Intente nuevamente.");
            }
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
