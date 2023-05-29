using RyazanSpace.UI.WPF.Services.Locator;
using RyazanSpace.UI.WPF.ViewModels.Authentication;
using RyazanSpace.UI.WPF.ViewModels.Main;

namespace RyazanSpace.UI.WPF.ViewModels
{
    internal class MainViewModel : Base.BaseViewModel
    {

        public MainViewModel()
        {
            Locator.WindowNavigation.ViewChanged += ViewChanged;
            if (Locator.Settings.Configuration.Token == null ||
                Locator.Settings.Configuration.DateExpire < System.DateTimeOffset.Now)
            {
                Locator.WindowNavigation.SetView(new AuthViewModel());
            }
            else
                Locator.WindowNavigation.SetView(new MainPageModel());
        }


        private void ViewChanged(Base.BaseViewModel actualView)
        {
            OnPropertyChanged(nameof(CurrentView));
        }

        public Base.BaseViewModel CurrentView => ServiceLocator.Instanse.WindowNavigation.CurrentView;
    }
}
