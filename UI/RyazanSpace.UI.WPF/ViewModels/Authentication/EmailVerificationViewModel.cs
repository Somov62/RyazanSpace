using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.UI.WPF.MVVM;
using RyazanSpace.UI.WPF.Services.Locator;
using System;
using System.Threading.Tasks;

namespace RyazanSpace.UI.WPF.ViewModels.Authentication
{
    internal class EmailVerificationViewModel : Base.BaseViewModel
    {
        private readonly int _userId;
        private int _sessionId;


        private RelayCommand _createSessionCommand;
        public RelayCommand CreateSessionCommand => 
            _createSessionCommand ??= new RelayCommand(async(v) => await CreateSession());

        private RelayCommand _confirmSessionCommand;
        public RelayCommand ConfirmSessionCommand =>
            _confirmSessionCommand ??= new RelayCommand(async (v) => await ConfirmSession());


        public EmailVerificationViewModel(int userId)
        {
            _userId = userId;
            CreateSessionCommand.Execute(null);
        }

        public async Task CreateSession()
        {
            _sessionId = await Locator.EmailVerificationService.CreateSession(_userId);
        }

        public int Code { get; set; }

        public async Task ConfirmSession()
        {
            var service = Locator.EmailVerificationService;
            bool result = false;

            try
            {
                result = await service.ConfirmSession(new EmailVerificationRequestDTO(_sessionId, Code));
            }
            catch (Exception ex) { Locator.ExceptionHandler.Handle(ex); }

            if (result)
                Locator.Navigation.SetView(new AuthViewModel());
            else
                Locator.Mbox.ShowInfo("Код неверен");
        }
    }
}
