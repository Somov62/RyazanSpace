using RyazanSpace.UI.WPF.Services.Locator;
using RyazanSpace.UI.WPF.ViewModels.Profile;

namespace RyazanSpace.UI.WPF.ViewModels.Main
{
    internal class MainPageModel : Base.BaseViewModel
    {
        public MainPageModel()
        {
            Locator.PageNavigation.PageChanged += ViewChanged;
            Locator.PageNavigation.SetPage(new ProfilePageModel());
        }

        private void ViewChanged(Base.BaseViewModel actualView) => OnPropertyChanged(nameof(CurrentView));

        public Base.BaseViewModel CurrentView => ServiceLocator.Instanse.PageNavigation.CurrentPage;
    }
}
