using RyazanSpace.UI.WPF.Services.Locator;
using RyazanSpace.UI.WPF.ViewModels.Authentication;

namespace RyazanSpace.UI.WPF.ViewModels
{
    internal class MainViewModel : Base.BaseViewModel
    {
       // private AuthViewModel _authPage;
       //
       // private RelayCommand _toAuthPageCommand;
       // public RelayCommand ToAuthPageCommand => _toAuthPageCommand ??= 
       //     new RelayCommand((v) => ServiceLocator.Instanse.WindowNavigationService.SetView(_authPage ??= new()));

        public MainViewModel()
        {
            Locator.Navigation.ViewChanged += ViewChanged;
            if (Locator.Settings.Configuration.Token == null ||
                Locator.Settings.Configuration.DateExpire < System.DateTimeOffset.Now)
            {
                Locator.Navigation.SetView(new AuthViewModel());
            }
        }


        private void ViewChanged(Base.BaseViewModel actualView)
        {
            OnPropertyChanged(nameof(CurrentView));
        }

        public Base.BaseViewModel CurrentView => ServiceLocator.Instanse.Navigation.CurrentView;
    }
}
