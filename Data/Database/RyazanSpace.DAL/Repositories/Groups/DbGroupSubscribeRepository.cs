using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbGroupSubscribeRepository : DbBaseRepository<GroupSubscriber>
    {
        public DbGroupSubscribeRepository(RyazanSpaceDbContext db) : base(db) { }


        public async Task<GroupSubscriber> GetById(int groupId, int userId, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(item => 
                item.GroupId == groupId && item.UserId == userId, cancel).ConfigureAwait(false);
        }

        public async Task<bool> Exist(int groupId, int userId, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item =>
                item.GroupId == groupId && item.UserId == userId, cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetGroupSubscribers(int groupId, CancellationToken cancel = default)
        {
            return await Items
                .Include(p => p.User)
                .Where(p => p.GroupId == groupId)
                .Select(p => p.User)
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<Group>> GetUserGroups(int userId, CancellationToken cancel = default)
        {
            return await Items
                .Include(p => p.Group)
                .Where(p => p.UserId == userId)
                .Select(p => p.Group)
                .ToArrayAsync(cancel)
                .ConfigureAwait(false);
        }

        public async Task<int> GetCountGroupSubscribers(int groupId, CancellationToken cancel = default)
        {
            return await Items.Include(p => p.User).Where(p => p.GroupId == groupId).CountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCountUserGroups(int userId, CancellationToken cancel = default)
        {
            return await Items.Include(p => p.Group).Where(p => p.UserId == userId).CountAsync(cancel).ConfigureAwait(false);
        }
    }
}
