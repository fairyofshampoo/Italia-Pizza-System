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
    public partial class ProductByOrderUC : UserControl
    {
        public ProductByOrderUC()
        {
            InitializeComponent();
        }

        public void SetData(InternalOrderProduct product)
        {
            string productName = GetProductName(product.productId);
            lblProductName.Content = productName;
            lblTotalAmount.Content = product.amount + "pzs.";
        }

        public string GetProductName(string productId)
        {
            OrderDAO internalOrderDAO = new OrderDAO();
            string name = internalOrderDAO.GetProductName(productId);
            return name;
        }
    }
}
