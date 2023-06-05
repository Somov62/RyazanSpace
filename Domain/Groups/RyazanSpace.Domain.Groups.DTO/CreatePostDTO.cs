using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.Domain.Groups.DTO
{
    public class CreatePostDTO : EntityDTO<Post>
    {
        public CreatePostDTO() { }

        public string Text { get; set; }

        public int GroupId { get; set; }

        public List<CloudResourceDTO> Resources { get; set; }

        public override Post MapToEntity()
        {
            return new Post()
            {
                Text = Text,
                GroupId = GroupId,
                Resources = Resources?.ConvertAll(p => p.MapToEntity() as CloudResource)
            };
        }

        public override Post UpdateEntity(Post entity)
        {
            entity.Text = Text;
            entity.GroupId = GroupId;
            entity.Resources = Resources?.ConvertAll(p => p.MapToEntity() as CloudResource);
            return entity;
        }

        protected override void InitByEntity(Post entity) { }
    }
}
