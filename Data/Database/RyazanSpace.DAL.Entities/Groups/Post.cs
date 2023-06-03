using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.DAL.Entities.Groups
{
    public class Post : Entity
    {
        public string Text { get; set; }

        public int GroupId { get; set; }
        public Group Group { get; set; }

        public DateTimeOffset CreationTime { get; set; }

        public List<CloudResource> Resources { get; set; }

    }
}
