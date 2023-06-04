using Microsoft.EntityFrameworkCore;
using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;

namespace RyazanSpace.DAL.Repositories.Base
{
    public class DbRepository<T> : DbBaseRepository<T>, IRepository<T> where T : Entity
    {
        public DbRepository(RyazanSpaceDbContext db) : base(db) { }

        public async Task<bool> ExistId(int id, CancellationToken cancel = default)
        {
            return await Items.AnyAsync(item => item.Id == id, cancel).ConfigureAwait(false);
        }

        public async Task<T> GetById(int id, CancellationToken cancel = default)
        {
            switch (Items)
            {
                case DbSet<T> set:
                    return await set.FindAsync(new object[] { id }, cancellationToken: cancel).ConfigureAwait(false);
                case { } items:
                    return await Items.FirstOrDefaultAsync(item => item.Id == id, cancel).ConfigureAwait(false);
                default:
                    throw new InvalidOperationException("Ошибка в определении источника данных");
            }

        }

        public override async Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            if (pageSize <= 0)
                return new Page<T>(Enumerable.Empty<T>(), pageSize, pageIndex, pageSize);

            IQueryable<T> query = Items;
            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0)
                return new Page<T>(Enumerable.Empty<T>(), 0, pageIndex, pageSize);

            if (query is not IOrderedQueryable<T>)
                query = query.OrderBy(item => item.Id);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);
            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page<T>(items, totalCount, pageIndex, pageSize);
        }

        public override async Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default)
        {
            if (count <= 0) return Enumerable.Empty<T>();


            IQueryable<T> query = Items switch
            {
                IOrderedQueryable<T> orderedQuery => orderedQuery,
                { } q => q.OrderBy(item => item.Id)
            };

            if (skip > 0) query = query.Skip(skip);
            return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
        }


        public async Task<T> DeleteById(int id, CancellationToken cancel = default)
        {
            var item = await GetById(id, cancel).ConfigureAwait(false);
            if (item == null) return null;
            return await Delete(item, cancel).ConfigureAwait(false);
        }


        public override async Task<T> Add(T item, CancellationToken cancel = default)
        {
            return await base.Update(item, cancel);
        }
    }
}
