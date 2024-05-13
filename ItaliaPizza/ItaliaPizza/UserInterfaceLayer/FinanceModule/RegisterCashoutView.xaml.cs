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
                Cashout cashout = CreateCashout();
                RegisterCashout(cashout);
            } 
        }

        private bool IsDataValid()
        {
            bool isTotalValid = ValidateTotal();
            bool isDescriptionValid = ValidateDescription();
            bool areAnyRadioButtonChecked = ValidateRadioButtons();
            return isTotalValid && isDescriptionValid && areAnyRadioButtonChecked; 
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

        private Cashout CreateCashout()
        {

            int cashoutType = 0;

            if(radioButtonCashin.IsChecked == true)
            {
                cashoutType = 1;
            }


            DateTime dateTime = DateTime.Now;
            string totalString = txtTotal.Text;
            decimal total = decimal.Parse(totalString);
            var newCashout = new Cashout
            {
                date = dateTime,
                cashoutType = txtDescription.Text,
                total = total, 
            };

            return newCashout;
        }

        private void RegisterCashout(Cashout cashout)
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            if (cashoutDAO.RegisterCashout(cashout))
            {
                //Mostrar mensaje de éxito
            }
            else
            {
                //Mostrar mensaje de error
            }
        }

        private bool ValidateRadioButtons()
        {
            bool areAnyRadioButtonChecked = false;
            if(radioButtonCashin.IsChecked == true || radioButtonCashout.IsChecked == true)
            {
                areAnyRadioButtonChecked = true;
            }
            return areAnyRadioButtonChecked;
        }

        private void ClearErrorLabels()
        {
            lblDescriptionError.Visibility = Visibility.Collapsed;
            lblTotalError.Visibility = Visibility.Collapsed;
        }

        private void RadioButtonCashin_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonCashout.IsChecked = false;
        }

        private void RadioButtonCashout_Checked(object sender, RoutedEventArgs e)
        {
            radioButtonCashin.IsChecked = false;
        }
    }
}
