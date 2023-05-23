using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Mails;
using RyazanSpace.Interfaces.Email;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Auth.Services
{
    public class ResetPasswordService
    {
        private readonly IRepository<ResetPasswordSession> _passwordRepository;
        private readonly WebUserRepository _userRepository;
        private readonly IEmailSender _mailService;

        public ResetPasswordService(
            IRepository<ResetPasswordSession> passwordRepository,
            WebUserRepository userRepository,
            IEmailSender mailService)
        {
            _passwordRepository = passwordRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }

        /// <summary>
        ///     <para>Создает сессию для сброса пароля.</para>
        /// Генерирует случайный пятизначный код подтверждения. <br/>
        /// Отправляет сообщение с кодом на электронную почту пользователя.
        /// </summary>
        /// <param name="model"><see cref="ResetPasswordRequestDTO"/> - модель с логином и почтой пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>id созданной сессии</returns>
        public async Task<int> CreateSession(ResetPasswordRequestDTO model, string cancelEndPoint)
        {
            var user = await _userRepository.GetByEmail(model.UserEmail).ConfigureAwait(false);
            if (user == null)
                throw new NotFoundException("Пользователь не найден!");

            if (user.Name != model.UserName)
                throw new ArgumentException("Данные о пользователя неверны!");

            Random rnd = new();
            ResetPasswordSession session = new()
            {
                Owner = user,
                VerificationCode = rnd.Next(10000, 100000)
            };
            session = await _passwordRepository.Add(session).ConfigureAwait(false);
            await _mailService.SendEmailAsync(
                new ResetPasswordMessage(
                    user.Email, 
                    session.VerificationCode, 
                    cancelEndPoint.Replace("ID", session.Id.ToString()
                    )));
            return session.Id;
        }

        /// <summary>
        ///     <para>Метод для сброса пароля</para>
        /// Сопоставляет присланный код подтверждения с настоящим
        /// </summary>
        /// <param name="model"><see cref="ConfirmResetPasswordDTO"/>
        /// - модель с кодом сессии, кодом подтверждения и новым паролем</param>
        /// <exception cref="NotFoundException"/>
        /// <exception cref="TimeOutSessionException"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>true - в случае успеха</returns>
        public async Task<bool> ConfirmSession(ConfirmResetPasswordDTO model)
        {
            if (model.SessionId < 1)
                throw new NotFoundException("Сессия сброса пароля не найдена!");

            var session = await _passwordRepository.GetById(model.SessionId).ConfigureAwait(false);
            if (session == null)
                throw new NotFoundException("Сессия сброса пароля не найдена!");

            if (session.DateExpire < DateTimeOffset.Now)
                throw new TimeOutSessionException("Сессия сброса пароля устарела! Повторите процесс с начала.");

            if (session.VerificationCode != model.VerificationCode)
                return false;

            var user = await _userRepository.GetById(session.Owner.Id).ConfigureAwait(false);
            user.Password = model.NewPassword;
            await _userRepository.Update(user).ConfigureAwait(false);
            await _mailService.SendEmailAsync(
                new SuccessResetPasswordMessage(user.Email, user.Name));
            return true;
        }

        /// <summary>
        /// Метод для принудительного завершения сессии сброса пароля.
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>
        public async Task BreakSession(int sessionId)
        {
            await _passwordRepository.DeleteById(sessionId).ConfigureAwait(false);
        }
    }
}
