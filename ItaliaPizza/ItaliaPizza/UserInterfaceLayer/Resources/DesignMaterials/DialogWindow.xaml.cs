using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using static ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials.DialogWindow;

namespace ItaliaPizza.UserInterfaceLayer.Resources.DesignMaterials
{
    /// <summary>
    /// Interaction logic for DialogWindow.xaml
    /// </summary>
    public partial class DialogWindow : Window
    {
        public DialogWindow()
        {
            InitializeComponent();
        }

        public enum DialogType
        {
            YesNo,
            OK
        }

        public enum IconType
        {
            None,
            Error,
            Warning,
            Information,
            Question
        }

        public void SetDialogWindowData(string title, string content, DialogType dialogType, IconType iconType)
        {
            this.txtTitle.Text = title;
            this.txtContent.Text = content;
            SetDialogType(dialogType);
            SetDialogIcon(iconType);
        }

        private void SetDialogType(DialogType dialogType)
        {
            switch (dialogType)
            {
                case DialogType.YesNo:
                    this.btnOK.Visibility = Visibility.Collapsed;
                    this.btnAccept.Visibility = Visibility.Visible;
                    this.btnCancel.Visibility = Visibility.Visible;
                    break;
                case DialogType.OK:
                    this.btnOK.Visibility = Visibility.Visible;
                    this.btnAccept.Visibility = Visibility.Collapsed;
                    this.btnCancel.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void SetDialogIcon(IconType iconType)
        {
            switch (iconType)
            {
                case IconType.Error:
                    this.errorIcon.Visibility = Visibility.Visible;
                    break;
                case IconType.Warning:
                    this.warningIcon.Visibility = Visibility.Visible;
                    break;
                case IconType.Information:
                    this.infoIcon.Visibility = Visibility.Visible;
                    break;
                case IconType.Question:
                    this.questionIcon.Visibility = Visibility.Visible;
                    break;
                default:
                    break;
            }
        }

        private void BtnOK_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnAccept_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
            Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }

}
