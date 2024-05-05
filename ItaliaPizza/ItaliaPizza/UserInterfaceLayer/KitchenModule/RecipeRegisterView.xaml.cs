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
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.KitchenModule
{
    /// <summary>
    /// Lógica de interacción para RecipeRegisterView.xaml
    /// </summary>
    public partial class RecipeRegisterView : Page
    {
        Product product;

        public RecipeRegisterView(Product productData)
        {
            InitializeComponent();
            product = productData;
            SetProductInfo(productData.name);
            SetAvailableSupplies();
            SetComboBoxStatusItems();
        }

        private void btnAddSupply_Click(object sender, RoutedEventArgs e)
        {
            if (IsAmountSupplyValid())
            {
                System.Windows.Controls.CheckBox selectedCheckBox = AvailableSuppliesWrapPanel.Children.OfType<System.Windows.Controls.CheckBox>().
                                                                    FirstOrDefault(c => c.IsChecked == true);
                Supply selectedSupply = selectedCheckBox.Tag as Supply;
                selectedSupply.amount = int.Parse(txtAmount.Text);

                SupplyUC supplyUC = new SupplyUC();
                supplyUC.Tag = selectedSupply;
                supplyUC.SetDataCard(selectedSupply);
                SuppliesSelectedListBox.Items.Add(supplyUC);

                AvailableSuppliesWrapPanel.Children.Remove(selectedCheckBox);
                CleanSupplySelectedArea();
            }
        }

        private void btnDeleteSelectedSupply_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.UserControl selectedUserControl = SuppliesSelectedListBox.SelectedItem as System.Windows.Controls.UserControl;

            if (selectedUserControl != null)
            {
                Supply supplySelected = selectedUserControl.Tag as Supply;

                AddSupplyToWrapPanel(supplySelected);

                SuppliesSelectedListBox.Items.Remove(selectedUserControl);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                Recipe recipe = GenerateRecipeObject();
                if (recipe != null)
                {
                    RegisterRecipe(recipe);
                }
            }
        }

        private Recipe GenerateRecipeObject()
        {
            string statusItem = cmbStatus.SelectedItem.ToString();
            byte status;

            if (statusItem == "Activa")
            {
                status = Constants.ACTIVE_STATUS;
            }
            else
            {
                status = Constants.INACTIVE_STATUS;
            }

            return new Recipe()
            {
                description = txtDescription.Text,
                status = status,
                name = txtProductName.Text,
                ProductId = product.productCode
            };
        }

        /* MÉTODO SIN ROLLBACK
        private void RegisterRecipe(Recipe recipe)
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            ProductDAO productDAO = new ProductDAO();

            if (productDAO.AddProduct(product))
            {
                if (!recipeDAO.AlreadyExistRecipe(recipe.name))
                {
                    if (recipeDAO.RegisterRecipe(recipe, product.productCode))
                    {
                        int idRecipe = recipeDAO.GetIdRecipe(recipe.name);
                        recipe.recipeCode = idRecipe;
                        List<Supply> suppliesSelected = GetSuppliesFromListBox();

                        if (suppliesSelected != null)
                        {
                            List<RecipeSupply> recipeSupplies = GenerateRecipeSupplies(suppliesSelected);
                            recipe.RecipeSupplies = recipeSupplies;

                            if (idRecipe > 0)
                            {
                                if (recipeDAO.RegisterRecipeSupplies(recipe))
                                {
                                    DialogManager.ShowSuccessMessageBox("Registro exitoso");
                                }
                                else
                                {
                                    DialogManager.ShowErrorMessageBox("Error al registrar los ingredientes de la receta");
                                }
                            }
                        }
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Error al registrar la receta");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("La receta ya está registrada");
                }
            }
            else
            {
                DialogManager.ShowErrorMessageBox("Error al registrar el producto");
            }
        }
        */

        private void RegisterRecipe(Recipe recipe)
        {
            RecipeDAO recipeDAO = new RecipeDAO();

            if (!recipeDAO.AlreadyExistRecipe(recipe.name))
            {
                List<Supply> suppliesSelected = GetSuppliesFromListBox();

                if (AreSuppliesActive(suppliesSelected))
                {
                    List<RecipeSupply> recipeSupplies = GenerateRecipeSupplies(suppliesSelected);
                    recipe.RecipeSupplies = recipeSupplies;

                    if (recipeDAO.RegisterRecipeWithSupplies(recipe, product))
                    {
                        DialogManager.ShowSuccessMessageBox("Registro exitoso");
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error durante el registro");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Al menos un suministro está inactivo");
                }
            }
            else
            {
                DialogManager.ShowErrorMessageBox("La receta ya está registrada");
            }
        }

        private List<Supply> GetSuppliesFromListBox()
        {
            List<Supply> suppliesSelected = new List<Supply>();

            foreach (var item in SuppliesSelectedListBox.Items)
            {
                SupplyUC supplyUC = item as SupplyUC;
                if (supplyUC != null)
                {
                    Supply supply = supplyUC.Tag as Supply;
                    supply.amount = (int)supplyUC.lblSupplyAmount.Content;
                    suppliesSelected.Add(supply);
                }
            }
            return suppliesSelected;
        }

        private List<RecipeSupply> GenerateRecipeSupplies(List<Supply> supplies)
        {
            List<RecipeSupply> recipeSupplies = new List<RecipeSupply>();
            foreach (var supply in supplies)
            {
                RecipeSupply recipeSupply = new RecipeSupply();
                recipeSupply.Supply = supply;
                recipeSupply.supplyAmount = supply.amount;
                recipeSupplies.Add(recipeSupply);
            }
            return recipeSupplies;
        }

        private bool AreSuppliesActive(List<Supply> supplies)
        {
            bool isActive = true;

            foreach (var supply in supplies)
            {
                if (supply.status != true)
                {
                    isActive = false;
                }
            }
            return isActive;
        }

        private void SetProductInfo(String productName)
        {
            txtProductName.Text = productName;
        }

        private void SetAvailableSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetAvailableSupplies();

            foreach (Supply supply in availableSupplies)
            {
                AddSupplyToWrapPanel(supply);
            }
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.CheckBox checkBox = sender as System.Windows.Controls.CheckBox;

            foreach (System.Windows.Controls.CheckBox otherCheckBox in AvailableSuppliesWrapPanel.Children.OfType<System.Windows.Controls.CheckBox>())
            {
                if (otherCheckBox != checkBox && otherCheckBox.IsChecked == true)
                {
                    otherCheckBox.IsChecked = false;
                }
            }

            if (checkBox != null && checkBox.IsChecked == true)
            {
                Supply selectedSupply = checkBox.Tag as Supply;
                ShowSupplyDetails(selectedSupply);
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            SupplySelectedGrid.Visibility = Visibility.Hidden;
            SupplySelectedGrid.IsEnabled = false;
        }

        private void ShowSupplyDetails(Supply supply)
        {
            txtAmount.Text = "";
            SupplySelectedGrid.Visibility = Visibility.Visible;
            SupplySelectedGrid.IsEnabled = true;

            lblSupplyName.Content = supply.name;
            lblMeasurementUnit.Content = supply.measurementUnit;
        }

        private void SetComboBoxStatusItems()
        {
            cmbStatus.ItemsSource = new string[]
            {
                "Activa", "Inactiva"
            };
        }

        private bool IsAmountSupplyValid()
        {
            bool isAmountValid = true;
            decimal amount = 0;

            if (txtAmount.Text.Equals(string.Empty) || !Decimal.TryParse(txtAmount.Text, out amount) || amount < 0)
            {
                txtAmount.BorderBrush = Brushes.Red;
                txtAmount.BorderThickness = new Thickness(2);
                isAmountValid = false;
            }
            return isAmountValid;
        }

        private void CleanSupplySelectedArea()
        {
            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            SupplySelectedGrid.Visibility = Visibility.Hidden;
            SupplySelectedGrid.IsEnabled = false;
        }

        private void AddSupplyToWrapPanel(Supply supply)
        {
            System.Windows.Controls.CheckBox check = new System.Windows.Controls.CheckBox();
            check.Content = supply.name;
            check.Margin = new Thickness(10);
            check.Tag = supply;
            check.Checked += CheckBox_Checked;
            check.Unchecked += CheckBox_Unchecked;
            AvailableSuppliesWrapPanel.Children.Add(check);
        }

        private bool ValidateFields()
        {
            bool validateFields = true;

            if (txtDescription.Text.Equals(string.Empty))
            {
                txtDescription.BorderBrush = Brushes.Red;
                txtDescription.BorderThickness = new Thickness(2);
                validateFields = false;
            }

            if (cmbStatus.SelectedItem == null)
            {
                cmbStatus.BorderBrush = Brushes.Red;
                cmbStatus.BorderThickness = new Thickness(2);
                validateFields = false;
            }

            if (SuppliesSelectedListBox.Items.Count == 0)
            {
                SuppliesSelectedListBox.BorderBrush = Brushes.Red;
                SuppliesSelectedListBox.BorderThickness = new Thickness(2);
                validateFields = false;
            }

            return validateFields;
        }
    }



}
