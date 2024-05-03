using ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials;
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
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Error", errorMessage, DialogWindow.DialogType.OK, DialogWindow.IconType.Error);
            dialogWindow.ShowDialog();
        }

        public static void ShowDataBaseErrorMessageBox()
        {
            ShowErrorMessageBox("Error de conexión a la base de datos. Por favor, inténtalo nuevamente más tarde.");
        }

        public static void ShowWarningMessageBox(string warningMessage)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Advertencia", warningMessage, DialogWindow.DialogType.OK, DialogWindow.IconType.Warning);
            dialogWindow.ShowDialog();
        }

        public static void ShowSuccessMessageBox(string successMessage)
        {
            DialogWindow dialogWindow = new DialogWindow();
            dialogWindow.SetDialogWindowData("Operación exitosa", successMessage, DialogWindow.DialogType.OK, DialogWindow.IconType.Information);
            dialogWindow.ShowDialog();
        }
    }
}
