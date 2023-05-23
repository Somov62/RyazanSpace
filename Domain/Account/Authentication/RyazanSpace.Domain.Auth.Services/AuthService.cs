using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.DAL.Client.Repositories.Credentials;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.Domain.Auth.Mails;
using RyazanSpace.Core.Exceptions;
using RyazanSpace.Interfaces.Email;

namespace RyazanSpace.Domain.Auth.Services
{
    public class AuthService
    {
        private readonly WebUserTokenRepository _tokenRepository;
        private readonly WebUserRepository _userRepository;
        private readonly IEmailSender _mailService;

        public AuthService(
            WebUserTokenRepository tokenRepository,
            WebUserRepository userRepository,
            IEmailSender mailService)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }


        /// <summary>
        /// Метод аунтефикации по токену
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns>пользователя, владеющего данным токеном</returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task<int> UserByToken(string token, CancellationToken cancel = default)
        {
            var sessionToken = await _tokenRepository.GetByToken(token);
            if (sessionToken == null || sessionToken.DateExpire < DateTimeOffset.Now)
                throw new NotFoundException("Сессия не найдена!");
            return sessionToken.Id;
        }

        /// <summary>
        ///  /// <para>Метод для входа в аккаунт.</para>
        /// Создает токен доступа
        /// </summary>
        /// <param name="model">Конфиденциальные данные</param>
        /// <param name="breakAddress">эндпоинт отмены входа</param>
        /// <param name="senderIpAddress">ip отправителя</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="UserNotVerifiedException"></exception>
        public async Task<TokenResponseDTO> Login(
            AuthRequestDTO model, 
            string breakAddress,
            string senderIpAddress,
            CancellationToken cancel = default)
        {
            var user = await _userRepository.GetByEmail(model.Login, cancel).ConfigureAwait(false);
            user ??= await _userRepository.GetByName(model.Login, cancel).ConfigureAwait(false);

            if (user == null)
                throw new NotFoundException("Пользователь не найден!");

            if (user.Password != model.Password)
                throw new ArgumentException("Пароль неверен!");

            if (!user.IsEmailVerified)
                throw new UserNotVerifiedException("Ваша электронная почта не подтверждена!");

            UserToken sessionToken = CreateToken(user);
            sessionToken = await _tokenRepository.Add(sessionToken, cancel).ConfigureAwait(false);
            await _mailService.SendEmailAsync(
                new LoginMessage(
                    user.Email, 
                    breakAddress.Replace("ID", sessionToken.Id.ToString()), 
                    senderIpAddress), 
                cancel).ConfigureAwait(false);
            return new TokenResponseDTO(sessionToken.Token, sessionToken.DateExpire);
        }

        /// <summary>
        /// Метод для продления токена доступа
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        /// <exception cref="TimeOutSessionException"></exception>
        public async Task<TokenResponseDTO> RefreshToken(string token, CancellationToken cancel = default)
        {
            var sessionToken = await _tokenRepository.GetByToken(token, cancel).ConfigureAwait(false);
            if (sessionToken == null)
                throw new NotFoundException("Сессия не найдена!");

            if (sessionToken.DateExpire < DateTimeOffset.Now)
                throw new TimeOutSessionException("Токен доступа устарел! Повторите процедуру авторизации.");

            RefreshToken(sessionToken);
            await _tokenRepository.Update(sessionToken, cancel).ConfigureAwait(false);
            return new TokenResponseDTO(sessionToken.Token, sessionToken.DateExpire);
        }

        /// <summary>
        /// <para>Метод для выхода из аккаунта.</para>
        /// Токен доступа удаляется и больше недействителен.
        /// </summary>
        /// <param name="token"></param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task Logout(string token, CancellationToken cancel = default)
        {
            var sessionToken = await _tokenRepository.GetByToken(token, cancel).ConfigureAwait(false);
            if (sessionToken == null)
                throw new NotFoundException("Сессия не найдена!");

            await _tokenRepository.Delete(sessionToken, cancel).ConfigureAwait(false);
        }


        /// <summary>
        /// <para>Метод принудительного удаления токена</para>
        /// </summary>
        /// <param name="tokenId">ID токена в базе данных</param>
        /// <param name="cancel"></param>
        /// <returns></returns>
        /// <exception cref="NotFoundException"></exception>
        public async Task BreakToken(int tokenId, CancellationToken cancel = default)
        {
            var sessionToken = await _tokenRepository.GetById(tokenId, cancel).ConfigureAwait(false);
            if (sessionToken == null)
                throw new NotFoundException("Сессия не найдена!");

            await _tokenRepository.Delete(sessionToken, cancel).ConfigureAwait(false);
        }

        private UserToken CreateToken(User client)
        {
            TokenGenerator generator = new();
            UserToken sessionToken = new() { Owner = client };
            sessionToken.Token = generator.GenerateToken(client);

            return RefreshToken(sessionToken);
        }

        private UserToken RefreshToken(UserToken token)
        {
            token.DateExpire = DateTimeOffset.Now.AddDays(30);
            return token;
        }
    }
}
