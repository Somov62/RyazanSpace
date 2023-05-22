using Newtonsoft.Json;
using RyazanSpace.UI.WPF.Models;
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

    public class Configuration
    {
        public Theme ActiveTheme { get; set; } = Theme.Light;

        public User User { get; set; }

        /// <summary>
        /// Not  implemented
        /// </summary>
        public bool IsSavingTrafficModeEnabled { get; set; }

        public string Token { get; set; }

        public DateTimeOffset DateExpire { get; set; }

        public string AuthAPI { get; set; }

        public bool RememberCredintials { get; set; }

    }
}
