using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
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

    public partial class RegisterProductToOrderView : Window
    {
        public string productId;

        public string orderCode;

        public RegisterInternalOrderView RegisterInternalOrderView { get; set; }

        public static string OrderCode; 

        private static readonly object _inventoryLock = new object();


        public RegisterProductToOrderView()
        {
            InitializeComponent();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void BtnAddOtherProduct_Click(object sender, RoutedEventArgs e)
        {
            lblTotalError.Visibility = Visibility.Collapsed;
            if (ValidateData())
            {
                List<RecipeSupply> recipeSupplies = GetSupply();
                if (recipeSupplies != null)
                {
                    string totalProducts = txtTotalProducts.Text;
                    int products = int.Parse(totalProducts);
                    List<string> ingredients = new List<string>();
                    for (int index = 0; index < recipeSupplies.Count(); index++)
                    {
                        recipeSupplies[index].supplyAmount = recipeSupplies[index].supplyAmount * products;
                        ingredients.Add(recipeSupplies[index].supplyId);
                    }

                    lock (_inventoryLock)
                    {
                        List<Supply> supplies = GetAmount(ingredients);
                        if(ValidateIngredientsAvailability(supplies, recipeSupplies)) //validar si la cantidad de cada uno de los ingredientes alcanza
                        { 
                            //Si hay lo suficiente agregarlo
                        } else
                        {
                            //Si no hay lo suficiente avisarle que no se puede agregar
                        }
                    }
                }
            }
        }

        private void AddProductToOrder()
        {

        }

        private bool ValidateIngredientsAvailability(List<Supply> supplies, List<RecipeSupply> recipeSupplies)
        {
            bool isValide = true;
            foreach (var recipeSupply in recipeSupplies)
            {
                int amount = supplies.FirstOrDefault(supply => supply.name == recipeSupply.supplyId)
                                     .amount;
                if(amount < recipeSupply.supplyAmount)
                {
                    isValide = false;
                    break;
                }
            }
            return isValide;
        }

        private List<Supply> GetAmount (List<string> ingredients)
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<Supply> supplies = internalOrderDAO.GetInventoryQuantitiesForIngredients(ingredients);
            return supplies;
        }

        private List<RecipeSupply> GetSupply()
        {
            InternalOrderDAO internalOrderDAO = new InternalOrderDAO();
            List<RecipeSupply> recipeSupplies = internalOrderDAO.GetSupplyForProduct(productId);
            return recipeSupplies;
        }

        private bool ValidateData()
        {
            bool isValid = false;
            string totalProducts = txtTotalProducts.Text;
            try
            {
                int products = int.Parse(totalProducts);
                isValid = true;
            } 
            catch (FormatException)
            {
                lblTotalError.Visibility = Visibility.Visible;
            }
            return isValid;
        }
    }
}
