using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO;
using Microsoft.Win32;
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
    /// Lógica de interacción para ProductRegisterView.xaml
    /// </summary>
    public partial class ProductRegisterView : Page
    {
        public ProductRegisterView()
        {
            InitializeComponent();
            SetComboBoxItems();
            txtCode.Text = GenerateProductCode();
        }



        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {

            }
        }

        private void cmbIsExternal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void btnSelectImage_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog()
            {
                Filter = "Image files (*.png;*.jpeg;*.jpg;*.bmp)|*.png;*.jpeg;*.jpg;*.bmp",
                Title = "Selecciona una imagen de producto"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                string imagePath = openFileDialog.FileName;
                BitmapImage productImage = new BitmapImage(new Uri(imagePath));
                ProductImage.Source = productImage;
            }
        }

        private void SetComboBoxItems()
        {
            cmbIsExternal.ItemsSource = new String[]
            {
                "Sí", "No"
            };

            cmbStatus.ItemsSource = new String[]
            {
                "Activo", "Inactivo"
            };
        }

        private string GenerateProductCode()
        {
            ProductDAO productDAO = new ProductDAO();

            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            string code;
            do
            {
                code = new string(Enumerable.Repeat(chars, 5)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
            } while (productDAO.IsCodeExisting(code));

            return code;
        }

        private bool ValidateFields()
        {
            bool validateFields = true;
            int amountValue = 0;
            decimal price = 0;

            if (txtName.Text.Equals(string.Empty) || !Validations.IsProductNameValid(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                txtName.BorderThickness = new Thickness(2);
                lblNameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtDescription.Text.Equals(string.Empty))
            {
                txtDescription.BorderBrush = Brushes.Red;
                txtDescription.BorderThickness = new Thickness(2);
                lblDescriptionError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtAmount.Text.Equals(string.Empty) || (!Int32.TryParse(txtAmount.Text, out amountValue)) || amountValue < 0)
            {
                txtAmount.BorderBrush = Brushes.Red;
                txtAmount.BorderThickness = new Thickness(2);
                lblAmountError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtPrice.Text.Equals(string.Empty) || (!Decimal.TryParse(txtPrice.Text, out price)) || price < 0)
            {
                txtPrice.BorderBrush = Brushes.Red;
                txtPrice.BorderThickness = new Thickness(2);
                lblPriceError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbIsExternal.SelectedItem == null)
            {
                cmbIsExternal.BorderBrush = Brushes.Red;
                cmbIsExternal.BorderThickness = new Thickness(2);
                lblIsExternalError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                cmbStatus.BorderBrush = Brushes.Red;
                cmbStatus.BorderThickness = new Thickness(2);
                lblStatusError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            return validateFields;
        }

        private void ResetFields()
        {
            txtName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtName.BorderThickness = new Thickness(0);
            lblNameError.Visibility = Visibility.Collapsed;

            cmbIsExternal.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbIsExternal.BorderThickness = new Thickness(0);
            lblIsExternalError.Visibility = Visibility.Collapsed;

            cmbStatus.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbStatus.BorderThickness = new Thickness(0);
            lblStatusError.Visibility = Visibility.Collapsed;

            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            lblAmountError.Visibility = Visibility.Collapsed;

            txtDescription.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtDescription.BorderThickness = new Thickness(0);
            lblDescriptionError.Visibility = Visibility.Collapsed;
        }
    }
}
