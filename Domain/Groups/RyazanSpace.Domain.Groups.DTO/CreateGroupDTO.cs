using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.Domain.Groups.DTO
{
    public class CreateGroupDTO : EntityDTO<Group>
    {
        public CreateGroupDTO() { }

        public CreateGroupDTO(Group group) : base(group) { }


        public string Name { get; set; }

        public override Group MapToEntity()
        {
            return new Group() { Name = this.Name };
        }

        public override Group UpdateEntity(Group entity)
        {
            entity.Name = this.Name;
            return entity;
        }

        protected override void InitByEntity(Group entity)
        {
            Name = entity.Name;
        }
    }
}