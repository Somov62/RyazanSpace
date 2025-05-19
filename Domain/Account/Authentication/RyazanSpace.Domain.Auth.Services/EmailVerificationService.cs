using RyazanSpace.Core.Exceptions;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Mails;
using RyazanSpace.Interfaces.Email;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.Domain.Auth.Services
{
    public class EmailVerificationService
    {

        private readonly WebUserRepository _userRepository;
        private readonly IRepository<EmailVerificationSession> _emailRepository;
        private readonly IEmailSender _mailService;

        public EmailVerificationService(
            WebUserRepository userRepository,
            IRepository<EmailVerificationSession> emailRepository,
            IEmailSender mailService
            )
        {
            _userRepository = userRepository;
            _emailRepository = emailRepository;
            _mailService = mailService;
        }

        /// <summary>
        ///     <para>Проверяет статус почты пользователя.</para>
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>true - почта подтверждена, в остальных случаях - false</returns>
        public async Task<bool> CheckEmailVerified(int userId)
        {
            var user = await this.GetUserById(userId).ConfigureAwait(false);
            return user.IsEmailVerified;
        }

        /// <summary>
        ///     <para>Создает сессию для подтверждения почты.</para>
        /// Генерирует случайный пятизначный код подтверждения. <br/>
        /// Отправляет сообщение с кодом на электронную почту пользователя.
        /// </summary>
        /// <param name="userId">id пользователя</param>
        /// <exception cref="NotFoundException"/>
        /// <returns>id созданной сессии</returns>
        public async Task<int> CreateSession(int userId)
        {
            var user = await this.GetUserById(userId).ConfigureAwait(false);
            if (user.IsEmailVerified)
                throw new Exception("Почта уже подтверждена!");
            
            Random rnd = new();
            EmailVerificationSession session = new()
            { 
                Owner = user,
                VerificationCode = rnd.Next(10000, 100000)
            };
            session = await _emailRepository.Add(session);
            await _mailService.SendEmailAsync(
                new EmailVerificationMessage(user.Email, session.VerificationCode)).ConfigureAwait(false);
            return session.Id;
        }

        /// <summary>
        ///     <para>Метод для подтверждения почты</para>
        /// Сопоставляет присланный код подтверждения с настоящим
        /// </summary>
        /// <param name="model"> Модель с кодом сессии и кодом подтверждения</param>
        /// <exception cref="NotFoundException"/>
        /// <exception cref="TimeOutSessionException"/>
        /// <exception cref="ArgumentException"/>
        /// <returns>true - в случае успеха</returns>
        public async Task<bool> ConfirmSession(EmailVerificationRequestDTO model)
        {
            if (model.SessionId < 1)
                throw new NotFoundException("Сессия подтверждения почты не найдена!");

            var session = await _emailRepository.GetById(model.SessionId).ConfigureAwait(false);
            if (session == null)
                throw new NotFoundException("Сессия подтверждения почты не найдена!");

            if (session.DateExpire < DateTimeOffset.Now)
                throw new TimeOutSessionException("Сессия подтвердения почты устарела! Повторите процесс с начала.");

            if (session.VerificationCode != model.VerificationCode)
                return false;

            var user = await this.GetUserById(session.Owner.Id).ConfigureAwait(false);
            user.IsEmailVerified = true;
            await _userRepository.Update(user).ConfigureAwait(false);
            await _mailService.SendEmailAsync(
                new SuccessEmailVerificationMessage(user.Email, user.Name)).ConfigureAwait(false);
            return true;
        }

        private async Task<User> GetUserById(int id)
        {
            if (id < 1)
                throw new NotFoundException("Пользователь не найден!");

            var user = await _userRepository.GetById(id).ConfigureAwait(false);
            if (user == null)
                throw new NotFoundException("Пользователь не найден!");
            return user;
        }
    }
}
