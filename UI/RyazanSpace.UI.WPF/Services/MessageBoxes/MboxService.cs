namespace RyazanSpace.UI.WPF.Services.MessageBoxes
{
    internal class MboxService
    {
        public void ShowInfo(string message)
        {
            var mbox = new InfoMessageBox(message);
            mbox.Owner = App.Current.MainWindow;
            mbox.ShowDialog();
        }
    }
}
