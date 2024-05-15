using ItaliaPizza.ApplicationLayer;
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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    /// <summary>
    /// Interaction logic for SupplyOrderRemoveUC.xaml
    /// </summary>
    public partial class SupplyOrderRemoveUC : UserControl
    {
        public SupplyOrderView SupplyOrderView { get; set; }
        public Supply supplyData;
        public SupplyOrderRemoveUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            int fontSize = 20;
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            txtAmount.FontSize = fontSize;
            txtName.FontSize = fontSize;
            txtUnit.FontSize = fontSize;
            txtAmount.Visibility = Visibility.Visible;
            brdChangeAmount.Visibility = Visibility.Collapsed;
            btnRemoveSupply.Visibility = Visibility.Hidden;
        }

        public void SetSupplyData(Supply supply)
        {
            txtAmount.Visibility = Visibility.Collapsed;
            supplyData = supply;
            txtName.Text = supply.name;
            txtChangeAmount.Text = supply.amount.ToString();
            txtUnit.Text = supply.measurementUnit;
        }

        private void BtnRemoveSupply_Click(object sender, RoutedEventArgs e)
        {
            RemoveSupply();
        }

        private void RemoveSupply()
        {
            this.SupplyOrderView.RemoveSupplyFromOrder(supplyData.name, this);
        }

        private void TxtChangeAmountChanged(object sender, TextChangedEventArgs e)
        {
            ResetTextBoxColors();

            string amountText = txtChangeAmount.Text;

            if (Validations.IsSupplyAmountValid(amountText))
            {
                supplyData.amount = decimal.Parse(amountText);
                txtAmount.Text = txtChangeAmount.Text;
            }
            else
            {
                SetTextBoxColorsToRed();
            }
        }

        private void ResetTextBoxColors()
        {
            txtAmount.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4C4C4C"));
            txtName.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4C4C4C"));
            txtUnit.Foreground = (SolidColorBrush)(new BrushConverter().ConvertFrom("#4C4C4C"));
            txtChangeAmount.Foreground = Brushes.Black;
        }

        private void SetTextBoxColorsToRed()
        {
            txtName.Foreground = Brushes.Red;
            txtUnit.Foreground = Brushes.Red;
            txtChangeAmount.Foreground = Brushes.Red;
        }
    }
}
