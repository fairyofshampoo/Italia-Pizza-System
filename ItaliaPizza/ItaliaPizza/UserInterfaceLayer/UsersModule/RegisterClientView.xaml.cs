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
    public partial class RegisterClientView : Page
    {

        private String re

        public RegisterClientView()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (registerClient())
            {
                //Mostrar mensaje de éxito
            }
        }

        private bool RegisterClient()
        {

        }

        private bool AreDataValid()
        {
            bool emailValidation = IsEmailVerified();
            bool clientDataValidation = ValidationClientData();
            return emailValidation && clientDataValidation;
        }

    }
}
