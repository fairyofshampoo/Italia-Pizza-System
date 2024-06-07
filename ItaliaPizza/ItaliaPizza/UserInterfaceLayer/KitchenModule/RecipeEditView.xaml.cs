using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
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
using static System.Runtime.CompilerServices.RuntimeHelpers;

namespace ItaliaPizza.UserInterfaceLayer.KitchenModule
{
    /// <summary>
    /// Lógica de interacción para RecipeEditView.xaml
    /// </summary>
    public partial class RecipeEditView : Page
    {
        Recipe recipeSelected;

        public RecipeEditView(Recipe recipe, List<Supply> recipeSupplies)
        {
            InitializeComponent();
            recipeSelected = recipe;
            SetModifyRecipe(recipe);
            SetRecipeSupplies(recipeSupplies);
            SetAvailableSupplies(recipeSupplies);

            if (UserSingleton.Instance.Role == Constants.CHEF_ROLE)
            {
                menuFrame.Content = new ChefMenu(this);
            }
            else
            {
                menuFrame.Content = new ManagerMenu(this);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                recipeSelected.description = txtDescription.Text;
                EditRecipe(recipeSelected);                
            }
        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea desactivar la receta? El producto asociado será también desactivado", 
                                      "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                RecipeDAO recipeDAO = new RecipeDAO();

                if (recipeDAO.ChangeStatus(recipeSelected, Constants.INACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Receta actualizada exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar la receta");
                }
            }
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea activar la receta? El producto asociado será también activado", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                List<Supply> suppliesSelected = GetSuppliesFromListBox();
                RecipeDAO recipeDAO = new RecipeDAO();

                if (AreSuppliesActive(suppliesSelected))
                {
                    if (recipeDAO.ChangeStatus(recipeSelected, Constants.ACTIVE_STATUS))
                    {
                        DialogManager.ShowSuccessMessageBox("Receta actualizada exitosamente");
                    }
                    else
                    {
                        DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar la receta");
                    }
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Al menos un suministro está inactivo");
                }
            }
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

        private void SetModifyRecipe(Recipe recipe)
        {
            txtDescription.Text = recipe.description;
            txtProductName.Text = recipe.name;

            if (recipe.status == Constants.INACTIVE_STATUS)
            {
                txtStatus.Text = "Inactiva";
                btnDesactive.IsEnabled = false;
                btnDesactive.Visibility = Visibility.Hidden;

                btnActive.IsEnabled = true;
                btnActive.Visibility = Visibility.Visible;
            }
            else
            {
                txtStatus.Text = "Activa";
            }
        }

        private void SetRecipeSupplies(List<Supply> recipeSupplies)
        {
            foreach (var supply in recipeSupplies)
            {
                SupplyUC supplyUC = new SupplyUC();
                supplyUC.Tag = supply;
                supplyUC.SetDataCard(supply);
                SuppliesSelectedListBox.Items.Add(supplyUC);
            }
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

        private void CleanSupplySelectedArea()
        {
            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            SupplySelectedGrid.Visibility = Visibility.Hidden;
            SupplySelectedGrid.IsEnabled = false;
        }

        private void SetAvailableSupplies(List<Supply> recipeSupplies)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> allSupplies = supplyDAO.GetSuppliesByStatus(true);

            var suppliesNotInRecipe = allSupplies.Where(supply => !recipeSupplies
                                     .Any(excluded => excluded.name == supply.name)).ToList();

            foreach (Supply supply in suppliesNotInRecipe)
            {
                AddSupplyToWrapPanel(supply);
            }

        }

        private void EditRecipe(Recipe recipe)
        {
            RecipeDAO recipeDAO = new RecipeDAO();

            List<Supply> suppliesSelected = GetSuppliesFromListBox();

            if (AreSuppliesActive(suppliesSelected))
            {
                List<RecipeSupply> recipeSupplies = GenerateRecipeSupplies(suppliesSelected);

                if (recipeDAO.EditRecipe(recipe))
                {
                    recipeDAO.DeleteRecipeSupplies(recipe.recipeCode);
                    recipe.RecipeSupplies = recipeSupplies;

                    if (recipeDAO.RegisterRecipeSupplies(recipe.recipeCode, recipeSupplies))
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
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error durante el registro");

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
    }
}
