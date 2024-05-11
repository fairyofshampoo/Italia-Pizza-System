using ItaliaPizza.ApplicationLayer;
using ItaliaPizza.DataLayer;
using ItaliaPizza.DataLayer.DAO;
using ItaliaPizza.UserInterfaceLayer.FinanceModule;
using ItaliaPizza.UserInterfaceLayer.ProductsModule;
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
    /// Lógica de interacción para EmployeesView.xaml
    /// </summary>
    public partial class EmployeesView : Page
    {
        private int rowsAdded = 0;

        public EmployeesView()
        {
            InitializeComponent();
            menuFrame.Content = new ManagerMenu(this);
            SetLastEmployees();
        }

        private void btnAllFilter_Click(object sender, RoutedEventArgs e)
        {
            btnActivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInactivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            SetLastEmployees();
        }

        private void btnActivesFilter_Click(object sender, RoutedEventArgs e)
        {
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInactivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnActivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));

            SearchEmployeeByStatus(Constants.ACTIVE_STATUS);
        }

        private void txtSearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            string searchText = ((TextBox)sender).Text;
            if (searchText.Length > 3)
            {
                SearchEmployeeByName(searchText);
            }
        }

        private void btnInactivesFilter_Click(object sender, RoutedEventArgs e)
        {
            btnAllFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnActivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 233, 225, 255));
            btnInactivesFilter.Background = new SolidColorBrush(Color.FromArgb(255, 255, 123, 0));
            
            SearchEmployeeByStatus(Constants.INACTIVE_STATUS);
        }

        private void btnAddEmployee_Click(object sender, RoutedEventArgs e)
        {
            EmployeeRegisterView employeeRegisterView = new EmployeeRegisterView();
            NavigationService.Navigate(employeeRegisterView);
        }

        private void SetLastEmployees()
        {
            List<Employee> employees = GetLastEmployees();
            if (employees.Any())
            {
                ShowEmployees(employees);
                txtSearchBar.IsReadOnly = false;
            }
            else
            {
                txtSearchBar.IsReadOnly = true;
                DialogManager.ShowErrorMessageBox("No hay empleados registrados");
            }
        }

        private void ShowEmployees(List<Employee> employees)
        {
            rowsAdded = 0;
            EmployeesGrid.Children.Clear();
            EmployeesGrid.RowDefinitions.Clear();

            if (employees.Any())
            {
                lblEmployeeNotFound.Visibility = Visibility.Collapsed;
                foreach (Employee employee in employees)
                {
                    AddEmployees(employee);
                }
            }
            else
            {
                lblEmployeeNotFound.Visibility = Visibility.Visible;
            }
        }

        private List<Employee> GetLastEmployees()
        {
            List<Employee> lastEmployees = new List<Employee>();
            EmployeeDAO employeeDAO = new EmployeeDAO();
            lastEmployees = employeeDAO.GetLastEmployeesRegistered();
            return lastEmployees;
        }

        private void AddEmployees(Employee employee)
        {
            EmployeeUC employeeCard = new EmployeeUC();
            employeeCard.EmployeesView = this;
            Grid.SetRow(employeeCard, rowsAdded);
            employeeCard.SetDataCards(employee);
            EmployeesGrid.Children.Add(employeeCard);
            rowsAdded++;

            RowDefinition row = new RowDefinition();
            EmployeesGrid.RowDefinitions.Add(row);
        }

        private void SearchEmployeeByName(string name)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            List<Employee> employees = employeeDAO.GetEmployeesByName(name);
            ShowEmployees(employees);
        }

        private void SearchEmployeeByStatus(int status)
        {
            EmployeeDAO employeeDAO = new EmployeeDAO();
            List<Employee> employees = employeeDAO.GetEmployeesByStatus(status);
            ShowEmployees(employees);
        }
    }
}
