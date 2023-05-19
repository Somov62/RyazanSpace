using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Entities.Resources;

namespace RyazanSpace.DAL
{
    public class RyazanSpaceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<EmailVerificationSession> EmailVerificationSessions { get; set; }
        public DbSet<ResetPasswordSession> ResetPasswordSessions { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Document> Documents { get; set; }

        public RyazanSpaceDbContext(DbContextOptions<RyazanSpaceDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<User>().Navigation(p => p.ProfileImage).AutoInclude();
            model.Entity<User>().HasIndex(nameof(User.Name)).IsUnique();
            model.Entity<User>().HasIndex(nameof(User.Email)).IsUnique();

            model.Entity<User>()
                .HasMany<UserToken>()
                .WithOne(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
                .HasMany<EmailVerificationSession>()
                .WithOne(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
                .HasMany<ResetPasswordSession>()
                .WithOne(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
                .HasMany<Image>()
                .WithOne(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
                .HasMany<Document>()
                .WithOne(p => p.Owner)
                .OnDelete(DeleteBehavior.Cascade);


            model.Entity<UserToken>().Navigation(p => p.Owner).AutoInclude();
            model.Entity<UserToken>().HasIndex(nameof(UserToken.Token)).IsUnique();


            model.Entity<Image>().Navigation(p => p.Owner).AutoInclude();

            model.Entity<Document>().Navigation(p => p.Owner).AutoInclude();

            model.Entity<EmailVerificationSession>().Navigation(p => p.Owner).AutoInclude();

            model.Entity<ResetPasswordSession>().Navigation(p => p.Owner).AutoInclude();
        }

    }
}
