using Microsoft.Extensions.DependencyInjection;

namespace RyazanSpace.UI.WPF.Services.Locator
{
    internal class ServiceLocator
    {
        public NavigationService NavigationService => App.Services.GetRequiredService<NavigationService>();
    }
}
