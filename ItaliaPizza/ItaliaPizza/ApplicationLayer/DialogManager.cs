using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace ItaliaPizza.ApplicationLayer
{
    public static class DialogManager
    {
        public static void ShowErrorMessageBox(string errorMessage)
        {
            MessageBox.Show(errorMessage, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowDataBaseErrorMessageBox()
        {
            MessageBox.Show("Error de conexión a la base de datos. Por favor, inténtalo nuevamente más tarde.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        public static void ShowWarningMessageBox(string warningMessage)
        {
            MessageBox.Show(warningMessage, "Advertencia", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        public static void ShowSuccessMessageBox(string successMessage)
        {
            MessageBox.Show(successMessage, "Operación exitosa", MessageBoxButton.OK, MessageBoxImage.Information);
        }

    }
}
