using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

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
            bool areAnyRadioButtonChecked = ValidateRadioButtons();
            return isTotalValid && areAnyRadioButtonChecked; 
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
                cashoutType = (byte?)cashoutType,
                total = total, 
            };
            return newCashout;
        }

        private void RegisterCashout(Cashout cashout)
        {
            CashoutDAO cashoutDAO = new CashoutDAO();
            if (cashoutDAO.RegisterCashout(cashout))
            {
                DialogManager.ShowSuccessMessageBox("Se ha registrado exitosamente su movimiento");
            }
            else
            {
                DialogManager.ShowErrorMessageBox("Ocurrió un error al registrar su movimiento. Intente nuevamente.");
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

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
