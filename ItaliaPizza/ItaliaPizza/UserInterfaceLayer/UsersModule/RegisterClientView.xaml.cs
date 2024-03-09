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
using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.DataLayer;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class RegisterClientView : Page
    {
        public RegisterClientView()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CleanErrorsLabels();
          //  if (AreDataValid())
            //{
                var client = new Client
                {
                    name = txtName.Text,
                    firstLastName = txtFirstName.Text,
                    secondLastName = txtLastName.Text,
                    phone = txtCellPhone.Text,
                    email = txtEmail.Text,
                };
                ClientDAO clientDAO = new ClientDAO();
                clientDAO.AddClient(client);
           // }else
            //{
             //   ShowErrors();
           // }
        }

        private bool AreDataValid()
        {
            bool emailValidation = IsEmailValid();
            bool clientDataValidation = ValidationClientData();
            return emailValidation && clientDataValidation;
        }

        private bool IsEmailValid()
        {
            ClientDAO clientDAO = new ClientDAO();
            String email = txtEmail.Text;
            bool isEmailValid = ApplicationLayer.Validation.IsEmailValid(email);
            bool isEmailAlreadyExisting = clientDAO.IsEmailExisting(email);
            return isEmailValid && isEmailAlreadyExisting;
        }

        private bool ValidationClientData()
        {
            String name = txtName.Text;
            String firstName = txtFirstName.Text;
            String lastName = txtLastName.Text;

            bool isNameValid = ApplicationLayer.Validation.IsNameValid(name);
            bool isMiddleNameValid = ApplicationLayer.Validation.IsNameValid(firstName);
            bool isLastNameValid = ApplicationLayer.Validation.IsNameValid(lastName);

            return isNameValid && isMiddleNameValid && isLastNameValid;
        }

        private void ShowErrors()
        {
            lblNameError.Visibility = Visibility.Visible;
            lblLastNameError.Visibility = Visibility.Visible;
            lblFirstNameError.Visibility = Visibility.Visible;
            lblCellPhoneError.Visibility = Visibility.Visible;
            lblEmailError.Visibility = Visibility.Visible;
        }

        private void CleanErrorsLabels()
        {
            lblNameError.Visibility = Visibility.Collapsed;
            lblLastNameError.Visibility = Visibility.Collapsed;
            lblFirstNameError.Visibility = Visibility.Collapsed;
            lblCellPhoneError.Visibility = Visibility.Collapsed;
            lblEmailError.Visibility = Visibility.Collapsed;
        }

    }
}
