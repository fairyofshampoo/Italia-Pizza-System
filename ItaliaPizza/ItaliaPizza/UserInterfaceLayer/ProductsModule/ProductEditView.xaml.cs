﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using Microsoft.Win32;
using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Brushes = System.Windows.Media.Brushes;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
using ItaliaPizza.ApplicationLayer.Utilities;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

namespace ItaliaPizza.UserInterfaceLayer.ProductsModule
{
    public partial class ProductEditView : Page
    {
        Product ProductToModify;

        public ProductEditView(string productCode)
        {
            InitializeComponent();
            SetModifyProduct(productCode);
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

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (ModifyProduct())
                {
                    DialogManager.ShowSuccessMessageBox("Producto actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el producto");
                }
            }
        }

        private bool ModifyProduct()
        {
            string name = txtName.Text;
            decimal price = Decimal.Parse(txtPrice.Text);
            string description = txtDescription.Text;
            byte[] picture = GenerateImageBytes();
            ImageOptimizationManager optimizationManager = new ImageOptimizationManager();
            byte[] optimizedPicture = optimizationManager.OptimizeImage(picture, 50, 800);
            string code = txtCode.Text;
            
            ProductDAO productDAO = new ProductDAO();
            Product product = new Product
            {
                description = description,
                name = name,
                price = price,
                picture = optimizedPicture,
            };

            bool result = false;

            try
            {
                result = productDAO.ModifyProduct(product, code);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }

            return result;
        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea eliminar el producto?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                ProductDAO productDAO = new ProductDAO();

                try
                {
                    if (productDAO.ChangeStatus(ProductToModify, Constants.INACTIVE_STATUS))
                    {
                        DialogManager.ShowSuccessMessageBox("Producto actualizado exitosamente");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el producto");
                    }
                }
                catch (ArgumentException)
                {
                    ApplicationLayer.DialogManager.ShowArgumentExceptionMessageBox();
                }
                catch (SqlException)
                {
                    ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
                }
                catch (DbUpdateException)
                {
                    ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
                }
                catch (EntityException)
                {
                    ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
                }
                catch (InvalidOperationException)
                {
                    ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
                }

            }
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea activar el producto?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                ProductDAO productDAO = new ProductDAO();

                try
                {
                    if (productDAO.ChangeStatus(ProductToModify, Constants.ACTIVE_STATUS))
                    {
                        DialogManager.ShowSuccessMessageBox("Producto actualizado exitosamente");
                        NavigationService.GoBack();
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el producto");
                    }
                }
                catch (ArgumentException)
                {
                    ApplicationLayer.DialogManager.ShowArgumentExceptionMessageBox();
                }
                catch (SqlException)
                {
                    ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
                }
                catch (DbUpdateException)
                {
                    ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
                }
                catch (EntityException)
                {
                    ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
                }
                catch (InvalidOperationException)
                {
                    ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
                }
            }
        }

        public void SetModifyProduct(string productCode)
        {
            ProductDAO productDAO = new ProductDAO();
            Product productInfo = new Product();
            
            try
            {
                productInfo = productDAO.GetProductByCode(productCode);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }


            if (productInfo != null)
            {
                ProductToModify = productInfo;

                txtCode.Text = productInfo.productCode;
                txtAmount.Text = productInfo.amount.ToString();
                txtDescription.Text = productInfo.description;
                txtPrice.Text = productInfo.price.ToString();
                txtName.Text = productInfo.name;
                ProductImage.Source = ConvertToBitMapImage(productInfo.picture);


                if (productInfo.status == Constants.ACTIVE_STATUS)
                {
                    txtStatus.Text = "Activo";
                }
                else
                {
                    txtStatus.Text = "Inactivo";

                    txtAmount.IsEnabled = false;
                    txtDescription.IsEnabled = false;
                    txtPrice.IsEnabled = false;
                    txtName.IsEnabled = false;
                    btnSelectImage.IsEnabled = false;

                    btnDesactive.IsEnabled = false;
                    btnDesactive.Visibility = Visibility.Hidden;

                    btnActive.IsEnabled = true;
                    btnActive.Visibility = Visibility.Visible;

                    btnSave.IsEnabled = false;
                    btnSave.Background = Brushes.Gray;
                }

                if (productInfo.isExternal == Constants.EXTERNAL_PRODUCT)
                {
                    txtIsExternal.Text = "Sí";
                    txtName.IsEnabled= false;
                }
                else
                {
                    txtIsExternal.Text = "No";
                }
            }
        }

        public static BitmapImage ConvertToBitMapImage(byte[] bytesChain)
        {
            var image = new BitmapImage();
            using (var stream = new MemoryStream(bytesChain))
            {
                stream.Seek(0, SeekOrigin.Begin);
                image.BeginInit();
                image.CacheOption = BitmapCacheOption.OnLoad;
                image.StreamSource = stream;
                image.EndInit();
            }
            return image;
        }

        public static BitmapImage ConvertToBitMapImage(ImageSource bitmapSource)
        {
            var bitmapImage = new BitmapImage();

            var bitmapEncoder = new PngBitmapEncoder();
            bitmapEncoder.Frames.Add(BitmapFrame.Create(bitmapSource as BitmapSource));

            using (var stream = new MemoryStream())
            {
                bitmapEncoder.Save(stream);
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = stream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
            }
            return bitmapImage;
        }

        private byte[] GenerateImageBytes()
        {
            byte[] imageBytes = null;

            if (ProductImage.Source != null)
            {
                BitmapSource bitmapSource = (BitmapSource)ProductImage.Source;
                JpegBitmapEncoder encoder = new JpegBitmapEncoder();
                encoder.Frames.Add(BitmapFrame.Create(bitmapSource));

                using (MemoryStream stream = new MemoryStream())
                {
                    encoder.Save(stream);
                    imageBytes = stream.ToArray();
                }
            }

            return imageBytes;
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
            
            return validateFields;
        }

        private void ResetFields()
        {
            txtName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtName.BorderThickness = new Thickness(0);
            lblNameError.Visibility = Visibility.Collapsed;

            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            lblAmountError.Visibility = Visibility.Collapsed;

            txtDescription.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtDescription.BorderThickness = new Thickness(0);
            lblDescriptionError.Visibility = Visibility.Collapsed;

            txtPrice.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtPrice.BorderThickness = new Thickness(0);
            lblPriceError.Visibility = Visibility.Collapsed;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
