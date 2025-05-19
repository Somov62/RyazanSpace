using Microsoft.Win32;
using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Cloud.DTO;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.Groups
{
    internal class GroupViewModel : Base.BaseViewModel
    {
        private RelayCommand _SetDescriptionCommand;
        public RelayCommand SetDescriptionCommand => _SetDescriptionCommand ??=
            new RelayCommand(async (v) => await SetDescription());

        private RelayCommand _SetLogoCommand;
        public RelayCommand SetLogoCommand => _SetLogoCommand ??=
            new RelayCommand(async (v) => await SetLogo());

        private RelayCommand _SubscribeCommand;
        public RelayCommand SubscribeCommand => _SubscribeCommand ??=
            new RelayCommand(async (v) => await Subscribe());

        private RelayCommand _PublishPostCommand;
        public RelayCommand PublishPostCommand => _PublishPostCommand ??=
            new RelayCommand(async (v) => await PublishPost());

        private RelayCommand _AddPhotoToPostCommand;
        public RelayCommand AddPhotoToPostCommand => _AddPhotoToPostCommand ??=
            new RelayCommand(async (v) => await AddPhotoToPost());

        private RelayCommand _loadNextPageCommand;
        public RelayCommand LoadNextPageCommand => _loadNextPageCommand ??=
            new RelayCommand(async (v) =>
            {
                if (_actualPage.PageSize * (_actualPage.PageIndex + 1) > Posts.Count) return;
                await LoadNextPage();
            });

        public GroupViewModel(GroupDTO group)
        {
            Group = group;
            LoadNextPageCommand.Execute(null);
        }

        private IPage<PostDTO> _actualPage = new Page<PostDTO>() { PageSize = 15, PageIndex = -1 };

        private GroupDTO _group;
        public GroupDTO Group { get => _group; set => Set(ref _group, value); }

        private PostDTO _post = new PostDTO() { Resources = new ObservableCollection<CloudResourceDTO>() };
        public PostDTO NewPost { get => _post; set => Set(ref _post, value); }

        public ObservableCollection<PostDTO> Posts { get; set; } = new ObservableCollection<PostDTO>();




        private async Task LoadNextPage()
        {
            var token = Locator.Settings.Configuration.Token;
            try
            {
                _actualPage = await Locator.Posts.GetByGroupPage(Group.Id, _actualPage.PageIndex + 1, 15, token);
            }
            catch (NotFoundException) { return; }
            catch (Exception ex)
            {
                Locator.ExceptionHandler.Handle(ex);
                return;
            }
            foreach (var item in _actualPage.Items)
            {
                Posts.Add(item);
            }
        }

        private async Task SetLogo()
        {
            OpenFileDialog d = new()
            {
                Filter = "PNG|*.png|JPG|*.jpg",
                Multiselect = false
            };
            if (d.ShowDialog() != true) return;

            var bytes = File.ReadAllBytes(d.FileName);
            var token = Locator.Settings.Configuration.Token;

            try
            {
                //Загружаем фото на сервер
                CloudResourceDTO response =
                    await Locator.Cloud.Upload(new UploadRequestDTO(bytes, CloudResourceType.Image), token);

                //Указываем сохраненное фото в качестве логотипа
                await Locator.Groups.SetLogo(Group.Id, response.Id, token);

                //Обновляем интерфейс
                if (Group.Logo == null) Group.Logo = new CloudResourceDTO();
                Group.Logo.DownloadLink = response.DownloadLink;
                OnPropertyChanged(nameof(Group));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

        private async Task SetDescription()
        {
            string status = Group.Description;
            if (Locator.Mbox.ShowInput("Придумайте описание для группы", ref status) != true) return;

            var token = Locator.Settings.Configuration.Token;
            try
            {
                //Запрос на сервер
                await Locator.Groups.SetDescription(Group.Id, status, token);

                //Обновляем интерфейс
                Group.Description = status;
                OnPropertyChanged(nameof(Group));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

        private async Task Subscribe()
        {
            var token = Locator.Settings.Configuration.Token;
            try
            {
                if (Group.IsSubscibed)
                    await Locator.Subs.Unsubscribe(Group.Id, token);
                else
                    await Locator.Subs.Subscribe(Group.Id, token);
                Group.IsSubscibed = !Group.IsSubscibed;
                Group.SubsCount += Group.IsSubscibed ? 1 : -1;
                OnPropertyChanged(nameof(Group));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

        private async Task PublishPost()
        {
            var token = Locator.Settings.Configuration.Token;
            if (!Group.IsOwner)
                return;
            try
            {
                CreatePostDTO dto = new()
                {
                    GroupId = Group.Id,
                    Text = NewPost.Text,
                    Resources = new List<CloudResourceDTO>(NewPost.Resources)
                };

                var post = await Locator.Posts.CreatePost(dto, token);

                Posts.Insert(0, post);
                NewPost = new PostDTO() { Resources = new ObservableCollection<CloudResourceDTO>() };
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }


        private async Task AddPhotoToPost()
        {

            OpenFileDialog d = new()
            {
                Filter = "PNG|*.png|JPG|*.jpg",
                Multiselect = false
            };
            if (d.ShowDialog() != true) return;

            var bytes = File.ReadAllBytes(d.FileName);
            var token = Locator.Settings.Configuration.Token;

            try
            {
                //Загружаем фото на сервер
                CloudResourceDTO response =
                    await Locator.Cloud.Upload(new UploadRequestDTO(bytes, CloudResourceType.Image), token);

                NewPost.Resources.Add(response);
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

    }
}
