using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.KitchenModule;
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
using System.Windows.Shapes;

namespace ItaliaPizza.UserInterfaceLayer.OrdersModule
{
    /// <summary>
    /// Lógica de interacción para RecipeProcedureView.xaml
    /// </summary>
    public partial class RecipeProcedureView : Window
    {
        int idRecipe;

        public RecipeProcedureView(string idProduct, string productName)
        {
            InitializeComponent();
            SetRecipeData(idProduct, productName);
            SetRecipeSupplies(idRecipe);
        }

        public void SetRecipeData(string productCode, string productName)
        {
            lblRecipeName.Content = productName;

            RecipeDAO recipeDAO = new RecipeDAO();
            idRecipe = recipeDAO.GetIdRecipe(productName);
            txtDescription.Text = recipeDAO.GetRecipeProcedure(idRecipe);
        }

        private void SetRecipeSupplies(int idRecipe)
        {
            SupplyDAO supplyDAO = new SupplyDAO();
            List<Supply> recipeSupplies = supplyDAO.GetRecipeSupplies(idRecipe);

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
