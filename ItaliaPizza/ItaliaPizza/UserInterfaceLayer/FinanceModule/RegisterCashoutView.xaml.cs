using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace ItaliaPizza.UserInterfaceLayer.FinanceModule
{
    public partial class RegisterCashoutView : Page
    {
        public RegisterCashoutView()
        {
            InitializeComponent();
        }

        private void BtnSaveCashout_Click(object sender, RoutedEventArgs e)
        {
            ClearErrorLabels();
            if(IsDataValid())
            {
                DateTime dateTime = DateTime.Now;
                TimeSpan time = dateTime.TimeOfDay;
                string totalString = txtTotal.Text;
                decimal total = decimal.Parse(totalString);
                var newCashout = new Cashout 
                {
                    date = DateTime.Today, 
                    time = time, 
                    cashoutType = txtDescription.Text, 
                    total = total
                };
                CashoutDAO cashoutDAO = new CashoutDAO();
                if (cashoutDAO.RegisterCashout(newCashout))
                {
                    //Mostrar mensaje de éxito
                }
                else
                {
                    //Mostrar mensaje de error
                }
            } 
        }

        private bool IsDataValid()
        {
            bool isTotalValid = ValidateTotal();
            bool isDescriptionValid = ValidateDescription();    
            return isTotalValid && isDescriptionValid;
        }

        private bool ValidateTotal()
        {
            string totalString = txtTotal.Text;
            bool isValid = true;
            try
            {
                decimal total = decimal.Parse(totalString);
            }
            catch (FormatException)
            {
                isValid = false;
                lblTotalError.Visibility = Visibility.Visible;
            }
            return isValid;
        }

        private bool ValidateDescription()
        {
            string descriptionString = txtDescription.Text;
            Regex regex = new Regex(@"^(?!\s)[^\s].{0,29}$");
            bool isValid = regex.IsMatch(descriptionString);
            if (!isValid)
            {
                lblDescriptionError.Visibility = Visibility.Visible;
            }
            return isValid;
        }

        private void ClearErrorLabels()
        {
            lblDescriptionError.Visibility = Visibility.Collapsed;
            lblTotalError.Visibility = Visibility.Collapsed;
        }
    }
}
