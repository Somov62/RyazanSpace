using System.Linq;
using System.Windows.Controls;
using System.Windows.Input;

namespace RyazanSpace.UI.WPF.Views.Authentication
{
    public partial class EmailVerificationPage : Page
    {
        public EmailVerificationPage() => InitializeComponent();

        private void Page_MouseDown(object sender, MouseButtonEventArgs e) => Keyboard.ClearFocus();

        private void Code_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            if (!e.Text.All(char.IsDigit)) e.Handled = true;
        }
    }
}
