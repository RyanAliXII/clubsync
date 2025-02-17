using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace ClubSync.Api.Database
{

    public class ClubSyncIdentityDbContext : IdentityDbContext<User, Role, Guid, UserClaim, UserRole, UserLogin, RoleClaim, UserToken>
    {
        public ClubSyncIdentityDbContext(DbContextOptions<ClubSyncIdentityDbContext> options)
      : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);

            builder.Entity<User>(entity =>
            {
                entity.ToTable(name: "clubsync_user");
            });
            builder.Entity<Role>(entity =>
            {
                entity.ToTable(name: "clubsync_role");
            });
            builder.Entity<UserRole>(entity =>
            {
                entity.ToTable("clubsync_user_role");

            });
            builder.Entity<UserClaim>(entity =>
            {
                entity.ToTable("clubsync_user_claim");

            });
            builder.Entity<UserLogin>(entity =>
            {
                entity.ToTable("clubsync_user_login");
            });
            builder.Entity<RoleClaim>(entity =>
            {
                entity.ToTable("clubsync_role_claim");

            });
            builder.Entity<UserToken>(entity =>
            {
                entity.ToTable("clubsync_user_token");
                //in case you chagned the TKey type
                // entity.HasKey(key => new { key.UserId, key.LoginProvider, key.Name });

            });
        }
    }
    public class User : IdentityUser<Guid>
    {
        public string GivenName { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;

    }
    public class Role : IdentityRole<Guid> { }
    public class RoleClaim : IdentityRoleClaim<Guid> { }
    public class UserClaim : IdentityUserClaim<Guid> { }
    public class UserLogin : IdentityUserLogin<Guid> { }
    public class UserRole : IdentityUserRole<Guid> { }
    public class UserToken : IdentityUserToken<Guid> { }


}