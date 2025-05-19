using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Base;

namespace RyazanSpace.DAL.Entities.Groups
{
    public class Like : BaseEntity
    {
        public int? UserId { get; set; }
        public User User { get; set; }

        public int? PostId { get; set; }
        public Post Post { get; set; }
    }
}
