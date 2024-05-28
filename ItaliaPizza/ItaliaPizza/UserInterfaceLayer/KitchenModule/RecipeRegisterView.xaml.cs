using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
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
            SetProductInfo(productData);
            SetAvailableSupplies();
        }

        private void btnAddSupply_Click(object sender, RoutedEventArgs e)
        {
            if (IsAmountSupplyValid())
            {
                System.Windows.Controls.RadioButton selectedRadioButton = AvailableSuppliesWrapPanel.Children.OfType<System.Windows.Controls.RadioButton>().
                                                                    FirstOrDefault(c => c.IsChecked == true);
                Supply selectedSupply = selectedRadioButton.Tag as Supply;
                selectedSupply.amount = decimal.Parse(txtAmount.Text);

                SupplyUC supplyUC = new SupplyUC();
                supplyUC.Tag = selectedSupply;
                supplyUC.SetDataCard(selectedSupply);
                SuppliesSelectedListBox.Items.Add(supplyUC);

                AvailableSuppliesWrapPanel.Children.Remove(selectedRadioButton);
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

        private void BtnSave_Click(object sender, RoutedEventArgs e)
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
            byte status;

            if (txtStatus.Text == "Activa")
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

        private void RegisterRecipe(Recipe recipe)
        {
            RecipeDAO recipeDAO = new RecipeDAO();

            List<Supply> suppliesSelected = GetSuppliesFromListBox();

            if (AreSuppliesActive(suppliesSelected))
            {
                List<RecipeSupply> recipeSupplies = GenerateRecipeSupplies(suppliesSelected);
                recipe.RecipeSupplies = recipeSupplies;

                if (recipeDAO.RegisterRecipeWithSupplies(recipe, product))
                {
                    DialogManager.ShowSuccessMessageBox("Registro exitoso");
                    RecipesView recipesView = new RecipesView();
                    NavigationService.Navigate(recipesView);
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error durante el registro. Intente nuevamente");
                }
            }
            else
            {
                DialogManager.ShowErrorMessageBox("Al menos un suministro está inactivo");
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
                    supply.amount = (decimal?)supplyUC.lblSupplyAmount.Content;
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

        private void SetProductInfo(Product product)
        {
            txtProductName.Text = product.name;
            
            if (product.status == Constants.ACTIVE_STATUS)
            {
                txtStatus.Text = "Activa";
            }
            else
            {
                txtStatus.Text = "Inactiva";
            }
        }

        private void SetAvailableSupplies()
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> availableSupplies = supplyDAO.GetSuppliesByStatus(true);

            foreach (Supply supply in availableSupplies)
            {
                AddSupplyToWrapPanel(supply);
            }
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.RadioButton radioButton = sender as System.Windows.Controls.RadioButton;

            if (radioButton != null && radioButton.IsChecked == true)
            {
                Supply selectedSupply = radioButton.Tag as Supply;
                ShowSupplyDetails(selectedSupply);
            }
        }

        private void RadioButton_Unchecked(object sender, RoutedEventArgs e)
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
            System.Windows.Controls.RadioButton radioButton = new System.Windows.Controls.RadioButton();
            radioButton.Content = supply.name;
            radioButton.Margin = new Thickness(10);
            radioButton.Tag = supply;
            radioButton.Checked += RadioButton_Checked;
            radioButton.Unchecked += RadioButton_Unchecked;
            AvailableSuppliesWrapPanel.Children.Add(radioButton);
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

            if (SuppliesSelectedListBox.Items.Count == 0)
            {
                SuppliesSelectedListBox.BorderBrush = Brushes.Red;
                SuppliesSelectedListBox.BorderThickness = new Thickness(2);
                validateFields = false;
            }

            return validateFields;
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
