using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.WebApiClients.Repositories.Account;
using RyazanSpace.DAL.WebApiClients.Repositories.Credentials;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using RyazanSpace.MailService;
using System.Threading.Channels;

namespace RyazanSpace.Domain.Auth.Services
{
    public class AuthService
    {
        private readonly WebUserTokenRepository _tokenRepository;
        private readonly WebUserRepository _userRepository;
        private readonly EmailSender _mailService;

        public AuthService(
            WebUserTokenRepository tokenRepository,
            WebUserRepository userRepository,
            EmailSender mailService)
        {
            _tokenRepository = tokenRepository;
            _userRepository = userRepository;
            _mailService = mailService;
        }


        public async Task<TokenResponseDTO> Login(AuthRequestDTO model, CancellationToken cancel = default)
        {
            var user = await _userRepository.GetByEmail(model.Login, cancel).ConfigureAwait(false);
            user ??= await _userRepository.GetByName(model.Login, cancel).ConfigureAwait(false);

            if (user == null)
                throw new NotFoundException("Пользователь не найден!");

            if (user.Password != model.Password)
                throw new ArgumentException("Пароль неверен!");

            UserToken sessionToken = CreateToken(user);
            await _tokenRepository.Add(sessionToken, cancel).ConfigureAwait(false);
            return new TokenResponseDTO(sessionToken.Token, sessionToken.DateExpire);
        }

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

        public async Task Logout(string token, CancellationToken cancel = default)
        {
            var sessionToken = await _tokenRepository.GetByToken(token, cancel).ConfigureAwait(false);
            if (sessionToken == null)
                throw new NotFoundException("Сессия не найдена!");

            await _tokenRepository.Delete(sessionToken, cancel).ConfigureAwait(false);
        }

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
