using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.ViewModels.Main;
using System;
using System.Security.Cryptography;
using System.Text;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class AuthViewModel : Base.BaseViewModel
    {

        #region Commands
        private ResetPasswordViewModel _resetPasswordPage;

        private RelayCommand _toResetPasswordCommand;
        public RelayCommand ToResetPasswordCommand => _toResetPasswordCommand ??=
            new RelayCommand((v) => Locator.WindowNavigation.SetView(_resetPasswordPage ??= new()));


        private RegistrationViewModel _registrationPage;

        private RelayCommand _toRegistrationCommand;
        public RelayCommand ToRegistrationCommand => _toRegistrationCommand ??=
            new RelayCommand((v) => Locator.WindowNavigation.SetView(_registrationPage ??= new()));

        private RelayCommand _loginCommand;
        public RelayCommand LoginCommand => _loginCommand ??= new RelayCommand((v) => Login());

        #endregion

        public string Email { get; set; }
        public string Password { get; set; }

        public bool RememberMe 
        {
            get => Locator.Settings.Configuration.RememberCredintials;
            set => Locator.Settings.Configuration.RememberCredintials = value;
        }

        private async void Login()
        {
            if (string.IsNullOrWhiteSpace(Password) || string.IsNullOrWhiteSpace(Email))
                return;

            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hash = Convert.ToHexString(hashBytes);

            try
            {
                var token = await Locator.AuthService.Login(new AuthRequestDTO(Email, hash));
                Locator.Settings.Configuration.Token = token.Token;
                Locator.Settings.Configuration.DateExpire = token.DateExpired;
                Locator.WindowNavigation.SetView(new MainPageModel());
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }

        }
    }
}
