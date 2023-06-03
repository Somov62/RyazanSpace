using Microsoft.Extensions.DependencyInjection;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Cloud.API.Client;
using RyazanSpace.Domain.Groups.API.Client;
using RyazanSpace.Domain.Profile.API.Client;
using RyazanSpace.UI.WPF.Services.MessageBoxes;

namespace RyazanSpace.UI.WPF.Services.Locator
{
    internal class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instanse => _instance ??= new ServiceLocator();


        public WindowNavigationService WindowNavigation => App.Services.GetRequiredService<WindowNavigationService>();
        public PageNavigationService PageNavigation => App.Services.GetRequiredService<PageNavigationService>();
        public WebExceptionsHandler ExceptionHandler => App.Services.GetRequiredService<WebExceptionsHandler>();
        public WebProfileService Profile => App.Services.GetRequiredService<WebProfileService>();
        public SettingsService Settings => App.Services.GetRequiredService<SettingsService>();
        public WebGroupService Groups => App.Services.GetRequiredService<WebGroupService>();
        public WebCloudService Cloud => App.Services.GetRequiredService<WebCloudService>();
        public ThemeService Theme => App.Services.GetRequiredService<ThemeService>();
        public MboxService Mbox => App.Services.GetRequiredService<MboxService>();

        public WebAuthService AuthService => App.Services.GetRequiredService<WebAuthService>();
        public WebRegistrationService RegService => App.Services.GetRequiredService<WebRegistrationService>();
        public WebResetPasswordService ResetPasswordService => App.Services.GetRequiredService<WebResetPasswordService>();
        public WebEmailVerificationService EmailVerificationService => App.Services.GetRequiredService<WebEmailVerificationService>();
    }
}
