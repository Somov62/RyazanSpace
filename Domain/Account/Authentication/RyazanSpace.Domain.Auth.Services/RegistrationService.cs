using RyazanSpace.DAL.Client.Repositories.Account;
using RyazanSpace.Domain.Auth.DTO;
using RyazanSpace.Domain.Auth.Exceptions;
using System.ComponentModel.DataAnnotations;
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
        /// <exception cref="UserAlreadyExistsException"></exception>
        public async Task<UserDTO> Register(RegRequestDTO model)
        {
        
            if (await _repository.ExistName(model.Name).ConfigureAwait(false))
                throw new UserAlreadyExistsException("Пользователь с таким именем уже существует");
            if (await _repository.ExistEmail(model.Email).ConfigureAwait(false))
                throw new UserAlreadyExistsException("На указанную почту уже зарегистрирован аккаунт");

            var user = model.MapToEntity();
            var createdUser = await _repository.Add(user).ConfigureAwait(false);
            return new UserDTO(createdUser);
        }
        
    }
}