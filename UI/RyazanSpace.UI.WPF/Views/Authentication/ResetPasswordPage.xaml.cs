using RyazanSpace.UI.WPF.ViewModels.Authentication;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace RyazanSpace.UI.WPF.Views.Authentication
{
    public partial class ResetPasswordPage : Page
    {
        public ResetPasswordPage() => InitializeComponent();

        private void Page_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void PasswordBox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            passwordBox.IsManipulationEnabled = passwordBox.Password.Length == 0;
            ((ResetPasswordViewModel)this.DataContext).NewPassword = passwordBox.Password;
        }
    }
}
