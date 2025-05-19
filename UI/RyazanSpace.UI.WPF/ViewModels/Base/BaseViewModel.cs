using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.Services.Locator;
using System.Runtime.CompilerServices;

namespace RyazanSpace.UI.WPF.ViewModels.Base
{
    internal class BaseViewModel : ObservableObject
    {
        public virtual void OnAppearing() => this.OnPropertyChanged();

        protected ServiceLocator Locator => ServiceLocator.Instanse;
    }
}
