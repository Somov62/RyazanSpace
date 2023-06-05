using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.Domain.Groups.DTO
{
    public class PostDTO : EntityDTO<Post>
    {
        public PostDTO() { }
        public PostDTO(Post post) : base(post) { }

        public string Text { get; set; }

        public GroupDTO Group { get; set; }

        public List<CloudResourceDTO> Resources { get; set; }

        public override Post MapToEntity()
        {
            return new Post()
            {
                Text = Text,
                Group = Group.MapToEntity(),
                Resources = Resources.ConvertAll(p => p.MapToEntity())
            };
        }

        public override Post UpdateEntity(Post entity)
        {
            entity.Text = Text;
            Group.UpdateEntity(entity.Group);
            entity.Resources = Resources.ConvertAll(p => p.MapToEntity());
            return entity;
        }

        protected override void InitByEntity(Post entity) 
        {
            Text = entity.Text;
            Group = new GroupDTO(entity.Group);
            Resources = entity.Resources.ConvertAll(p => new CloudResourceDTO(p));
        }
    }
}
