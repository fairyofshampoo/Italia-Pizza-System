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
    /// Lógica de interacción para RecipesView.xaml
    /// </summary>
    public partial class RecipesView : Page
    {
        List<Recipe> recipesList;
        List<Supply> recipeSupplies;

        public RecipesView()
        {
            InitializeComponent();
            SetRecipesInComboBox();
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            Recipe recipeSelected = cmbRecipeName.SelectedItem as Recipe;
            RecipeEditView recipeEditView = new RecipeEditView(recipeSelected, recipeSupplies);
            NavigationService.Navigate(recipeEditView);
        }

        private void cmbRecipeName_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Recipe recipeSelected = cmbRecipeName.SelectedItem as Recipe;
            SetRecipeData(recipeSelected);
            RecipesSuppliesListBox.Items.Clear();
            SetRecipeSupplies(recipeSelected);
        }

        private void SetRecipesInComboBox()
        {
            RecipeDAO recipeDAO = new RecipeDAO();
            recipesList = recipeDAO.GetRecipes();

            if (recipesList != null)
            {
                cmbRecipeName.ItemsSource = recipesList;
                cmbRecipeName.DisplayMemberPath = "name";
            }
            else
            {
                DialogManager.ShowErrorMessageBox("No hay recetas registradas");
            }
            
        }

        private void SetRecipeData(Recipe recipeSelected)
        {
            txtDescription.Text = recipeSelected.description.ToString();

            if (recipeSelected.status == Constants.INACTIVE_STATUS)
            {
                txtStatus.Text = "Inactiva";
            }
            else
            {
                txtStatus.Text = "Activa";
            }
        }

        private void SetRecipeSupplies(Recipe recipeSelected)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            recipeSupplies = supplyDAO.GetRecipeSupplies(recipeSelected.recipeCode);

            foreach (var supply in recipeSupplies)
            {
                SupplyUC supplyUC = new SupplyUC();
                supplyUC.Tag = supply;
                supplyUC.SetDataCard(supply);
                RecipesSuppliesListBox.Items.Add(supplyUC);
            }            
        }
    }
}
