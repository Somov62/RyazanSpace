using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RyazanSpace.UI.WPF.Services;
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

        public static IHost __Hosting;
        public static IHost Hosting => __Hosting ??= App.CreateHostBuilder(Environment.GetCommandLineArgs()).Build();
        public static IServiceProvider Services => Hosting.Services;

        public App() => InitializeComponent();

        protected override async void OnStartup(StartupEventArgs e)
        {
            var host = Hosting;
            base.OnStartup(e);
            await host.StartAsync().ConfigureAwait(true);
            Services.GetRequiredService<MainWindow>().Show();
        }

        protected override async void OnExit(ExitEventArgs e)
        {
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
            services.AddSingleton<NavigationService>();
            services.AddScoped<MainViewModel>();
        }
    }
}
