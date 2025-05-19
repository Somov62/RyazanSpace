using System.Windows;

namespace RyazanSpace.UI.WPF.Services.MessageBoxes
{
    public partial class InfoMessageBox : Window
    {
        public InfoMessageBox(string message)
        {
            InitializeComponent();
            txtContent.Text = message;
        }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
