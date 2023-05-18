using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Account
{
    public class DbUserRepository : DbNamedRepository<User>
    {
        public DbUserRepository(RyazanSpaceDbContext db) : base(db) { }

        public async Task<bool> ExistEmail(string email, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item => item.Email == email, cancel).ConfigureAwait(false);
        }

        public async Task<User> GetByEmail(string email, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(item => item.Email == email, cancel).ConfigureAwait(false);
        }
    }
}
