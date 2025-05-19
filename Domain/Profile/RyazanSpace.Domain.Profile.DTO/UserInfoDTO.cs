using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.Domain.Profile.DTO
{
    public class UserInfoDTO : EntityDTO<User>
    {
        public UserInfoDTO() { }

        public UserInfoDTO(User user) : base(user) { }

        public int Id { get; set; }
        public string Name { get; set; }
        public CloudResource ProfileImage { get; set; }


        public override User MapToEntity()
        {
            return new User()
            {
                Id = this.Id,
                Name = this.Name,
                Avatar = this.ProfileImage
            };
        }

        public override User UpdateEntity(User entity)
        {
            entity.Name = this.Name;
            entity.Avatar = this.ProfileImage;
            return entity;
        }

        protected override void InitByEntity(User entity)
        {
            Name = entity.Name;
            ProfileImage = entity.Avatar;
            Id = entity.Id;
        }
    }
}