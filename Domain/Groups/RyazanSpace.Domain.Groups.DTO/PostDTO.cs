using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.Domain.Groups.DTO
{
    public class PostDTO : EntityDTO<Post>
    {
        public PostDTO() { }
        public PostDTO(Post post) : base(post) { }

        public string Text { get; set; }
        public DateTimeOffset CreationTime { get; set; }

        public GroupDTO Group { get; set; }

        public ICollection<CloudResourceDTO> Resources { get; set; } = new List<CloudResourceDTO>();

        public override Post MapToEntity()
        {
            var post = new Post()
            {
                Text = Text,
                CreationTime = CreationTime,
                Group = Group?.MapToEntity(),
            };
            if (Resources != null)
            {
                post.Resources = new List<CloudResource>();
                foreach (var item in Resources)
                {
                    post.Resources.Add(item.MapToEntity() as CloudResource);
                }
            }
            return post;
        }

        public override Post UpdateEntity(Post entity)
        {
            entity.Text = Text;
            entity.CreationTime = CreationTime;
            Group?.UpdateEntity(entity.Group);
            if (Resources != null)
            {
                entity.Resources = new List<CloudResource>();
                foreach (var item in Resources)
                {
                    entity.Resources.Add(item.MapToEntity() as CloudResource);
                }
            }
            return entity;
        }

        protected override void InitByEntity(Post entity)
        {
            Text = entity.Text;
            CreationTime = entity.CreationTime;
            if (entity.Group != null)
                Group = new GroupDTO(entity.Group);
            Resources = entity.Resources?.ConvertAll(p => new CloudResourceDTO(p));
        }
    }
}
