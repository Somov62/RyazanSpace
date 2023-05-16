using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;

namespace RyazanSpace.DAL.API.Data
{
    public class RyazanSpaceDbInitializer
    {
        private RyazanSpaceDbContext _db;

        public RyazanSpaceDbInitializer(RyazanSpaceDbContext db) => _db = db;

        public void Initialize() 
        {
            _db.Database.Migrate();

            InitializeTestUsers();

            _db.SaveChanges();
        }

        private void InitializeTestUsers()
        {
            if (_db.Users.Any()) return;

            var users = new User[5];
            for (int i = 0; i < 5; i++)
            {
                users[i] = new User()
                {
                    Email = $"user{i}@mail.com",
                    Name = $"user{i}"
                };
            }
            _db.Users.AddRange(users);
        }

    }
}
