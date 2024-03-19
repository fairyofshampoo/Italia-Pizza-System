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
            
        }

        private void BtnSave_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
