using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.API.Client;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Linq;
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
            new RelayCommand((v) =>
            {
                Locator.PageNavigation.SetPage(new GroupViewModel(v as GroupDTO));
            });

        private RelayCommand _AddGroupCommand;
        public RelayCommand AddGroupCommand => _AddGroupCommand ??=
            new RelayCommand((v) => { AddGroup(); });

        private async void AddGroup()
        {
            string groupName = string.Empty;
            if (Locator.Mbox.ShowInput("Придумайте имя для группы", ref groupName) != true) return;

            var token = Locator.Settings.Configuration.Token;
            CreateGroupDTO dto = new() { Name = groupName };
            try
            {
                //Запрос на сервер
                int id = await Locator.Groups.CreateGroup(dto, token);
                var group = await Locator.Groups.GetGroupById(id, token);
                Groups.Prepend(group);
                ManagedGroups.Prepend(group);
                OpenGroupCommand.Execute(group);
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

        public GroupsViewModel()
        {
            _groupService = Locator.Groups;
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

        public override async void OnAppearing()
        {
            base.OnAppearing();
            //Обновление данных

            //Обновление списка всех групп
            _actualPage = new Page<GroupDTO>() { PageSize = 15, PageIndex = -1 };
            Groups = new ObservableCollection<GroupDTO>();
            LoadNextPageCommand.Execute(null);
            OnPropertyChanged(nameof(Groups));

            var token = Locator.Settings.Configuration.Token;

            //Обновление списка подписок
            SubscribedGroups = new ObservableCollection<GroupDTO>(await Locator.Subs.GetSubscribedGroups(token));
            OnPropertyChanged(nameof(SubscribedGroups));

            //Обновление списка управляемых сообществ
            ManagedGroups = new ObservableCollection<GroupDTO>(await Locator.Groups.GetManagedGroups(token));
            OnPropertyChanged(nameof(ManagedGroups));
        }
    }
}
