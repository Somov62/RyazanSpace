using RyazanSpace.DAL.WebApiClients.Repositories.Account;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.Text;
using System.Text.RegularExpressions;

namespace RyazanSpace.Domain.Auth.Services
{
    public class RegistrationService
    {
        private readonly WebUserRepository _repository;

        public RegistrationService(WebUserRepository repository) => _repository = repository;


        /// <summary>
        /// Регистрация пользователя в системе
        /// </summary>
        /// <param name="model"><see cref="RegRequestDTO"/> - модель первичных данных</param>
        /// <returns><see cref="UserDTO"/> - модель пользоваетеля</returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="UserAlreadyExistsException"></exception>
        public async Task<UserDTO> Register(RegRequestDTO model)
        {
            if (!ValidateRegisterData(model, out string errors))
                throw new ArgumentException(errors, nameof(model));

            if (await _repository.ExistName(model.Name).ConfigureAwait(false))
                throw new UserAlreadyExistsException("Пользователь с таким именем уже существует");
            if (await _repository.ExistEmail(model.Email).ConfigureAwait(false))
                throw new UserAlreadyExistsException("На указанную почту уже зарегистрирован аккаунт");

            var user = model.MapToEntity();
            var createdUser = await _repository.Add(user).ConfigureAwait(false);
            return new UserDTO(createdUser);
        }
        
        private bool ValidateRegisterData(RegRequestDTO model, out string errors)
        {
            StringBuilder sb = new();
            
            if (string.IsNullOrWhiteSpace(model.Name)) 
                sb.AppendLine("Укажите имя пользователя");
            if (string.IsNullOrWhiteSpace(model.Email))
                sb.AppendLine("Укажите электронную почту");
            if (string.IsNullOrWhiteSpace(model.Password))
                sb.AppendLine("Укажите пароль");

            if (!ValidateEmail())
                sb.AppendLine("Неверный формат электронной почты");
            if (!ValidateMD5())
                sb.AppendLine("Пароль должен быть в формате MD5");

            errors = sb.ToString();
            return errors.Length == 0;

            bool ValidateEmail()
            {
                string pattern = @"^[\w!#$%&'*+\-/=?\^_`{|}~]+(\.[\w!#$%&'*+\-/=?\^_`{|}~]+)*"
                          + "@"
                          + @"((([\w]+([-\w]*[\w]+)*\.)+[a-zA-Z]+)|"
                          + @"((([01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]).){3}[01]?[0-9]{1,2}|2[0-4][0-9]|25[0-5]))\z";
                return Regex.IsMatch(model.Email, pattern);
            }

            bool ValidateMD5() => Regex.IsMatch(model.Password, "^[0-9a-fA-F]{32}$", RegexOptions.Compiled);
        }
    }
}