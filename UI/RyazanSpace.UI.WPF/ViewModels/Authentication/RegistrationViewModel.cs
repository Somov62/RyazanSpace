using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Linq;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class RegistrationViewModel : Base.BaseViewModel
    {

        private RelayCommand _toLoginCommand;
        public RelayCommand ToLoginCommand => _toLoginCommand ??=
            new RelayCommand((v) => Locator.WindowNavigation.GoBack());

        private RelayCommand _regCommand;
        public RelayCommand RegistrationCommand => _regCommand ??= new RelayCommand((v) => Register());


        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }


        private async void Register()
        {
            if (string.IsNullOrWhiteSpace(Password))
            {
                Locator.Mbox.ShowInfo("Укажите пароль!");
                return;
            }
            else
            {
                StringBuilder errors = new();
                string password = Password;
                if (password.Length < 8)
                    errors.AppendLine("Пароль минимум 8 символов");
                if (!password.Any(Char.IsLower) || !password.Any(Char.IsUpper))
                    errors.AppendLine("Используйте в пароле буквы верхнего и нижнего регистров");
                if (!password.Any(Char.IsDigit))
                    errors.AppendLine("Используйте в пароле цифры");
                char[] chars = "!@#$%^&*()_+{}|?><:-=][\';/.,'".ToCharArray();
                if (password.Intersect(chars).Count() == 0)
                    errors.AppendLine("Используйте в пароле спецсимволы");
                if (errors.Length > 0)
                {
                    Locator.Mbox.ShowInfo(errors.ToString());
                    return;
                }
            }

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
                Locator.WindowNavigation.SetView(new EmailVerificationViewModel(user.Id));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }
            

        }
    }
}
