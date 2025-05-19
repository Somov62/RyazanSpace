using RyazanSpace.UI.WPF.Services.Locator;
using System;
using System.Linq;
using System.Windows;

namespace RyazanSpace.UI.WPF.Services
{
    internal class ThemeService
    {
        private const string _propertyName = "ActiveTheme";

        public Theme GetCurrentTheme()
        {
            ResourceDictionary theme = GetCurrentResource();
            if (theme == null)
            {
                SetTheme(Theme.Light);
                return Theme.Light;
            }

            switch (theme.GetType().Name)
            {
                case "LightTheme":
                    return Theme.Light;

                case "DarkTheme":
                    return Theme.Dark;
                default:
                    SetTheme(Theme.Light);
                    return Theme.Light;
            }
        }

        public void SetTheme(Theme theme)
        {
            DeleteOldTheme();
            switch (theme)
            {
                case Theme.Light:
                    Application.Current.Resources.MergedDictionaries.Add(
                        new ResourceDictionary() { Source = new Uri("../Resources/Themes/LightTheme.xaml", UriKind.Relative)});
                    break;
                case Theme.Dark:
                    Application.Current.Resources.MergedDictionaries.Add(
                        new ResourceDictionary() { Source = new Uri("../Resources/Themes/DarkTheme.xaml", UriKind.Relative)});
                    break;
            }
            ServiceLocator.Instanse.Settings.Configuration.ActiveTheme = theme;
        }

        private void DeleteOldTheme()
        {
            ResourceDictionary theme = GetCurrentResource();
            Application.Current.Resources.MergedDictionaries.Remove(theme);
        }

        private ResourceDictionary GetCurrentResource()
        {
            return Application.Current.Resources.MergedDictionaries.FirstOrDefault(p => p.GetType().ToString().Contains("Theme"));
        }

    }

    public enum Theme
    {
        Light,
        Dark
    }
}
