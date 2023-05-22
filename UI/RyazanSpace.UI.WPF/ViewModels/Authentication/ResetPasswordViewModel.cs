using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.Services.Locator;
using System.Security.Cryptography;
using System.Text;
using System;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class ResetPasswordViewModel : Base.BaseViewModel
    {
        private RelayCommand _toLoginCommand;
        public RelayCommand ToLoginCommand => _toLoginCommand ??=
            new RelayCommand((v) => ServiceLocator.Instanse.Navigation.GoBack());


        private int _sessionId;


        private RelayCommand _resetPasswordCommand;
        public RelayCommand ResetPasswordCommand =>
            _resetPasswordCommand ??= new RelayCommand(async (v) => await ResetPassword());



        public string Email { get; set; }
        public string UserName { get; set; }
        public string NewPassword { get; set; }
        public int Code { get; set; }

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

            using MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.UTF8.GetBytes(NewPassword);
            byte[] hashBytes = md5.ComputeHash(inputBytes);
            string hash = Convert.ToHexString(hashBytes);
            bool result = false;
            try
            {
                result = await service.ConfirmSession(new ConfirmResetPasswordDTO(_sessionId, Code, hash));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }

            if (result)
                Locator.Navigation.SetView(new AuthViewModel());
            else
                Locator.Mbox.ShowInfo("Код неверен");
        }
    }
}
