﻿using System;
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
            if (AreFieldsValid() && IsEmailExisting())
            {
                var client = new Client
                {
                    name = txtName.Text,
                    phone = txtCellPhone.Text,
                    email = txtEmail.Text,
                };
                ClientDAO clientDAO = new ClientDAO();
                if (clientDAO.AddClient(client))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha registrado el cliente");
                    CleanTextFields();
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
            bool isEmailAlreadyExisting = clientDAO.IsEmailExisting(email);
            return isEmailAlreadyExisting;
        }

        private void ShowErrors(List<int> errors)
        {
            Console.WriteLine("Pasé por acá");
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
