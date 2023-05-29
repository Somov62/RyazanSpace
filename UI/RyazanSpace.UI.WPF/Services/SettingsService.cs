using Newtonsoft.Json;
using RyazanSpace.UI.WPF.Models;
using RyazanSpace.UI.WPF.MVVM;
using System;
using System.IO;

namespace RyazanSpace.UI.WPF.Services
{
    internal class SettingsService
    {
        private readonly string _pathToFile = Path.Combine(Environment.CurrentDirectory, "appsettings.json");

        public SettingsService()
        {
            Configuration = Load();
        }

        public Configuration Configuration { get; }

        public void SaveChanges()
        {
            if (!Configuration.RememberCredintials)
            {
                Configuration.Token = null;
                Configuration.DateExpire = default;
            }
            File.WriteAllText(_pathToFile, JsonConvert.SerializeObject(Configuration));
        }

        private Configuration Load()
        {
            if (!File.Exists(_pathToFile))
                File.Create(_pathToFile).Close();
            Configuration config = JsonConvert.DeserializeObject<Configuration>(File.ReadAllText(_pathToFile));
            config ??= new Configuration();
            return config;
        }
    }

    internal class Configuration : ObservableObject
    {
        private Theme _activeTheme = Theme.Light;
        public Theme ActiveTheme { get => _activeTheme; set => Set(ref _activeTheme, value); }

        private User _user;
        public User User { get => _user; set => Set(ref _user, value); }

        /// <summary>
        /// Not  implemented
        /// </summary>
        public bool IsSavingTrafficModeEnabled { get; set; }

        private string _token;
        public string Token { get => _token; set => Set(ref _token, value); }

        private DateTimeOffset _dateExpire;
        public DateTimeOffset DateExpire { get => _dateExpire; set => Set(ref _dateExpire, value); }

        public string AuthAPI { get; set; }
        public string ProfileAPI { get; set; }

        public string CloudAPI { get; set; }

        public bool RememberCredintials { get; set; }
    }
}
