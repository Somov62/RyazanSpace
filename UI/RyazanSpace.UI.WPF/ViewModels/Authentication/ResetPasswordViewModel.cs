using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.Services.Locator;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Linq;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class ResetPasswordViewModel : Base.BaseViewModel
    {
        private RelayCommand _toLoginCommand;
        public RelayCommand ToLoginCommand => _toLoginCommand ??=
            new RelayCommand((v) => ServiceLocator.Instanse.WindowNavigation.GoBack());


        private int _sessionId;


        private RelayCommand _resetPasswordCommand;
        public RelayCommand ResetPasswordCommand =>
            _resetPasswordCommand ??= new RelayCommand(async (v) => await ResetPassword());



        public string Email { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public int? Code { get; set; }

        public bool IsSecondStep { get; set; }

        public async Task ResetPassword()
        {
            var service = ServiceLocator.Instanse.ResetPasswordService;

            if (_sessionId == 0)
            {
                _sessionId = await service.CreateSession(new ResetPasswordRequestDTO(UserName, Email));
                IsSecondStep = true;
                OnPropertyChanged(nameof(IsSecondStep));
                return;
            }

            if (Code == null)
            {
                Locator.Mbox.ShowInfo("Укажите код!");
                return;
            }

            if (string.IsNullOrWhiteSpace(NewPassword))
            {
                Locator.Mbox.ShowInfo("Укажите пароль!");
                return;
            }
            else
            {
                StringBuilder errors = new();
                string password = NewPassword;
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
            byte[] inputBytes = Encoding.UTF8.GetBytes(NewPassword);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hash = Convert.ToHexString(hashBytes);
            bool result = false;
            try
            {
                result = await service.ConfirmSession(new ConfirmResetPasswordDTO(_sessionId, Code.Value, hash));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }

            if (result)
                Locator.WindowNavigation.SetView(new AuthViewModel());
            else
                Locator.Mbox.ShowInfo("Код неверен");
        }
    }
}
