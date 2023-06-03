using Microsoft.Win32;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Cloud.DTO;
using RyazanSpace.Domain.Profile.DTO;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.IO;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.Profile
{
    internal class ProfilePageModel : Base.BaseViewModel
    {

        private RelayCommand _getProfileCommand;
        public RelayCommand GetProfileCommand => _getProfileCommand ??=
            new RelayCommand(async (v) => Profile = await GetProfile()); 
        
        private RelayCommand _setAvatarCommand;
        public RelayCommand SetAvatarCommand => _setAvatarCommand ??=
            new RelayCommand(async (v) => await SetAvatar());

        private RelayCommand _setStatusCommand;
        public RelayCommand SetStatusCommand => _setStatusCommand ??=
            new RelayCommand(async (v) => await SetStatus());

        private RelayCommand _logOutCommand;
        public RelayCommand LogOutCommand => _logOutCommand ??=
            new RelayCommand(async (v) => await LogOut());


        private ProfilePageModel _profilePage;
        private RelayCommand _toProfilePageCommand;
        public RelayCommand ToProfilePageCommand => _toProfilePageCommand ??=
            new RelayCommand((v) => Locator.PageNavigation.SetPage(_profilePage ??= new()));


        public ProfilePageModel()
        {
            GetProfileCommand.Execute(null);
        }

        private ProfileDTO _profile;
        public ProfileDTO Profile  { get => _profile; set => Set(ref _profile, value); }

        private async Task<ProfileDTO> GetProfile()
        {
            var token = Locator.Settings.Configuration.Token;

            try
            {
                var id = await Locator.AuthService.TryGetUserByToken(token);
                if (id == null) return null;
                return await Locator.Profile.GetProfileById(id.Value, token);
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
            return null;
        }

        private async Task LogOut()
        {
            var config = Locator.Settings.Configuration;

            try
            {
                await Locator.AuthService.Logout(config.Token);
                config.Token = null;
                config.DateExpire = default;
                config.RememberCredintials = true;
                Locator.WindowNavigation.GoBack();
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }

        }

        private async Task SetAvatar()
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
                Domain.Cloud.DTO.CloudResourceDTO response =
                    await Locator.Cloud.Upload(new UploadRequestDTO(bytes, CloudResourceType.Image), token);

                //Указываем сохраненное фото в качестве аватарки
                await Locator.Profile.SetAvatar(response.Id, token);

                //Обновляем интерфейс
                Profile.Avatar.DownloadLink = response.DownloadLink;
                OnPropertyChanged("Profile");
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }

        private async Task SetStatus()
        {
            string status = Profile.Status;
            if (Locator.Mbox.ShowInput("Введите новый статус", ref status) != true) return;

            var token = Locator.Settings.Configuration.Token;
            try
            {
                //Запрос на сервер
                await Locator.Profile.SetStatus(status, token);
                
                //Обновляем интерфейс
                Profile.Status = status;
                OnPropertyChanged("Profile");
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }
    }
}
