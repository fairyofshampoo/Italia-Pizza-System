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

namespace ItaliaPizza.UserInterfaceLayer.KitchenModule
{
    /// <summary>
    /// Lógica de interacción para RecipeRegisterView.xaml
    /// </summary>
    public partial class RecipeRegisterView : Page
    {
        public RecipeRegisterView(Product product)
        {
            InitializeComponent();
            SetProductInfo(product.name);
        }

        private void btnAddSupply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDeleteSelectedSupply_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void SetProductInfo(String productName)
        {
            txtProductName.Text = productName;
        }
    }
}
