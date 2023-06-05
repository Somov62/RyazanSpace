using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;

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
                Resources = Resources.ConvertAll(p => p.MapToEntity())
            };
        }

        public override Post UpdateEntity(Post entity)
        {
            entity.Text = Text;
            entity.GroupId = GroupId;
            entity.Resources = Resources.ConvertAll(p => p.MapToEntity());
            return entity;
        }

        protected override void InitByEntity(Post entity) { }
    }
}
