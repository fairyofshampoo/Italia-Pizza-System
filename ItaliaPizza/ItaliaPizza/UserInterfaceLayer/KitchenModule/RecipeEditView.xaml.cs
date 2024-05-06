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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.KitchenModule
{
    /// <summary>
    /// Lógica de interacción para RecipeEditView.xaml
    /// </summary>
    public partial class RecipeEditView : Page
    {
        public RecipeEditView(Recipe recipe, List<Supply> recipeSupplies)
        {
            InitializeComponent();
            SetModifyRecipe(recipe);
            SetRecipeSupplies(recipeSupplies);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {

        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {

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

        private void SetModifyRecipe(Recipe recipe)
        {
            txtDescription.Text = recipe.description;
            txtProductName.Text = recipe.name;

            if (recipe.status == Constants.INACTIVE_STATUS)
            {
                txtStatus.Text = "Inactiva";
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
            System.Windows.Controls.CheckBox check = new System.Windows.Controls.CheckBox();
            check.Content = supply.name;
            check.Margin = new Thickness(10);
            check.Tag = supply;
            check.Checked += CheckBox_Checked;
            check.Unchecked += CheckBox_Unchecked;
            AvailableSuppliesWrapPanel.Children.Add(check);
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
       
        private void CleanSupplySelectedArea()
        {
            txtAmount.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtAmount.BorderThickness = new Thickness(0);
            SupplySelectedGrid.Visibility = Visibility.Hidden;
            SupplySelectedGrid.IsEnabled = false;
        }

        private void SetAvailableSupplies()
        {
            
        }

        private void EditRecipe(Recipe recipe)
        {
            
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
