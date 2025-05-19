using System.Windows;

namespace RyazanSpace.UI.WPF.Services.MessageBoxes
{
    public partial class InputMessageBox : Window
    {
        public InputMessageBox(string title, string inputText)
        {
            InitializeComponent();
            Title = title;
            InputText = inputText;
            this.DataContext = this;
        }

        public string InputText { get; set; }

        private void Border_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Close_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Ok_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
            this.Close();
        }
    }
}
