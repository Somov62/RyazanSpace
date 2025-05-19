using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Groups;

namespace RyazanSpace.DAL.API.Data
{
    public class RyazanSpaceDbInitializer
    {
        private RyazanSpaceDbContext _db;

        public RyazanSpaceDbInitializer(RyazanSpaceDbContext db) => _db = db;

        public void Initialize() 
        {
            _db.Database.Migrate();

            //InitializeTestUsers();
            //InitializeTestGroups();
            //
            //_db.SaveChanges();
        }

        private void InitializeTestUsers()
        {
            if (_db.Users.Any()) return;

            var entities = new User[5];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new User()
                {
                    Email = $"user{i}@mail.com",
                    Name = $"user{i}",
                    Password = "81dc9bdb52d04dc20036dbd8313ed055",
                    IsEmailVerified = true,
                    RegDate = DateTimeOffset.Now,
                    Status = "status"
                };
            }
            _db.Users.AddRange(entities);
            var owner = new User()
            {
                Email = $"misha2003980@gmail.com",
                Name = $"Михаил",
                Password = "61fd809f2d7cfdd91cddc057f3ab65f1",
                IsEmailVerified = true,
                RegDate = DateTimeOffset.Now,
                Status = "amogus"
            };
            _db.Users.Add(owner);
        }

        private void InitializeTestGroups()
        {
            if (_db.Groups.Any()) return;

            var logo = _db.CloudResources.Find(1021);
            var entities = new Group[10];
            for (int i = 0; i < entities.Length; i++)
            {
                entities[i] = new Group()
                {
                    Name = $"Группа под номером {i}",
                    OwnerId = i % 2 == 0 ? 6 : 1,
                    Description = $"Это группа очень интересная под номером {i}",
                    RegDate = DateTimeOffset.Now
                    //Logo = logo
                };
            }
            _db.Groups.AddRange(entities);
        }
    }
}
