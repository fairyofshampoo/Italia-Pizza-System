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
    /// Interaction logic for SupplyOrder.xaml
    /// </summary>
    public partial class SupplyOrderView : Page
    {
        public SupplyOrderView()
        {
            InitializeComponent();
            SetSuppliesInPage();
        }

        private void SetSuppliesInPage()
        {
            //Consultar supplies activos con método GetSuppliesAvailable();
            SupplyOrderAddUC supplyUC = new SupplyOrderAddUC();
            supplyUC.SetTitleData();
            suppliesListView.Items.Add(supplyUC);
            //Por cada supply activo hay que mandar a crear un SupplyOrderAddUC
        }

        private void BtnSaveOrder_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
