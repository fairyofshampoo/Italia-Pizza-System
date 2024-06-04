using ItaliaPizzaData.DataLayer.DAO;
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
using ItaliaPizza.ApplicationLayer;
using System.Text.RegularExpressions;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for ReportUC.xaml
    /// </summary>
    public partial class ReportUC : UserControl
    {
        private Supply supplyData;
        private Product productData;
        private bool isSupply;
        public bool isValid;
        public bool isDifferent;
        public InventoryReport InventoryReport {  get; set; }
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
                this.InventoryReport.suppliesDictionary.Add(supply, this);
                SetSupplyData(supply);
            }
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
            isValid = true;
            txtChangeCurrentAmount.Foreground = Brushes.Black;
            if (txtChangeCurrentAmount.Text.Length > 7)
            {
                LimitTextLength();
            }
            else
            {
                ValidateAmountInput();
            }
        }

        private void LimitTextLength()
        {
            txtChangeCurrentAmount.Text = txtChangeCurrentAmount.Text.Substring(0, 7);
            txtChangeCurrentAmount.CaretIndex = txtChangeCurrentAmount.Text.Length;
        }

        private void ValidateAmountInput()
        {
            decimal amount = GetAmount();
            decimal enteredAmount;

            if (isSupply)
            {
                if (!decimal.TryParse(txtChangeCurrentAmount.Text, out enteredAmount) ||
                    !IsDecimalValid(txtChangeCurrentAmount.Text))
                {
                    isValid = false;
                    txtChangeCurrentAmount.Foreground = Brushes.Red;
                }
            }
            else
            {
                if (!decimal.TryParse(txtChangeCurrentAmount.Text, out enteredAmount) ||
                    !IsIntegerValid(txtChangeCurrentAmount.Text))
                {
                    isValid = false;
                    txtChangeCurrentAmount.Foreground = Brushes.Red;
                }
            }

            SolidColorBrush backgroundBrush = GetBackgroundBrush(amount, enteredAmount);
            Visibility noteVisibility = GetNoteVisibility(amount, enteredAmount);

            UpdateBackgroundAndNote(noteVisibility, backgroundBrush);
        }

        private bool IsIntegerValid(string input)
        {
            return Validations.IsProductIntegerValid(input);
        }

        private bool IsDecimalValid(string input)
        {
            return Validations.IsSupplyNewAmountValid(input);
        }

        private decimal GetAmount()
        {
            decimal amount = 0;

            if (isSupply)
            {
                amount = supplyData.amount ?? 0;
            }
            else
            {
                amount = productData.amount ?? 0;
            }

            return amount;
        }

        private SolidColorBrush GetBackgroundBrush(decimal amount, decimal enteredAmount)
        {
            isDifferent = true;

            if (enteredAmount == amount)
            {
                isDifferent = false;
                return Brushes.LightGreen;
            }
            else if (enteredAmount < amount)
            {
                return GetOrangeBrush();
            }
            else
            {
                return Brushes.LightBlue;
            }
        }

        private Visibility GetNoteVisibility(decimal amount, decimal enteredAmount)
        {
            Visibility noteVisibility = Visibility.Visible;

            if (enteredAmount == amount)
            {
                noteVisibility = Visibility.Collapsed;
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
