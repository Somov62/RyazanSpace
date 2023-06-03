using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbPostRepository : DbRepository<Post>
    {
        public DbPostRepository(RyazanSpaceDbContext db) : base(db) { }

    }
}
