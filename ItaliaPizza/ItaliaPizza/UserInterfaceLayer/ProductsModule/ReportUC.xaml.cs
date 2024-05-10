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
using ItaliaPizza.ApplicationLayer;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for ReportUC.xaml
    /// </summary>
    public partial class ReportUC : UserControl
    {
        private Supply supplyData;
        private bool isNoteRequired;
        private Product productData;
        private bool isSupply;
        public ReportUC()
        {
            InitializeComponent();
        }

        public void SetTitleData()
        {
            txtAmount.FontWeight = FontWeights.Bold;
            txtName.FontWeight = FontWeights.Bold;
            txtSupplyArea.FontWeight = FontWeights.Bold;
            txtUnit.FontWeight = FontWeights.Bold;
            txtCurrentAmount.FontWeight = FontWeights.Bold;
            brdCurrentAmount.Visibility = Visibility.Collapsed;
            brdNote.Visibility = Visibility.Collapsed;
        }
        public void SetObjectData(object item)
        {
            if (item is Supply supply)
            {
                SetSupplyData(supply);
            }
            else if (item is Product product && product.isExternal == 1)
            {
                SetProductData(product);
            }
        }

        private void SetProductData(Product product)
        {
            isSupply = false;
            productData = product;
            txtName.Text = product.name;
            txtAmount.Text = product.amount.ToString();
            txtSupplyArea.Text = "Producto externo";
            txtUnit.Text = "Unidad";
            txtCurrentAmount.Visibility = Visibility.Collapsed;
            brdCurrentAmount.Visibility = Visibility.Visible;
        }

        public void SetSupplyData(Supply supply)
        {
            isSupply = true;
            supplyData = supply;
            txtName.Text = supply.name;
            txtAmount.Text = supply.amount.ToString();
            txtSupplyArea.Text = supply.SupplyArea.area_name;
            txtUnit.Text = supply.measurementUnit;
            txtCurrentAmount.Visibility = Visibility.Collapsed;
            brdCurrentAmount.Visibility = Visibility.Visible;
        }

        private void CurrentAmount_TextChanged(object sender, TextChangedEventArgs e)
        {
            decimal amount = GetAmount();
            decimal enteredAmount = GetEnteredAmount();

            SolidColorBrush backgroundBrush = GetBackgroundBrush(amount, enteredAmount);
            Visibility noteVisibility = GetNoteVisibility(amount, enteredAmount);

            UpdateBackgroundAndNote(noteVisibility, backgroundBrush);
        }

        private decimal GetAmount()
        {
            decimal amount = productData.amount ?? 0;
            if (isSupply)
            {
                amount = supplyData.amount ?? 0;
            }

            return amount;
        }

        private decimal GetEnteredAmount()
        {
            if (!decimal.TryParse(txtChangeCurrentAmount.Text, out decimal enteredAmount))
            {
                txtChangeCurrentAmount.Text = string.Empty;
                return 0;
            }
            return enteredAmount;
        }

        private SolidColorBrush GetBackgroundBrush(decimal amount, decimal enteredAmount)
        {
            if (enteredAmount == amount)
            {
                return Brushes.LightGreen;
            }
            else if (enteredAmount < amount)
            {
                return Brushes.LightBlue;
            }
            else
            {
                return GetOrangeBrush();
            }
        }

        private Visibility GetNoteVisibility(decimal amount, decimal enteredAmount)
        {
            Visibility noteVisibility = Visibility.Visible;
            isNoteRequired = true;

            if (enteredAmount == amount)
            {
                noteVisibility = Visibility.Collapsed;
                isNoteRequired = false;
            }

            return noteVisibility;
        }


        private void UpdateBackgroundAndNote(Visibility noteVisibility, SolidColorBrush backgroundBrush)
        {
            brdNote.Visibility = noteVisibility;
            this.Background = backgroundBrush;
        }

        private SolidColorBrush GetOrangeBrush()
        {
            Color orangeColor = (Color)ColorConverter.ConvertFromString("#FA9500");
            return new SolidColorBrush(orangeColor);
        }

        private void Note_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtNote.Text.Length > 250)
            {
                txtNote.Text = txtNote.Text.Substring(0, 250);
                txtNote.CaretIndex = txtNote.Text.Length;
            }
        }


    }
}
