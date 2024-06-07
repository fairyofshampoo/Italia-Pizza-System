using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using ItaliaPizza.UserInterfaceLayer.OrdersModule;
using System;
using System.Net;
using System.Security;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using ItaliaPizza.UserInterfaceLayer.Controllers;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class LoginView : Page
    {
        private readonly EmployeeController _employeeController = new EmployeeController();
        public LoginView()
        {
            InitializeComponent();
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            if (HandleLoginAttempt())
            {
                SaveSession();
                DisplayMainMenuView();
            }
        }

        private void DisplayMainMenuView()
        {
            string userRole = UserSingleton.Instance.Role;
            switch (userRole)
            {
                case Constants.CHEF_ROLE:
                    SearchInternalOrderView ordersChefView = new SearchInternalOrderView(false);
                    NavigationService.Navigate(ordersChefView);
                    break;
                case Constants.CASHIER_ROLE:
                    TellerScreenView tellerScreenView = new TellerScreenView();
                    NavigationService.Navigate(tellerScreenView);
                    break;
                case Constants.WAITER_ROLE:
                    SearchInternalOrderView ordersWaiterView = new SearchInternalOrderView(true);
                    NavigationService.Navigate(ordersWaiterView);
                    break;
                case Constants.MANAGER_ROLE:
                    SuppliersView suppliersView = new SuppliersView();
                    NavigationService.Navigate(suppliersView);
                    break;
                default:
                    break;
            }
        }


        private bool HandleLoginAttempt()
        {
            bool continueLogin = _employeeController.VerifyLoginFields(txtUsername.Text, GetPassword());
            if (continueLogin)
            {
                string username = txtUsername.Text;
                string password = GetPassword();
                string passwordHashed = Encription.ToSHA2Hash(password);
                continueLogin = _employeeController.AuthenticateUser(username, passwordHashed);

                if (continueLogin == false)
                {
                    DialogManager.ShowWarningMessageBox("Credenciales inválidas");
                }

            } else
            {
                DialogManager.ShowWarningMessageBox("Los datos ingresados son erroneos, no cumplen el formato solicitado");
            }

            return continueLogin;
        }

        private string GetPassword()
        {
            bool isChecked = tgbtnPasswordVisibility.IsChecked ?? false;
            string password = txtPasswordDisplay.Text;

            if (!isChecked)
            {
                SecureString passwordToAccess = pwbPassword.SecurePassword;
                password = new NetworkCredential(string.Empty, passwordToAccess).Password;
            }

            return password;
        }

        private void SaveSession()
        {
            string username = txtUsername.Text;
            Account account = _employeeController.GetAccountData(username);

            if(account != null){
                UserSingleton.Instance.Initialize(account);
            }
        }

        private void TgbtnPasswordVisibility_Checked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(true);
        }

        private void TgbtnPasswordVisibility_Unchecked(object sender, RoutedEventArgs e)
        {
            TogglePasswordVisibility(false);
        }

        private void TogglePasswordVisibility(bool isVisible)
        {
            if (isVisible)
            {
                gridPassword.Visibility = Visibility.Collapsed;
                gridPasswordDisplay.Visibility = Visibility.Visible;
                gridPasswordDisplay.IsEnabled = true;
            }
            else
            {
                gridPassword.Visibility = Visibility.Visible;
                gridPasswordDisplay.Visibility = Visibility.Collapsed;
                gridPasswordDisplay.IsEnabled = false;
            }

            if (isVisible)
            {
                txtPasswordDisplay.Text = pwbPassword.Password;
                SetPasswordIcon("InvisibleIcon.png");
            }
            else
            {
                pwbPassword.Password = txtPasswordDisplay.Text;
                SetPasswordIcon("VisibleIcon.png");
            }
        }

        private void SetPasswordIcon(string iconPassword)
        {
            try
            {
                Image imgPasswordIcon = tgbtnPasswordVisibility.Template.FindName("imgPasswordIcon", tgbtnPasswordVisibility) as Image;

                if (imgPasswordIcon != null)
                {
                    imgPasswordIcon.Source = new BitmapImage(new Uri($"/UserInterfaceLayer/Resources/Icons/{iconPassword}", UriKind.Relative));
                }
            }
            catch (UriFormatException)
            {
                DialogManager.ShowErrorMessageBox("No se pudo encontrar el archivo de iconos para contraseña");
            }
        }
    }
}
