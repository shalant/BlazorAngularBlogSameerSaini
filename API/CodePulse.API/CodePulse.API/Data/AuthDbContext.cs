using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CodePulse.API.Data;

public class AuthDbContext : IdentityDbContext
{
public AuthDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        var readerRoleId = "88ca765e-1fbc-4e28-977a-ebc78fad26a6";
        var writerRoleId = "6db869ab-4afb-493e-8740-f085bcf94170";

        // Create reader and writer roles
        var roles = new List<IdentityRole>()
        {
            new IdentityRole()
            {
                Id = readerRoleId,
                Name = "Reader",
                NormalizedName = "Reader".ToUpper(),
                ConcurrencyStamp = readerRoleId
            },
            new IdentityRole()
            {
                Id = writerRoleId,
                Name = "Writer",
                NormalizedName = "Writer".ToUpper(),
                ConcurrencyStamp = writerRoleId
            }
        };

        // seed the roles
        builder.Entity<IdentityRole>().HasData(roles);

        // Create an admin user
        var adminUserId = "1d5a15ee-722b-47e4-838a-cb14ccec75fb";
        var admin = new IdentityUser()
        {
            Id = adminUserId,
            UserName = "doug.rosenberg@gmail.com",
            Email = "doug.rosenberg@gmail.com",
            NormalizedEmail = "doug.rosenberg@gmail.com".ToUpper(),
            NormalizedUserName = "doug.rosenberg@gmail.com".ToUpper()
        };

        admin.PasswordHash = new PasswordHasher<IdentityUser>().HashPassword(admin, "Admin@123");

        builder.Entity<IdentityUser>().HasData(admin);

        // Give roles to admin

        var adminRoles = new List<IdentityUserRole<string>>()
        {
            new()
            {
                UserId = adminUserId,
                RoleId = readerRoleId
            },
            new()
            {
                UserId = adminUserId,
                RoleId = writerRoleId
            },
        };

        builder.Entity<IdentityUserRole<string>>().HasData(adminRoles);
    }
}
