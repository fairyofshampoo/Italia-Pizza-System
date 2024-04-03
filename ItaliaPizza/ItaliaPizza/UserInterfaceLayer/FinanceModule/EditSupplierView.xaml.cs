using ItaliaPizza.ApplicationLayer;
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
    /// Interaction logic for EditSupplierView.xaml
    /// </summary>
    public partial class EditSupplierView : Page
    {
        public string SupplierEmail { get; set; }
        public EditSupplierView()
        {
            InitializeComponent();

            SetSupplyAreaItems();
            txtPhone.PreviewTextInput += TxtPhone_PreviewTextInput;
            SetSupplierData();
        }

        private void SetSupplierData()
        {
            throw new NotImplementedException();
        }

        private void SetSupplyAreaItems()
        {
            throw new NotImplementedException();
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
        private void TxtPhone_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            if (!Validations.IsNumber(e.Text))
            {
                e.Handled = true;
                return;
            }

            TextBox textBox = sender as TextBox;
            if (textBox.Text.Length >= 10)
            {
                e.Handled = true;
            }

        }

        private void BtnActivate_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BtnDesactivate_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
