using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using System.Security.Cryptography;
using System.Text;
using System;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class RegistrationViewModel : Base.BaseViewModel
    {

        private RelayCommand _toLoginCommand;
        public RelayCommand ToLoginCommand => _toLoginCommand ??=
            new RelayCommand((v) => Locator.Navigation.GoBack());

        private RelayCommand _regCommand;
        public RelayCommand RegistrationCommand => _regCommand ??= new RelayCommand((v) => Register());


        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        private async void Register()
        {
            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(Password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hash = Convert.ToHexString(hashBytes);

            try
            {
                var user = await Locator.RegService.Register(new RegRequestDTO()
                {
                    Email = this.Email,
                    Name = this.UserName,
                    Password = hash
                });
                Locator.Navigation.SetView(new EmailVerificationViewModel(user.Id));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
            

        }
    }
}
