using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.Core.DTO;
using RyazanSpace.Core.Validation;

namespace RyazanSpace.Domain.Auth.DTO
{
    public class UserDTO : EntityDTO<User>
    {
        public UserDTO() { }
        public UserDTO(User entity) : base(entity) { }

        public int Id { get; set; }

        [Email]
        public string Email { get; set; }

        public string Name { get; set; }

        //public string Password { get; set; }

        //public bool IsEmailVerified { get; set; }

        //public ImageDTO ProfileImage { get; set; }

        public override User MapToEntity() => new() 
        { 
            Id = this.Id,
            Email = this.Email, 
            Name = this.Name 
        };

        public override User UpdateEntity(User entity)
        {
            entity.Email = this.Email;
            entity.Name = this.Name;
            return entity;
        }

        protected override void InitByEntity(User entity)
        {
            this.Id = entity.Id;
            this.Email = entity.Email;
            this.Name = entity.Name;
        }
    }
}