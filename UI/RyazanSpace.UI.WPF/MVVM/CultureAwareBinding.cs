using System.Globalization;
using System.Windows.Data;

namespace RyazanSpace.UI.WPF.MVVM
{
    public class CultureAwareBinding : Binding
    {
        public CultureAwareBinding()
        {
            ConverterCulture = CultureInfo.CurrentCulture;
        }
    }
}
