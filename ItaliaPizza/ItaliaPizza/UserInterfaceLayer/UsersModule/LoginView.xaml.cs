using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security;
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

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    /// <summary>
    /// Interaction logic for LoginView.xaml
    /// </summary>
    public partial class LoginView : Page
    {
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
            else
            {
                DialogManager.ShowWarningMessageBox("Los datos ingresados son erroneos");
            }
        }

        private void DisplayMainMenuView()
        {
            TellerScreenView tellerScreenView = new TellerScreenView();
            this.NavigationService.Navigate(tellerScreenView);
        }

        private bool HandleLoginAttempt()
        {
            bool continueLogin = VerifyFields();
            if (continueLogin)
            {
                continueLogin = ValidateCredentials();

            }

            return continueLogin;
        }

        private bool ValidateCredentials()
        {
            bool result = false;
            string username = txtUsername.Text;
            string password = GetPassword();
            string passwordHashed = Encription.ToSHA2Hash(password);
            EmployeeDAO employeeDAO = new EmployeeDAO();

            result = employeeDAO.AuthenticateAccount(username, passwordHashed);

            return result;
        }

        private bool VerifyFields()
        {
            string username = txtUsername.Text;
            string password = GetPassword();
            bool passwordValidation = VerifyPassword(password);
            bool gamerTagValidation = VerifyUsername(username);

            return passwordValidation && gamerTagValidation;
        }

        private bool VerifyUsername(string username)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(username))
            {
                isValid = false;
            }

            return isValid;
        }

        private bool VerifyPassword(string password)
        {
            bool isValid = true;
            if (string.IsNullOrEmpty(password))
            {
                isValid = false;
            }

            return isValid;
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
            Account account = GetAccountData(username);

            if(account != null){
                UserSingleton.Instance.Initialize(account);
            }
        }

        private Account GetAccountData(String username)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Account account = employeeDAO.GetAccountByUsername(username);
            return account;
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
