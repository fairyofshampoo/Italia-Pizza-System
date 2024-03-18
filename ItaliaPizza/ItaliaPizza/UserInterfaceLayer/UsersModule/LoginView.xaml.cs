using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
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
            Account account = null;
            return account;
        }

        private void TgbtnPasswordVisibility_Unchecked(object sender, RoutedEventArgs e)
        {

        }

        private void TgbtnPasswordVisibility_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
