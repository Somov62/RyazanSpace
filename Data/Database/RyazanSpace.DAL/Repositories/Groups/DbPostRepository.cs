using Microsoft.EntityFrameworkCore;
using RyazanSpace.Core.DTO;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;
using RyazanSpace.DAL.Repositories.Base;
using RyazanSpace.Interfaces.Repositories;
using System.Diagnostics;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbPostRepository : DbRepository<Post>
    {
        public DbPostRepository(RyazanSpaceDbContext db) : base(db) { }

        protected override IQueryable<Post> Items => base.Items.OrderByDescending(p => p.Id);



        public async Task<IPage<Post>> GetByGroupPage(int groupdId, int pageIndex, int pageSize, CancellationToken cancel = default)
        {
            if (pageSize <= 0)
                return new Page<Post>(Enumerable.Empty<Post>(), pageSize, pageIndex, pageSize);

            IQueryable<Post> query = Items.Where(p => p.GroupId == groupdId).OrderByDescending(p => p.Id);
            var totalCount = await query.CountAsync(cancel).ConfigureAwait(false);
            if (totalCount == 0)
                return new Page<Post>(Enumerable.Empty<Post>(), 0, pageIndex, pageSize);

            if (pageIndex > 0)
                query = query.Skip(pageIndex * pageSize);
            query = query.Take(pageSize);
            var items = await query.ToArrayAsync(cancel).ConfigureAwait(false);

            return new Page<Post>(items, totalCount, pageIndex, pageSize);
        }

        public override async Task<Post> Add(Post item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            if (item.Group != null)
            {
                item.GroupId = item.Group.Id;
                item.Group = null;
            }

            if (item.Resources != null)
            {
                var res = item.Resources;
                for (int i = 0; i < res.Count; i++)
                {
                    var entity = await _db.CloudResources.FirstOrDefaultAsync(p => p.Id == res[i].Id, cancellationToken: cancel);
                    if (entity != null)
                    {
                        _db.Entry(entity).State = EntityState.Unchanged;
                        res[i] = entity;
                    }
                }
               
            }
            var e = _db.ChangeTracker.Entries();
            foreach (var a in e)
            {
                Debug.WriteLine(a.Metadata.DisplayName() + a.State);
            }
            await _db.Posts.AddAsync(item, cancel);
             e = _db.ChangeTracker.Entries();
            foreach (var a in e)
            {
                Debug.WriteLine(a.Metadata.DisplayName() + a.State);
            }

            if (AutoSaveChanges)
            {
                await SaveChanges(cancel).ConfigureAwait(false);
                _db.Entry(item).Reload();
            }

            return item;
        }

        public override async Task<Post> Update(Post item, CancellationToken cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));

            var entity = await _db.Posts.FirstOrDefaultAsync(p => p.Id == item.Id, cancellationToken: cancel);
            _db.Attach(entity);

            entity.Text = item.Text;
            entity.GroupId = item.GroupId;
            entity.CreationTime = item.CreationTime;

            if (item.Group != null)
            {
                entity.GroupId = item.Group.Id;
                entity.Group = null;
            }

            if (item.Resources != null)
            {
                var list = new List<CloudResource>(entity.Resources);
                entity.Resources.Clear();
                foreach (var res in item.Resources)
                {
                    var entityRes = list.FirstOrDefault(p => p.Id == res.Id);
                    entity.Resources.Add(entityRes ?? res);
                }
            }

            if (AutoSaveChanges)
            {
                await SaveChanges(cancel).ConfigureAwait(false);
                _db.Entry(entity).Reload();
            }
            return entity;
        }
    }
}
