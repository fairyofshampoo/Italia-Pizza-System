using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
using System.Data.Entity.Core;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace ItaliaPizza.UserInterfaceLayer.UsersModule
{

    public partial class EmployeeUC : UserControl
    {
        private Employee employeeData;

        public EmployeesView EmployeesView { get; set; }

        public EmployeeUC()
        {
            InitializeComponent();
        }

        public void SetDataCards(Employee employee)
        {
            lblFullName.Content = employee.name;
            lblEmployeeType.Content = employee.role;
            lblEmail.Content = employee.email;
            Account account = new Account();
            EmployeeDAO employeeDAO = new EmployeeDAO();

            try
            {
                account = employeeDAO.GetEmployeeAccountByEmail(employee.email);
            }
            catch (SqlException)
            {
                ApplicationLayer.DialogManager.ShowDataBaseErrorMessageBox();
            }
            catch (ArgumentException)
            {
                ApplicationLayer.DialogManager.ShowArgumentExceptionMessageBox();
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

            if (account.status == Constants.INACTIVE_STATUS)
            {
                lblFullName.Foreground = Brushes.Red;
            }

            employeeData = employee;
        }

        private void BtnEditEmployee_Click(object sender, RoutedEventArgs e)
        {
            EditEmployeeView editEmployeeView = new EditEmployeeView(employeeData);
            EmployeesView.NavigationService.Navigate(editEmployeeView);
        }
    }
}
