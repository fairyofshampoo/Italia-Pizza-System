﻿using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
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
    /// Lógica de interacción para EditEmployeeView.xaml
    /// </summary>
    public partial class EditEmployeeView : Page
    {
        public EditEmployeeView(Employee employeeData)
        {
            InitializeComponent();
            SetComboBoxItems();

            SetModifyEmployee(employeeData.email);
        }

        private void btnDesactive_Click(object sender, RoutedEventArgs e)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Confirmación", "¿Desea eliminar al empleado?", DialogWindow.DialogType.YesNo, DialogWindow.IconType.Question);

            if (dialogWindow.ShowDialog() == true)
            {
                string user = txtUsername.Text;
                EmployeeDAO employeeDAO = new EmployeeDAO();

                if (employeeDAO.ChangeStatus(user, Constants.INACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Empleado actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private void btnActive_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("¿Desea activar al empleado?", "Confirmación", MessageBoxButton.YesNo, MessageBoxImage.Information);

            if (result == MessageBoxResult.Yes)
            {
                string user = txtUsername.Text;
                EmployeeDAO employeeDAO = new EmployeeDAO();

                if (employeeDAO.ChangeStatus(user, Constants.ACTIVE_STATUS))
                {
                    DialogManager.ShowSuccessMessageBox("Empleado actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ResetFields();

            if (ValidateFields())
            {
                if (ModifyEmployee())
                {
                    DialogManager.ShowSuccessMessageBox("Empleado actualizado exitosamente");
                }
                else
                {
                    DialogManager.ShowErrorMessageBox("Ha ocurrido un error al actualizar el empleado");
                }
            }
        }

        private bool ModifyEmployee()
        {
            string name = txtName.Text;
            string phone = txtPhone.Text;
            string email = txtEmail.Text;
            string employeeType = cmbEmployeeType.SelectedItem.ToString();
            string user = txtUsername.Text;

            EmployeeDAO employeeDAO = new EmployeeDAO();

            Employee employee = new Employee
            {
                name = name,
                phone = phone,
                email = email,
                role = employeeType,
            };

            Account account = new Account
            {
                user = user,
            };

            if (pswPassword.Password != "passex1*")
            {
                account.password = pswPassword.Password;
            }

            return employeeDAO.ModifyEmployee(employee, account);
        }

        public void SetModifyEmployee(string email)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            Employee employeeInfo = employeeDAO.GetEmployeeByEmail(email);
            Account accountinfo = employeeDAO.GetEmployeeAccountByEmail(email);
            int statusAccount = 0;

            if (employeeInfo != null)
            {
                txtName.Text = employeeInfo.name;
                txtPhone.Text = employeeInfo.phone;
                txtEmail.Text = employeeInfo.email;
                cmbEmployeeType.SelectedIndex = cmbEmployeeType.Items.IndexOf(employeeInfo.role);
            }

            if (accountinfo != null)
            {
                txtUsername.Text = accountinfo.user;
                pswPassword.Password = "passex1*";
                statusAccount = accountinfo.status;

                if (statusAccount == Constants.INACTIVE_STATUS)
                {
                    txtName.IsEnabled = false;                    
                    txtPhone.IsEnabled = false;
                    cmbEmployeeType.IsEnabled = false;
                    pswPassword.IsEnabled = false;

                    btnDesactive.IsEnabled = false;
                    btnDesactive.Visibility = Visibility.Hidden;

                    btnActive.IsEnabled = true;
                    btnActive.Visibility = Visibility.Visible;

                    btnSave.IsEnabled = false;
                    btnSave.Background = Brushes.Gray;
                }
            }
        }

        private void SetComboBoxItems()
        {
            cmbEmployeeType.ItemsSource = new String[]
            {
                "Cocinero", "Cajero", "Mesero"
            };
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

            if (txtPhone.Text.Equals(string.Empty) || !Validations.IsPhoneValid(txtPhone.Text))
            {
                txtPhone.BorderBrush = Brushes.Red;
                txtPhone.BorderThickness = new Thickness(2);
                lblPhoneError.Visibility = Visibility.Visible;
                validateFields = false;
            }

            if (cmbEmployeeType.SelectedItem == null)
            {
                cmbEmployeeType.BorderBrush = Brushes.Red;
                cmbEmployeeType.BorderThickness = new Thickness(2);
                lblEmployeeTypeError.Visibility = Visibility.Visible;
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

        private void ResetFields()
        {
            txtName.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtName.BorderThickness = new Thickness(0);
            lblNameError.Visibility = Visibility.Collapsed;            

            txtPhone.BorderBrush = System.Windows.Media.Brushes.Transparent;
            txtPhone.BorderThickness = new Thickness(0);
            lblPhoneError.Visibility = Visibility.Collapsed;

            cmbEmployeeType.BorderBrush = System.Windows.Media.Brushes.Transparent;
            cmbEmployeeType.BorderThickness = new Thickness(0);
            lblEmployeeTypeError.Visibility = Visibility.Collapsed;

            pswPassword.BorderBrush = System.Windows.Media.Brushes.Transparent;
            pswPassword.BorderThickness = new Thickness(0);
            lblPassowrdError.Visibility = Visibility.Collapsed;
        }
    }
}
