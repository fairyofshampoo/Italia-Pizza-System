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
            if (AreDataValid())
            {
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
            }else
            {

            }
        }

        private bool AreDataValid()
        {
            bool emailValidation = IsEmailExisting();
            bool clientDataValidation = ValidationClientData();
            return emailValidation && clientDataValidation;
        }

        private bool IsEmailExisting()
        {
            ClientDAO clientDAO = new ClientDAO();
            String email = txtEmail.Text;
            bool isEmailAlreadyExisting = clientDAO.IsEmailExisting(email);
            return isEmailAlreadyExisting;
        }

        private bool ValidationClientData()
        {
            String name = txtName.Text;
            String firstName = txtFirstName.Text;
            String lastName = txtLastName.Text;
            String email = txtEmail.Text;
            String phone = txtCellPhone.Text;

            List<int> errors = new List<int>();
            bool IsValid = true; 

            if(!Validations.IsNameValid(name))
            {
                int errorName = 1;
                errors.Add(errorName);
            }

            if (!Validations.IsNameValid(lastName))
            {
                int errorLastName = 2;
                errors.Add(errorLastName);
            }

            if (!Validations.IsNameValid(firstName))
            {
                int errorFirstName = 3;
                errors.Add(errorFirstName);
            }

            if(!Validations.IsPhoneValid(phone))
            {
                int errorPhone = 4;
                errors.Add(errorPhone);
            }

            if (!Validations.IsEmailValid(email))
            {
                int errorEmail = 5;
                errors.Add(errorEmail);
            }

            if (errors.Any())
            {
                ShowErrors(errors);
                IsValid = false;
            }

            return IsValid;
        }

        private void ShowErrors(List<int> errors)
        {
            for (int index = 0; index < errors.Count; index++) 
            {
                int error = errors[index];
                switch (error)
                {
                    case 1:
                        lblNameError.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        lblLastNameError.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        lblFirstNameError.Visibility = Visibility.Visible;
                        break;
                    case 4:
                        lblCellPhoneError.Visibility = Visibility.Visible;
                        break;
                    case 5:
                        lblEmailError.Visibility = Visibility.Visible;
                        break;
                }
            }
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
