using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Repositories.Base;

namespace RyazanSpace.DAL.Repositories.Credentials
{
    public class DbUserTokenRepository : DbRepository<UserToken>
    {
        public DbUserTokenRepository(RyazanSpaceDbContext db) : base(db) { }


        public async Task<bool> ExistToken(string token, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item => item.Token == token, cancel).ConfigureAwait(false);
        }

        public async Task<UserToken> GetByToken(string token, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(item => item.Token == token, cancel).ConfigureAwait(false);
        }

        public async Task<UserToken> DeleteByToken(string token, CancellationToken cancel = default)
        {
            var item = await GetByToken(token, cancel).ConfigureAwait(false);
            if (item == null) return null;
            return await Delete(item, cancel).ConfigureAwait(false);
        }
    }
}
