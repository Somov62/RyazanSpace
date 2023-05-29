using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RyazanSpace.Domain.Auth.API.Client;
using RyazanSpace.Domain.Cloud.API.Client;
using RyazanSpace.Domain.Profile.API.Client;
using RyazanSpace.UI.WPF.Services;
using RyazanSpace.UI.WPF.Services.Locator;
using RyazanSpace.UI.WPF.Services.MessageBoxes;
using RyazanSpace.UI.WPF.ViewModels;
using RyazanSpace.UI.WPF.Views.Windows;
using System;
using System.Data;
using System.Linq;
using System.Windows;

namespace RyazanSpace.UI.WPF
{
    public partial class App : Application
    {
        public static Window WindowActivate => App.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsActive);
        public static Window WindowFocused => App.Current.Windows.Cast<Window>().FirstOrDefault(p => p.IsFocused);

        public static Window WindowCurrent => WindowFocused ?? WindowActivate;

        private static IHost __Hosting;
        public static IHost Hosting => __Hosting ??= App.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Hosting.Services;

        public App() => InitializeComponent();

        protected override async void OnStartup(StartupEventArgs e)
        {
            var theme = ServiceLocator.Instanse.Settings.Configuration.ActiveTheme;
            ServiceLocator.Instanse.Theme.SetTheme(theme);
            var host = Hosting;
            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(true);
        }

        protected override async void OnExit(ExitEventArgs e)
        {
            ServiceLocator.Instanse.Settings.SaveChanges();
            using var host = Hosting;
            base.OnExit(e);
            await host.StopAsync().ConfigureAwait(false);
        }


        public static IHostBuilder CreateHostBuilder(string[] Args) => Host
            .CreateDefaultBuilder(Args)
            .ConfigureServices(ConfigureServices)
            ;

        public static void ConfigureServices(HostBuilderContext host, IServiceCollection services)
        {
            services.AddSingleton<WindowNavigationService>();
            services.AddSingleton<PageNavigationService>();
            services.AddTransient<WebExceptionsHandler>();
            services.AddSingleton<SettingsService>();
            services.AddTransient<ThemeService>();
            services.AddTransient<MboxService>();

            services.AddHttpClient<WebAuthService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["AuthAPI"]}/Auth/"); });

            services.AddHttpClient<WebRegistrationService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["AuthAPI"]}/Registration/"); });
            
            services.AddHttpClient<WebEmailVerificationService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["AuthAPI"]}/EmailVerification/"); });

            services.AddHttpClient<WebResetPasswordService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["AuthAPI"]}/ResetPassword/"); });

            services.AddHttpClient<WebProfileService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["ProfileAPI"]}/Profiles/"); });
            
            services.AddHttpClient<WebCloudService>
                  (configureClient:
                  client => { client.BaseAddress = new Uri($"{host.Configuration["CloudAPI"]}/Resources/"); });
        }
    }
}
