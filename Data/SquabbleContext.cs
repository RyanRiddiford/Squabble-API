using Microsoft.EntityFrameworkCore;
using Squabble.Models;
using Squabble.Models.Entities;

namespace Squabble.Data
{
    //DbContext for Squabble test
    public class SquabbleContext : DbContext
    {
        //Constructor
        public SquabbleContext(DbContextOptions<SquabbleContext> options) : base(options)
        {

        }

        public DbSet<User> Accounts { get; set; }
        public DbSet<Login> Logins { get; set; }
        public DbSet<Server> Servers { get; set; }
        public DbSet<Channel> Channels { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<ServerAdmin> ServerAdmins { get; set; }
        public DbSet<ServerMember> ServerMembers { get; set; }
        public DbSet<ChannelMember> ChannelMembers { get; set; }

        public DbSet<ServerOwner> ServerOwners { get; set; }

        public DbSet<FriendRequest> FriendRequests { get; set; }

        public DbSet<Friendship> Friendships { get; set; }

        public DbSet<KanbanItem> KanbanItems { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Login>()
                .HasKey(c => new { c.Email, c.UserName });
            modelBuilder.Entity<ServerOwner>()
                .HasKey(c => new { c.UserId, c.ServerID });
            modelBuilder.Entity<ServerAdmin>()
                .HasKey(c => new { c.UserId, c.ServerID });
            modelBuilder.Entity<ServerMember>()
                .HasKey(c => new { c.UserID, c.ServerID });
            modelBuilder.Entity<ChannelMember>()
                .HasKey(c => new { c.UserID, c.ChannelId });

            modelBuilder.Entity<FriendRequest>(entity =>
            {
                entity.HasKey(c => new { c.SenderID, c.ReceiverID });
                entity.HasOne(x => x.Sender)
                    .WithMany(x => x.FriendRequests)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });
            modelBuilder.Entity<Friendship>(entity =>
            {
                entity.HasKey(c => new { c.UserID, c.FriendID });
                entity.HasOne(x => x.User)
                    .WithMany(x => x.Friends)
                    .IsRequired(false)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Seed();
        }
    }
}
