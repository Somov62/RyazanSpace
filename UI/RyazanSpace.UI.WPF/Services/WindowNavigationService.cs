using RyazanSpace.UI.WPF.ViewModels.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace RyazanSpace.UI.WPF.Services
{
    internal class WindowNavigationService
    {
        private BaseViewModel _currentView = null!;
        private readonly Stack<BaseViewModel> _viewsStack = new();
        public Window MainWindow => App.Current.MainWindow;


        public BaseViewModel CurrentView
        {
            get => _currentView;
            private set
            {
                _currentView = value;
                Notify();
            }
        }

        public void SetView(BaseViewModel viewModel)
        {
            CurrentView = viewModel;
            if (!_viewsStack.Contains(viewModel))
                _viewsStack.Push(viewModel);

            CurrentView?.OnAppearing();
        }

        public void GoBack()
        {
            _viewsStack.Pop();

            if (_viewsStack.Count > 0) 
                SetView(_viewsStack.Peek());
        }


        public delegate void ViewChangedHandler(BaseViewModel actualView);

        public event ViewChangedHandler ViewChanged;

        private void Notify()
        {
            ViewChanged?.Invoke(CurrentView);
        }
    }
}
