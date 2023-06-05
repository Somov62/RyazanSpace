using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.Domain.Profile.DTO
{
    public class ProfileDTO : EntityDTO<User>
    {
        public ProfileDTO() { }

        public ProfileDTO(User user) : base(user) { }

        public int Id { get; set; }
        public string Name { get; set; }

        public CloudResourceDTO Avatar { get; set; }

        public string Status { get; set; }

        public DateTimeOffset RegDate { get; set; }

        public override User MapToEntity()
        {
            return new User()
            {
                Id = Id,
                Name = Name,
                Avatar = Avatar?.MapToEntity() as CloudResource,
                RegDate = RegDate,
                Status = Status
            };
        }

        public override User UpdateEntity(User entity)
        {
            entity.Id = Id;
            entity.Name = Name;
            entity.Avatar = Avatar?.MapToEntity() as CloudResource;
            entity.RegDate = RegDate;
            entity.Status = Status;
            return entity;
        }

        protected override void InitByEntity(User entity)
        {
            Id = entity.Id;
            Name = entity.Name;
            Avatar = new CloudResourceDTO(entity.Avatar);
            RegDate = entity.RegDate;
            Status = entity.Status;
        }
    }
}
