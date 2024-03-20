using ItaliaPizza.DataLayer.DAO;
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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplierUC.xaml
    /// </summary>
    public partial class SupplierUC : UserControl
    {
        public string supplierId;
        public SupplierUC()
        {
            InitializeComponent();
        }

        private void BtnSupplyOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnEditSupplier_Click(object sender, RoutedEventArgs e)
        {

        }

        public void SetDataCards(Supplier supplier)
        {
            lblSupplierName.Content = supplier.manager + ": " + supplier.companyName;
            lblPhoneNumber.Content = supplier.phone;
            lblEmail.Content = supplier.email;

            StringBuilder supplyAreasText = new StringBuilder();
            foreach (var supplyArea in supplier.SupplyAreas)
            {
                supplyAreasText.Append(supplyArea.area_name);
                supplyAreasText.Append(", ");
            }

            lblSupplyArea.Content = supplyAreasText.ToString();
            supplierId = supplier.email;
        }
    }
}
