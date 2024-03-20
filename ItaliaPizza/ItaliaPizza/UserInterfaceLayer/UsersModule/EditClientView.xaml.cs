using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
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
    public partial class EditClientView : Page
    {
        private string emailClient;

        public EditClientView(string email)
        {
            InitializeComponent();
            this.emailClient = email;
            ShowData();
        }

        private void ShowData() 
        {
            txtEmail.Text = emailClient;
        }

        private void BtnDisable_Click(object sender, RoutedEventArgs e)
        {
            ClientDAO clientDAO = new ClientDAO();
            if (clientDAO.DisableClient(emailClient))
            {
                ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha deshabilitado el usuario");
            }
            else
            {
                //Mensaje de error
            }
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {
            CleanErrorsLabels();
            if (AreFieldsValid())
            {
                var client = new Client
                {
                    name = txtName.Text + " " + txtFirstLastName.Text + " " + txtSecondLastName.Text,
                    phone = txtCellPhone.Text
                };
                ClientDAO clientDAO = new ClientDAO();
                if(clientDAO.EditDataClient(client))
                {
                    ApplicationLayer.DialogManager.ShowSuccessMessageBox("Se ha editado la información del cliente");
                    CleanTextFields();
                }
            }
        }

        private bool AreFieldsValid()
        {
            bool areFieldsValid = true;
            List<string> data = new List<string>();
            data.Add(txtName.Text);
            data.Add(txtFirstLastName.Text);
            data.Add(txtSecondLastName.Text);
            data.Add(txtCellPhone.Text);
            data.Add(txtEmail.Text);

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
                        lblNameError.Visibility = Visibility.Visible;
                        break;
                    case 2:
                        lblFirstLastNameError.Visibility = Visibility.Visible;
                        break;
                    case 3:
                        lblSecondLastNameError.Visibility = Visibility.Visible;
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
            lblNameError.Visibility = Visibility.Visible;
            lblFirstLastNameError.Visibility = Visibility.Visible;
            lblSecondLastNameError.Visibility = Visibility.Visible;
            lblCellPhoneError.Visibility = Visibility.Visible;
        }

        private void CleanTextFields()
        {
            txtName.Text = string.Empty;
            txtFirstLastName.Text = string.Empty;
            txtSecondLastName.Text = string.Empty;
            txtCellPhone.Text = string.Empty;
        }

    }
}
