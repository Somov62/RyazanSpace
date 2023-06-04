using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.Domain.Groups.DTO
{
    public class GroupDTO : EntityDTO<Group>
    {
        public GroupDTO() { }
        public GroupDTO(Group group) : base(group) { }

        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public DateTimeOffset RegDate { get; set; }

        public CloudResourceDTO Logo { get; set; }

        public bool IsSubscibed { get; set; }
        public bool IsOwner { get; set; }

        public int SubsCount { get; set; }

        public override Group MapToEntity()
        {
            return new Group()
            {
                Description = this.Description,
                Name = this.Name,
                RegDate = this.RegDate,
                Logo = this.Logo.MapToEntity()
            };
        }

        public override Group UpdateEntity(Group entity)
        {
            entity.Name = this.Name;
            entity.Description = this.Description;
            entity.RegDate = this.RegDate;
            this.Logo.UpdateEntity(entity.Logo);
            return entity;
        }

        protected override void InitByEntity(Group entity)
        {
            Description = entity.Description;
            Name = entity.Name;
            RegDate = entity.RegDate;
            Logo = new CloudResourceDTO(entity.Logo);
            Id = entity.Id;
        }
    }
}
