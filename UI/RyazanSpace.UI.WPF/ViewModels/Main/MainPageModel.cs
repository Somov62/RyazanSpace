using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.Services.Locator;
using RyazanSpace.UI.WPF.ViewModels.Groups;
using RyazanSpace.UI.WPF.ViewModels.Profile;
using System.Windows;

namespace RyazanSpace.UI.WPF.ViewModels.Main
{
    internal class MainPageModel : Base.BaseViewModel
    {

        private ProfilePageModel _profilePage;

        private RelayCommand _toProfileCommand;
        public RelayCommand ToProfileCommand => _toProfileCommand ??=
            new RelayCommand((v) => Locator.PageNavigation.SetPage(_profilePage ??= new()));

        private GroupsViewModel _groupsPage;

        private RelayCommand _toGroupsCommand;
        public RelayCommand ToGroupsCommand => _toGroupsCommand ??=
            new RelayCommand((v) => Locator.PageNavigation.SetPage(_groupsPage ??= new()));

        private RelayCommand _shutdownCommand;
        public RelayCommand ShutdownCommand => _shutdownCommand ??=
            new RelayCommand((v) => Application.Current.Shutdown());


        public MainPageModel()
        {
            Locator.PageNavigation.PageChanged += ViewChanged;
            ToProfileCommand.Execute(null);
        }

        private void ViewChanged(Base.BaseViewModel actualView) => OnPropertyChanged(nameof(CurrentView));

        public Base.BaseViewModel CurrentView => ServiceLocator.Instanse.PageNavigation.CurrentPage;
    }
}
