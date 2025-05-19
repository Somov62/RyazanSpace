using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbGroupRepository : DbNamedRepository<Group>
    {
        public DbGroupRepository(RyazanSpaceDbContext db) : base(db) { }

        protected override IQueryable<Group> Items => base.Items.OrderByDescending(p => p.Id);

        public async Task<IEnumerable<Group>> GetManagedGroups(int userId, CancellationToken cancel = default)
        {
            return await Items.Where(p => p.Owner.Id == userId).ToArrayAsync(cancel).ConfigureAwait(false);
        }

    }
}
