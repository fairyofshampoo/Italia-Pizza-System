using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{
    public partial class EditClientView : Page
    {
        private string emailClient;

        public EditClientView(Client client)
        {
            InitializeComponent();
            emailClient = client.email;
            ShowData(client);
            if(client.status == 1)
            {
                ShowEnableClient();
            }else
            {
                ShowDisableClient();
            }
        }

        private void ShowData(Client client) 
        {
            txtName.Text = client.name;
            txtEmail.Text = client.email;
            txtCellPhone.Text = client.phone;
        }

        private void ShowEnableClient()
        {
            txtName.IsEnabled = true;
            txtCellPhone.IsEnabled = true;
            btnDisable.Visibility = Visibility.Visible;
            btnEnable.Visibility = Visibility.Collapsed;
            btnModify.Visibility = Visibility.Visible;
        }

        private void ShowDisableClient()
        {
            txtName.IsEnabled = false;
            txtCellPhone.IsEnabled = false;
            btnEnable.Visibility = Visibility.Visible;
            btnDisable.Visibility = Visibility.Hidden;
            btnModify.Visibility = Visibility.Collapsed;
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            int status = 0;
            ChangeStatus(status);
        }

        private void BtnEnable_Click(object sender, RoutedEventArgs e)
        {
            int status = 1;
            ChangeStatus(status);
        }

        private void ChangeStatus(int status)
        {
            ClientDAO clientDAO = new ClientDAO();
            if (clientDAO.ChangeStatusClient(emailClient, status))
            {
                if(status == 0)
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha deshabilitado al usuario");
                    ShowDisableClient();
                } else
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha habilitado al usuario");
                    ShowEnableClient();
                }
            }
            else
            {
                ApplicationLayer.DialogManager.ShowErrorMessageBox("No se ha podido realizar el cambios, inténtelo más tarde");
            }
        }

        private void BtnModify_Click(object sender, RoutedEventArgs e)
        {
            CleanErrorsLabels();
            if (AreFieldsValidForEdit())
            {
                var client = new Client
                {
                    name = txtName.Text,
                    phone = txtCellPhone.Text,
                    email = txtEmail.Text,
                };
                ClientDAO clientDAO = new ClientDAO();
                if (clientDAO.EditDataClient(client))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha editado la información del cliente");
                    NavigationService.GoBack();
                }
            }
        }

        private bool AreFieldsValidForEdit()
        {
            bool areFieldsValid = true;
            List<string> data = new List<string>();

            data.Add(txtName.Text);
            data.Add(txtCellPhone.Text);

            List<int> errors = ApplicationLayer.Validations.ValidationClientData(data);
           
            if(errors.Any())
            {
                areFieldsValid = false;
                ShowErrors(errors);
            } 
            return areFieldsValid;
        }

        private void ShowErrors(List<int> errors)
        {
            for (int index = 0; index < errors.Count; index++)
            {
                int error = errors[index];
                switch (error)
                {

                    case 1:
                        lblNameEditedError.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        lblCellPhoneEditedError.Visibility = Visibility.Visible;
                        break;
                }
            }
        }

        private void CleanErrorsLabels()
        {
            lblNameEditedError.Visibility = Visibility.Collapsed;
            lblCellPhoneEditedError.Visibility = Visibility.Collapsed;
        }

        private void BtnGoBack_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
