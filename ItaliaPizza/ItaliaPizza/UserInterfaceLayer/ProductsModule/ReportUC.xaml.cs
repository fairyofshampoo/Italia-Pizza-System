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

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    /// <summary>
    /// Interaction logic for ReportUC.xaml
    /// </summary>
    public partial class ReportUC : UserControl
    {
        private Supply supplyData;
        private bool isNoteRequired;
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
        public void SetSupplyData(Supply supply)
        {
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
            SolidColorBrush backgroundBrush;
            Visibility noteVisibility;

            if (!decimal.TryParse(txtChangeCurrentAmount.Text, out decimal enteredAmount))
            {
                txtChangeCurrentAmount.Text = string.Empty;
                return;
            }

            if (enteredAmount == supplyData.amount)
            {
                backgroundBrush = Brushes.LightGreen;
                noteVisibility = Visibility.Collapsed;
            }
            else
            {
                noteVisibility = Visibility.Visible;
                if (enteredAmount < supplyData.amount)
                    backgroundBrush = Brushes.Red;
                else
                    backgroundBrush = GetOrangeBrush();
            }

            UpdateBackgroundAndNote(true, noteVisibility, backgroundBrush);
        }

        private void UpdateBackgroundAndNote(bool noteRequired, Visibility noteVisibility, SolidColorBrush backgroundBrush)
        {
            isNoteRequired = noteRequired;
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
