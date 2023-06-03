using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbGroupRepository : DbNamedRepository<Group>
    {
        public DbGroupRepository(RyazanSpaceDbContext db) : base(db) { }

        protected override IQueryable<Group> Items => base.Items.OrderByDescending(p => p.Id);
    }
}
