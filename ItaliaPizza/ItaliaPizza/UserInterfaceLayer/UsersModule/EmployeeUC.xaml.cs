using ItaliaPizza.ApplicationLayer;
using ItaliaPizzaData.DataLayer;
using ItaliaPizzaData.DataLayer.DAO;
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
    /// Lógica de interacción para EmployeeUC.xaml
    /// </summary>
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

            EmployeeDAO employeeDAO = new EmployeeDAO();
            Account account = employeeDAO.GetEmployeeAccountByEmail(employee.email);

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
