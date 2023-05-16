using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.Repositories.Base
{
    public class DbNamedRepository<T> : DbRepository<T>, INamedRepository<T> where T : NamedEntity
    {
        public DbNamedRepository(RyazanSpaceDbContext db) : base(db)
        {
        }

        public async Task<bool> ExistName(string name, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item => item.Name == name, cancel).ConfigureAwait(false);
        }

        public async Task<T> GetByName(string name, CancellationToken cancel = default)
        {
            return await Items.FirstOrDefaultAsync(item => item.Name == name, cancel).ConfigureAwait(false);
        }

        public async Task<T> DeleteByName(string name, CancellationToken cancel = default)
        {
            var item = await GetByName(name, cancel).ConfigureAwait(false);
            if (item == null) return null;
            return await Delete(item, cancel).ConfigureAwait(false);
        }
    }
}
