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

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for InventoryReport.xaml
    /// </summary>
    public partial class InventoryReport : Page
    {
        public InventoryReport()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            //Saca dialogo de "SEGURO QUE DESEAS SALIR"
            //GoBack
        }

        private void BtnDownload_Click(object sender, RoutedEventArgs e)
        {
            //Si hay productos genera reporte con cada producto
            //Si no hay, genera reporte con mensaje de que no hay productos fecha y exportado por: ...
        }
    }
}
