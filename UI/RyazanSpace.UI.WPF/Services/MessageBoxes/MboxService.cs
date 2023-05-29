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

        public bool? ShowInput(string title, ref string inputText)
        {
            var mbox = new InputMessageBox(title, inputText);
            mbox.Owner = App.Current.MainWindow;
            mbox.ShowDialog();
            inputText = mbox.InputText;
            return mbox.DialogResult;
        }
    }
}
