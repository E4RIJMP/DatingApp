using API.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class DataContext(DbContextOptions options) : IdentityDbContext<AppUser, AppRole, int,
    IdentityUserClaim<int>, AppUserRole, IdentityUserLogin<int>, IdentityRoleClaim<int>,
    IdentityUserToken<int>>(options)
{
    public DbSet<UserLike> Likes { get; set; }
    public DbSet<Message> Messages { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>()
            .HasMany(user => user.UserRoles)
            .WithOne(userRole => userRole.User)
            .HasForeignKey(userRole => userRole.UserId)
            .IsRequired();

        modelBuilder.Entity<AppRole>()
            .HasMany(role => role.UserRoles)
            .WithOne(userRole => userRole.Role)
            .HasForeignKey(userRole => userRole.RoleId)
            .IsRequired();

        modelBuilder.Entity<UserLike>()
            .HasKey(like => new { like.SourceUserId, like.TargetUserId });

        modelBuilder.Entity<UserLike>()
            .HasOne(like => like.SourceUser)
            .WithMany(user => user.LikedUser)
            .HasForeignKey(like => like.SourceUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<UserLike>()
            .HasOne(like => like.TargetUser)
            .WithMany(user => user.LikedByUsers)
            .HasForeignKey(like => like.TargetUserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.Recipient)
            .WithMany(message => message.MessagesReceived)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Message>()
            .HasOne(message => message.Sender)
            .WithMany(message => message.MessagesSent)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
