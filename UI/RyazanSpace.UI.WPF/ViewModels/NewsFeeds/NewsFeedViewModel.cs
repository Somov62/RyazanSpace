using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Domain.Groups.DTO;
using RyazanSpace.Interfaces.Repositories;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.NewsFeeds
{
    internal class NewsFeedViewModel : Base.BaseViewModel
    {
        private RelayCommand _loadNextPageCommand;
        public RelayCommand LoadNextPageCommand => _loadNextPageCommand ??=
            new RelayCommand(async (v) =>
            {
                if (_actualPage.PageSize * (_actualPage.PageIndex + 1) > Posts.Count) return;
                await LoadNextPage();
            });

        private IPage<PostDTO> _actualPage = new Page<PostDTO>() { PageSize = 15, PageIndex = -1 };

        public ObservableCollection<PostDTO> Posts { get; set; } = new ObservableCollection<PostDTO>();


        private async Task LoadNextPage()
        {
            var token = Locator.Settings.Configuration.Token;
            try
            {
                _actualPage = await Locator.Posts.GetPage(_actualPage.PageIndex + 1, 15, token);
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

        public override void OnAppearing()
        {
            base.OnAppearing();
            //Обновление списка всех постов
            _actualPage = new Page<PostDTO>() { PageSize = 15, PageIndex = -1 };
            Posts = new ObservableCollection<PostDTO>();
            LoadNextPageCommand.Execute(null);
            OnPropertyChanged(nameof(Posts));
        }
    }
}
