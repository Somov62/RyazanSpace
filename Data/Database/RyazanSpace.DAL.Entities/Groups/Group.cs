using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.DAL.Entities.Groups
{
    public class Group : NamedEntity
    {
        public string Description { get; set; }

        public DateTimeOffset RegDate { get; set; }

        public User Owner { get; set; }

        public int? OwnerId { get; set; }

        public CloudResource Logo { get; set; }

    }
}
