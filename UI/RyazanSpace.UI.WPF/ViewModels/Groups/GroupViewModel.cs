using Microsoft.Win32;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.Domain.Cloud.DTO;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.UI.WPF.MVVM;
using System.Threading.Tasks;
using System;
using System.IO;

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

        public GroupViewModel(GroupDTO group)
        {
            Group = group;
        }

        private GroupDTO _group;
        public GroupDTO Group { get => _group; set => Set(ref _group, value); }

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
                Domain.Cloud.DTO.CloudResourceDTO response =
                    await Locator.Cloud.Upload(new UploadRequestDTO(bytes, CloudResourceType.Image), token);

                //Указываем сохраненное фото в качестве логотипа
                await Locator.Groups.SetLogo(Group.Id, response.Id, token);

                //Обновляем интерфейс
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
                Group.SubsCount += Group.IsSubscibed ? 1 : -1 ;
                OnPropertyChanged(nameof(Group));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
        }
    }
}
