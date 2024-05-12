using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.KitchenModule;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using static System.Net.Mime.MediaTypeNames;

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
        }

        public void SetRegisterForInternalProduct()
        {
            cmbIsExternal.SelectedItem = "No";
            cmbIsExternal.IsEnabled = false;
            cmbIsExternal.Foreground = Brushes.DarkGreen;
            ChangeProductType();
        }

        private void btnContinue_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (!IsProductCodeExisting() && !IsInternalProductRecipeExisting())
                {
                    Product productData = GetProductData();
                    RecipeRegisterView recipeRegisterView = new RecipeRegisterView(productData);
                    NavigationService.Navigate(recipeRegisterView);
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("El producto ingresado ya se encuentra registrado");
                }

            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (!IsProductCodeExisting())
                {
                    if (RegisterProduct())
                    {
                        DialogManager.ShowSuccessMessageBox("Producto registrado exitosamente");
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al agregar el producto");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("El código ingresado ya se encuentra registrado");
                }

            }
        }

        private bool RegisterProduct()
        {
            Product product = GetProductData();
            ProductDAO productDAO = new ProductDAO();           
            return productDAO.AddProduct(product);
        }

        private Product GetProductData()
        {
            string name = txtName.Text;
            string code = txtCode.Text;
            decimal price = Decimal.Parse(txtPrice.Text);
            string description = txtDescription.Text;
            byte[] picture = GenerateImageBytes();

            string isExternalItem = cmbIsExternal.SelectedItem.ToString();
            string statusItem = cmbStatus.SelectedItem.ToString();
            byte isExternal;
            byte status;
            Int32 amount = Int32.Parse(txtAmount.Text);

            if (isExternalItem == "Sí")
            {
                isExternal = Constants.EXTERNAL_PRODUCT;
            }
            else
            {
                isExternal = Constants.INTERNAL_PRODUCT;
            }

            if (statusItem == "Activo")
            {
                status = Constants.ACTIVE_STATUS;
            }
            else
            {
                status = Constants.INACTIVE_STATUS;
            }

            Product product = new Product
            {
                productCode = code,
                status = status,
                amount = amount,
                description = description,
                isExternal = isExternal,
                name = name,
                price = price,
                picture = picture
            };

            return product;
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

        private void cmbIsExternal_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbIsExternal.SelectedItem != null)
            {
                ChangeProductType();
            }
        }

        private void ChangeProductType()
        {
            if (cmbIsExternal.SelectedItem.ToString() == "No")
            {
                btnContinue.IsEnabled = true;
                btnContinue.Visibility = Visibility.Visible;

                btnSave.Visibility = Visibility.Hidden;
                btnSave.IsEnabled = false;

                txtCode.IsEnabled = false;
                txtCode.Text = GenerateProductCode();

                txtAmount.Text = "0";
                txtAmount.IsEnabled = false;
            }

            if (cmbIsExternal.SelectedItem.ToString() == "Sí")
            {
                btnContinue.IsEnabled = false;
                btnContinue.Visibility = Visibility.Hidden;

                btnSave.Visibility = Visibility.Visible;
                btnSave.IsEnabled = true;

                txtAmount.Text = "1";
                txtAmount.IsEnabled = true;

                txtCode.Clear();
                txtCode.IsEnabled = true;
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

        private byte[] GenerateImageBytes()
        {
            byte[] imageBytes = null;

            if (ProductImage.Source != null)
            {
                BitmapSource bitmapSource = (BitmapSource)ProductImage.Source;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using(MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    imageBytes = stream.ToArray();
                }
            }

            return imageBytes;
        }

        private bool IsProductCodeExisting()
        {
            ProductDAO productDAO = new ProductDAO();
            string code = txtCode.Text;
            bool isCodeAlreadyExisting = productDAO.IsCodeExisting(code);
            return isCodeAlreadyExisting;
        }

        private bool IsInternalProductRecipeExisting()
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            string name = txtName.Text;
            bool isInternalProductRecipeExisting = recipeDAO.AlreadyExistRecipe(name);
            return isInternalProductRecipeExisting;
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

            if (txtCode.Text.Equals(string.Empty) || !Validations.IsProductCodeValid(txtCode.Text))
            {
                txtCode.BorderBrush = Brushes.Red;
                txtCode.BorderThickness = new Thickness(2);
                lblCodeError.Visibility = Visibility.Visible;
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

            txtCode.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtCode.BorderThickness = new Thickness(0);
            lblCodeError.Visibility = Visibility.Collapsed;

            txtPrice.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtPrice.BorderThickness = new Thickness(0);
            lblPriceError.Visibility = Visibility.Collapsed;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
