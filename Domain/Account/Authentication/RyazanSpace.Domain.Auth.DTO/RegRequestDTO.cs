using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.Interfaces.DTO;
using System.ComponentModel.DataAnnotations;

namespace RyazanSpace.Domain.Auth.DTO
{
    public class RegRequestDTO : EntityDTO<User>
    {
        public RegRequestDTO() { }
        public RegRequestDTO(User entity) : base(entity) { }

        [Required]
        public string Email { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Password { get; set; }

        public override User MapToEntity() => new()
        {
            Email = this.Email,
            Name = this.Name,
            Password = this.Password
        };

        public override User UpdateEntity(User entity)
        {
            entity.Email = this.Email;
            entity.Name = this.Name;
            entity.Password = this.Password;
            return entity;
        }

        protected override void InitByEntity(User entity)
        {
            this.Email = entity.Email;
            this.Name = entity.Name;
            this.Password = entity.Password;
        }
    }
}
