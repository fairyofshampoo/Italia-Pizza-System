using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ItaliaPizzaData.DataLayer.DAO;
using ItaliaPizzaData.DataLayer;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;

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
            if (AreFieldsValid() && IsEmailExisting())
            {
                var client = new Client
                {
                    name = txtName.Text,
                    phone = txtCellPhone.Text,
                    email = txtEmail.Text,
                };
                ClientDAO clientDAO = new ClientDAO();
                try
                {
                    if (clientDAO.AddClient(client))
                    {
                        ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha registrado el cliente");
                        CleanTextFields();
                    }
                }
                catch (SqlException)
                {
                    ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
                }
                catch (DbUpdateException)
                {
                    ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
                }
                catch (EntityException)
                {
                    ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
                }
                catch (InvalidOperationException)
                {
                    ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
                }

            } 
        }

        private bool AreFieldsValid()
        {
            bool areFieldsValid = true;
            List<string> data = new List<string>();
            data.Add(txtName.Text);
            data.Add(txtCellPhone.Text);
            data.Add(txtEmail.Text);

            List<int> errors = ApplicationLayer.Validations.ValidationClientData(data);

            if (errors.Any())
            {
                areFieldsValid = false;
                ShowErrors(errors);
            }
            return areFieldsValid;
        }

        private bool IsEmailExisting()
        {
            ClientDAO clientDAO = new ClientDAO();
            String email = txtEmail.Text;
            bool isEmailAlreadyExisting = false;
            try
            {
                isEmailAlreadyExisting = clientDAO.IsEmailExisting(email);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (DbUpdateException)
            {
                ApplicationLayer.DialogManager.ShowDBUpdateExceptionMessageBox();
            }
            catch (EntityException)
            {
                ApplicationLayer.DialogManager.ShowEntityExceptionMessageBox();
            }
            catch (InvalidOperationException)
            {
                ApplicationLayer.DialogManager.ShowInvalidOperationExceptionMessageBox();
            }
            return isEmailAlreadyExisting;
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
                        lblCellPhoneError.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        lblEmailError.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void CleanErrorsLabels()
        {
            lblNameError.Visibility = Visibility.Collapsed;
            lblCellPhoneError.Visibility = Visibility.Collapsed;
            lblEmailError.Visibility = Visibility.Collapsed;
        }

        private void CleanTextFields()
        {
            txtName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtCellPhone.Text = string.Empty;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
