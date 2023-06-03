using Microsoft.EntityFrameworkCore;
using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Base;
using RyazanSpace.Interfaces.Repositories;
using System.Linq.Expressions;

namespace RyazanSpace.DAL.Repositories.Base
{
    public class DbBaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        private readonly RyazanSpaceDbContext _db;

        protected DbSet<T> Set { get; }

        protected virtual IQueryable<T> Items => Set;

        public bool AutoSaveChanges { get; set; } = true;

        public DbBaseRepository(RyazanSpaceDbContext db)
        {
            _db = db;
            Set = _db.Set<T>();
        }

        public async Task<bool> Exist(T item, CancellationToken cancel = default)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));

            return await Set.ContainsAsync(item, cancel); //Items.AnyAsync(i => i.Id == item.Id, cancel).ConfigureAwait(false);
        }

        public async Task<int> GetCount(CancellationToken cancel = default)
        {
            return await Items.CountAsync(cancel).ConfigureAwait(false);
        }

        public async Task<IEnumerable<T>> GetAll(CancellationToken cancel = default)
        {
            return await Items.ToArrayAsync(cancel).ConfigureAwait(false);
        }

        public virtual async Task<IEnumerable<T>> Get(int skip, int count, CancellationToken cancel = default)
        {
            if (count <= 0) return Enumerable.Empty<T>();

            IQueryable<T> query = Items;
            if (skip > 0) query = query.Skip(skip);
            return await query.Take(count).ToArrayAsync(cancel).ConfigureAwait(false);
        }

        public virtual async Task<IPage<T>> GetPage(int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            if (pageSize <= 0)
                return new Page<T>(Enumerable.Empty<T>(), pageSize, pageIndex, pageSize);

            IQueryable<T> query = Items;
            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0)
                return new Page<T>(Enumerable.Empty<T>(), 0, pageIndex, pageSize);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);
            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page<T>(items, totalCount, pageIndex, pageSize);
        }

        public async Task<IEnumerable<T>> GetByExpression(Expression<Func<T, bool>> predicate)
        {
            return await Items.AsNoTracking().Where(predicate).ToArrayAsync();
        }

        public async Task<T> Add(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            _db.Update(item);
            //await _db.AddAsync(item).ConfigureAwait(false);
            if (AutoSaveChanges)
                await SaveChanges(cancel).ConfigureAwait(false);

            return item;
        }

        public async Task<T> Update(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            _db.Update(item);
            if (AutoSaveChanges)
            {
                await SaveChanges(cancel).ConfigureAwait(false);
                _db.Entry(item).Reload();
            }

            return item;
        }

        public async Task<T> Delete(T item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            if (!await Exist(item, cancel))
                return null;

            _db.Remove(item);
            if (AutoSaveChanges)
                await SaveChanges(cancel).ConfigureAwait(false);

            return item;
        }
        public async Task<int> SaveChanges(CancellationToken cancel = default)
        {
            return await _db.SaveChangesAsync(cancel).ConfigureAwait(false);
        }

        
    }
}
