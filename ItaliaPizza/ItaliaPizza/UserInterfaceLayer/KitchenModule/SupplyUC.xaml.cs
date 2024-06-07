using ItaliaPizzaData.DataLayer;
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
    /// Lógica de interacción para SupplyProductCardUC.xaml
    /// </summary>
    public partial class SupplyUC : UserControl
    {
        public SupplyUC()
        {
            InitializeComponent();
        }

        public void SetDataCard(Supply supply)
        {
            lblSupplyName.Content = supply.name;
            lblSupplyAmount.Content = supply.amount;
            lblSupplyMeasurementUnit.Content = supply.measurementUnit;
        }
    }
}
