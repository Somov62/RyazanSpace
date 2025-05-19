using Microsoft.EntityFrameworkCore;
using RyazanSpace.DAL.Entities.Account;
using RyazanSpace.DAL.Entities.Credentials;
using RyazanSpace.DAL.Entities.Groups;
using RyazanSpace.DAL.Entities.Resources.Base;

namespace RyazanSpace.DAL
{
    public class RyazanSpaceDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<UserToken> UserTokens { get; set; }
        public DbSet<EmailVerificationSession> EmailVerificationSessions { get; set; }
        public DbSet<ResetPasswordSession> ResetPasswordSessions { get; set; }
        public DbSet<CloudResource> CloudResources { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<GroupSubscriber> Subscribers { get; set; }
        public DbSet<Like> Likes { get; set; }

        public RyazanSpaceDbContext(DbContextOptions<RyazanSpaceDbContext> options) : base(options) { }


        protected override void OnModelCreating(ModelBuilder model)
        {
            base.OnModelCreating(model);

            model.Entity<CloudResource>().Property(p => p.Type).HasConversion<int>();

            model.Entity<CloudResource>()
                .HasMany<User>()
                .WithOne(p => p.Avatar);

            model.Entity<CloudResource>()
                .HasMany<Group>()
                .WithOne(p => p.Logo);
            //model.Entity<CloudResource>()
            //    .HasMany<Post>()
            //    .WithMany(p => p.Resources);
            model.Entity<Post>()
                .HasMany<CloudResource>(p => p.Resources)
                .WithMany();

            model.Entity<User>().HasIndex(nameof(User.Name)).IsUnique();
            model.Entity<User>().HasIndex(nameof(User.Email)).IsUnique();
            model.Entity<User>().Navigation(p => p.Avatar).AutoInclude();
            //model.Entity<User>().ToTable(p => p.HasCheckConstraint("Avatar", "AvatarId = 1"));


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
               .HasMany<CloudResource>()
               .WithOne(p => p.Owner)
               .OnDelete(DeleteBehavior.Cascade);

            model.Entity<User>()
              .HasMany<Group>()
              .WithOne(p => p.Owner)
              .OnDelete(DeleteBehavior.SetNull);

            model.Entity<Group>().Navigation(p => p.Owner).AutoInclude();
            model.Entity<Group>().Navigation(p => p.Logo).AutoInclude();

            model.Entity<Group>()
              .HasMany<Post>()
              .WithOne(p => p.Group)
              .OnDelete(DeleteBehavior.Cascade);

            model.Entity<Post>().Navigation(p => p.Resources).AutoInclude();
            model.Entity<Post>().Navigation(p => p.Group).AutoInclude();

            model.Entity<UserToken>().Navigation(p => p.Owner).AutoInclude();
            model.Entity<UserToken>().HasIndex(nameof(UserToken.Token)).IsUnique();

            model.Entity<EmailVerificationSession>().Navigation(p => p.Owner).AutoInclude();

            model.Entity<ResetPasswordSession>().Navigation(p => p.Owner).AutoInclude();


            model.Entity<Like>().HasKey(p => new { p.UserId, p.PostId });
            model.Entity<GroupSubscriber>().HasKey(p => new { p.UserId, p.GroupId});
            //model.Entity<CloudResource>().Navigation(p => p.Owner).AutoInclude();

        }
    }
}
