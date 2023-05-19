using RyazanSpace.UI.WPF.MVVM;
using System.Runtime.CompilerServices;

namespace RyazanSpace.UI.WPF.ViewModels.Base
{
    internal class BaseViewModel : ObservableObject
    {

        public bool Set<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (Equals(field, value))
                return false;

            field = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        public virtual void OnAppearing() => this.OnPropertyChanged();
    }
}
