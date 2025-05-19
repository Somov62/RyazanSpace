using RyazanSpace.UI.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Windows;

namespace RyazanSpace.UI.WPF.Services
{
    internal class PageNavigationService
    {
        private BaseViewModel _currentPage = null!;
        private readonly Stack<BaseViewModel> _pagesStack = new();
        public Window MainWindow => App.Current.MainWindow;


        public BaseViewModel CurrentPage
        {
            get => _currentPage;
            private set
            {
                _currentPage = value;
                Notify();
            }
        }

        public void SetPage(BaseViewModel viewModel)
        {
            CurrentPage = viewModel;
            if (!_pagesStack.Contains(viewModel))
                _pagesStack.Push(viewModel);

            CurrentPage?.OnAppearing();
        }

        public void GoBack()
        {
            if (_pagesStack.Count < 2)
            {
                throw new Exception("You cannot go back when less 2 views in stack");
            }
            _pagesStack.Pop();

            SetPage(_pagesStack.Peek());
        }


        public delegate void PageChangedHandler(BaseViewModel actualView);

        public event PageChangedHandler PageChanged;

        private void Notify()
        {
            PageChanged?.Invoke(CurrentPage);
        }
    }
}
