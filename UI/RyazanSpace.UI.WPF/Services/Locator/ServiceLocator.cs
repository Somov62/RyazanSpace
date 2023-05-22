using Microsoft.Extensions.DependencyInjection;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.UI.WPF.Services.MessageBoxes;

namespace RyazanSpace.UI.WPF.Services.Locator
{
    internal class ServiceLocator
    {
        private static ServiceLocator _instance;
        public static ServiceLocator Instanse => _instance ??= new ServiceLocator();


        public NavigationService Navigation => App.Services.GetRequiredService<NavigationService>();
        public WebExceptionsHandler ExceptionHandler => App.Services.GetRequiredService<WebExceptionsHandler>();
        public SettingsService Settings => App.Services.GetRequiredService<SettingsService>();
        public ThemeService Theme => App.Services.GetRequiredService<ThemeService>();
        public MboxService Mbox => App.Services.GetRequiredService<MboxService>();

        public WebAuthService AuthService => App.Services.GetRequiredService<WebAuthService>();
        public WebRegistrationService RegService => App.Services.GetRequiredService<WebRegistrationService>();
        public WebResetPasswordService ResetPasswordService => App.Services.GetRequiredService<WebResetPasswordService>();
        public WebEmailVerificationService EmailVerificationService => App.Services.GetRequiredService<WebEmailVerificationService>();
    }
}
