using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Repositories.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RyazanSpace.DAL.Repositories.Groups
{
    public class DbLikesRepository : DbBaseRepository<Like>
    {
        public DbLikesRepository(RyazanSpaceDbContext db) : base(db) { }
    }
}
