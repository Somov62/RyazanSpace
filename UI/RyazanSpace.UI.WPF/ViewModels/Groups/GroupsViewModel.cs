using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.API.Client;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.Groups
{
    internal class GroupsViewModel : Base.BaseViewModel
    {


        private RelayCommand _loadNextPageCommand;
        public RelayCommand LoadNextPageCommand => _loadNextPageCommand ??=
            new RelayCommand(async (v) =>
            {
            if (_actualPage.PageSize * (_actualPage.PageIndex + 1) > Groups.Count) return;
                await LoadNextPage();
            });

        private RelayCommand _OpenGroupCommand;
        public RelayCommand OpenGroupCommand => _OpenGroupCommand ??=
            new RelayCommand(async (v) =>
            {
                Locator.PageNavigation.SetPage(new GroupViewModel(v as GroupDTO));
            });

        public GroupsViewModel()
        {
            _groupService = Locator.Groups;
            LoadNextPageCommand.Execute(null);
        }

        public ObservableCollection<GroupDTO> Groups { get; set; } = new ObservableCollection<GroupDTO>();
        public ObservableCollection<GroupDTO> SubscribedGroups { get; set; } = new ObservableCollection<GroupDTO>();
        public ObservableCollection<GroupDTO> ManagedGroups { get; set; } = new ObservableCollection<GroupDTO>();


        private IPage<GroupDTO> _actualPage = new Page<GroupDTO>() { PageSize = 15, PageIndex = -1 };
        private readonly WebGroupService _groupService;

        private async Task LoadNextPage()
        {
            var token = Locator.Settings.Configuration.Token;
            try
            {
                _actualPage = await _groupService.GetPage(_actualPage.PageIndex + 1, 15, token);
            }
            catch (NotFoundException) { return; }
            catch (Exception ex)
            {
                Locator.ExceptionHandler.Handle(ex);
                return;
            }
            foreach (var item in _actualPage.Items)
            {
                Groups.Add(item);
            }
        }
    }
}
