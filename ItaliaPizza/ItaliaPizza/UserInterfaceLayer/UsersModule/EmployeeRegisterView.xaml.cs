using ItaliaPizza.ApplicationLayer;
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
    /// <summary>
    /// Lógica de interacción para EmployeeRegisterView.xaml
    /// </summary>
    public partial class EmployeeRegisterView : Page
    {
        public EmployeeRegisterView()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                string validationDuplicateData = ValidateDuplicateData();
                if (string.IsNullOrEmpty(validationDuplicateData))
                {
                    {
                        if (RegisterEmployee())
                        {
                            DialogManager.ShowSuccessMessageBox("Empleado registrado exitosamente");
                        }
                    }
                }
                else
                {
                    DialogManager.ShowWarningMessageBox(validationDuplicateData);
                }
            }

        }

        /*
        private void AddModifyEmployee(string operation)
        {
            if (ValidateFields())
            {
                string validationDuplicateData = ValidateDuplicateData();

                if (string.IsNullOrEmpty(validationDuplicateData))
                {
                    if (RegisterEmployee())
                    {
                        DialogManager.ShowSuccessMessageBox("Empleado registrado exitosamente");
                    }
                }
                else
                {
                    DialogManager.ShowWarningMessageBox(validationDuplicateData);
                }
            }
        
        }
        */

        private bool RegisterEmployee()
        {
            string name = txtName.Text;
            string firstLastName = txtFirstLastName.Text;
            string secondLastName = txtSecondLastName.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;
            string employeeType = cmbEmployeeType.SelectedItem.ToString();
            string user = txtUsername.Text;
            string password = pswPassword.Password;

            EmployeeDAO employeeDAO = new EmployeeDAO();

            Employee employee = new Employee
            {
                name = name,
                firstLastName = firstLastName,
                secondLastName = secondLastName,
                phone = phone,
                email = email,
                role = employeeType,
            };

            Account account = new Account
            {
                user = user,
                password = password,
            };

            return employeeDAO.AddEmployee(employee, account);
        }

        private bool ValidateFields()
        {
            bool validateFields = true;

            if (txtName.Text.Equals(string.Empty) || !Validations.IsNameValid(txtName.Text))
            {
                txtName.BorderBrush = Brushes.Red;
                txtName.BorderThickness = new Thickness(2);
                lblNameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtFirstLastName.Text.Equals(string.Empty) || !Validations.IsNameValid(txtFirstLastName.Text))
            {
                txtFirstLastName.BorderBrush = Brushes.Red;
                txtFirstLastName.BorderThickness = new Thickness(2);
                lblFirstLastNameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtSecondLastName.Text.Equals(string.Empty) || !Validations.IsNameValid(txtSecondLastName.Text))
            {
                txtSecondLastName.BorderBrush = Brushes.Red;
                txtSecondLastName.BorderThickness = new Thickness(2);
                lblSecondLastNameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtPhone.Text.Equals(string.Empty) || !Validations.IsPhoneValid(txtPhone.Text))
            {
                txtPhone.BorderBrush = Brushes.Red;
                txtPhone.BorderThickness = new Thickness(2);
                lblPhoneError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtEmail.Text.Equals(string.Empty) || !Validations.IsEmailValid(txtEmail.Text))
            {
                txtEmail.BorderBrush = Brushes.Red;
                txtEmail.BorderThickness = new Thickness(2);
                lblEmailError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbEmployeeType.SelectedItem == null)
            {
                cmbEmployeeType.BorderBrush = Brushes.Red;
                cmbEmployeeType.BorderThickness = new Thickness(2);
                lblEmployeeTypeError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (txtUsername.Text.Equals(string.Empty) || !Validations.IsUserValid(txtUsername.Text))
            {
                txtUsername.BorderBrush = Brushes.Red;
                txtUsername.BorderThickness = new Thickness(2);
                lblUsernameError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (string.IsNullOrEmpty(pswPassword.Password))
            {
                pswPassword.BorderBrush = Brushes.Red;
                pswPassword.BorderThickness = new Thickness(2);
                lblPassowrdError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            return validateFields;
        }

        private String ValidateDuplicateData()
        {
            StringBuilder validationDuplicateData = new StringBuilder();

            if (IsEmailExisting())
            {
                validationDuplicateData.AppendLine("El correo ingresado ya se encuentra registrado.");
            }

            if (IsUserExisting())
            {
                validationDuplicateData.AppendLine("El nommbre de usuario ingresado ya se encuentra registrado.");
            }

            return validationDuplicateData.ToString();
        }

        private bool IsEmailExisting()
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            String email = txtEmail.Text;
            bool isEmailAlreadyExisting = employeeDAO.IsEmailExisting(email);
            return isEmailAlreadyExisting;
        }

        private bool IsUserExisting()
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            String user = txtUsername.Text;
            bool isUserAlreadyExisting = employeeDAO.IsUserExisting(user);
            return isUserAlreadyExisting;
        }

        private void ResetFields()
        {
            txtName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtName.BorderThickness = new Thickness(0);
            lblNameError.Visibility = Visibility.Collapsed;

            txtFirstLastName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtFirstLastName.BorderThickness = new Thickness(0);
            lblFirstLastNameError.Visibility = Visibility.Collapsed;

            txtSecondLastName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtSecondLastName.BorderThickness = new Thickness(0);
            lblSecondLastNameError.Visibility = Visibility.Collapsed;

            txtPhone.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtPhone.BorderThickness = new Thickness(0);
            lblPhoneError.Visibility = Visibility.Collapsed;

            txtEmail.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtEmail.BorderThickness = new Thickness(0);
            lblEmailError.Visibility = Visibility.Collapsed;

            cmbEmployeeType.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbEmployeeType.BorderThickness = new Thickness(0);
            lblEmployeeTypeError.Visibility = Visibility.Collapsed;

            txtUsername.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtUsername.BorderThickness = new Thickness(0);
            lblUsernameError.Visibility = Visibility.Collapsed;

            pswPassword.BorderBrush = System.Windows.Media.Brushes.Transparent;
            pswPassword.BorderThickness = new Thickness(0);
            lblPassowrdError.Visibility = Visibility.Collapsed;
        }
    }
}
