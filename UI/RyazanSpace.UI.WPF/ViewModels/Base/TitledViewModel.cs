namespace RyazanSpace.UI.WPF.ViewModels.Base
{
    class TitledViewModel : BaseViewModel
    {
        private string _title = string.Empty;
        public string Title { get => _title; set => Set(ref _title, value); }
    }
}
