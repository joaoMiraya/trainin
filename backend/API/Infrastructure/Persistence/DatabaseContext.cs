using Microsoft.EntityFrameworkCore;
using API.Domain.Entities;

namespace API.Infrastructure.Persistence;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options) { }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Reply> Replies { get; set; }
    public DbSet<Like> Likes { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<Report> Reports { get; set; }
    public DbSet<Friendship> Friendships { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasMany(u => u.Posts).WithOne(p => p.User).HasForeignKey(p => p.UserId);
            entity.HasMany(u => u.Comments).WithOne(c => c.User).HasForeignKey(c => c.UserId);
            entity.HasMany(u => u.Replies).WithOne(r => r.User).HasForeignKey(r => r.UserId);
            entity.HasMany(u => u.Likes).WithOne(l => l.User).HasForeignKey(l => l.UserId);
            entity.HasMany(u => u.Notifications).WithOne(n => n.User).HasForeignKey(n => n.UserId);
            entity.HasMany(u => u.Reports).WithOne(r => r.User).HasForeignKey(r => r.UserId);
            entity.HasMany(u => u.SentFriendRequests)
                  .WithOne(f => f.Requester)
                  .HasForeignKey(f => f.RequesterId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasMany(u => u.ReceivedFriendRequests)
                  .WithOne(f => f.Addressee)
                  .HasForeignKey(f => f.AddresseeId)
                  .OnDelete(DeleteBehavior.Restrict);
            entity.HasOne(u => u.Role)
                  .WithMany(r => r.Users)
                  .HasForeignKey(u => u.RoleId)
                  .OnDelete(DeleteBehavior.Restrict);
        });

        // Role
        modelBuilder.Entity<Role>().HasKey(r => r.Id);

        // Post
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.HasMany(p => p.Comments).WithOne(c => c.Post).HasForeignKey(c => c.PostId);
            entity.HasMany(p => p.Likes).WithOne(l => l.Post).HasForeignKey(l => l.PostId);
            entity.HasMany(p => p.Reports).WithOne(r => r.Post).HasForeignKey(r => r.PostId);
        });

        // Comment
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.HasMany(c => c.Replies).WithOne(r => r.Comment).HasForeignKey(r => r.CommentId);
            entity.HasMany(c => c.Reports).WithOne(r => r.Comment).HasForeignKey(r => r.CommentId);
        });

        // Reply
        modelBuilder.Entity<Reply>(entity =>
        {
            entity.HasKey(r => r.Id);
            entity.HasMany(r => r.Reports).WithOne(rp => rp.Reply).HasForeignKey(rp => rp.ReplyId);
        });

        // Like
        modelBuilder.Entity<Like>().HasKey(l => l.Id);

        // Notification
        modelBuilder.Entity<Notification>().HasKey(n => n.Id);

        // Report
        modelBuilder.Entity<Report>(entity =>
        {
            entity.HasKey(r => r.Id);

            entity.HasOne(r => r.Post)
                  .WithMany(p => p.Reports)
                  .HasForeignKey(r => r.PostId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Comment)
                  .WithMany(c => c.Reports)
                  .HasForeignKey(r => r.CommentId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Reply)
                  .WithMany(rp => rp.Reports)
                  .HasForeignKey(r => r.ReplyId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // Friendship
        modelBuilder.Entity<Friendship>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Status).HasConversion<string>();
        });
    }
}
