using Microsoft.EntityFrameworkCore;
using TweeterDB.Entity;

namespace TweeterDB.Context
{
    public class TweetContext : DbContext
    {
        public TweetContext(DbContextOptions<TweetContext> options): base(options)
        {
        }

        public DbSet<Tweet> Tweets { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.UserId);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
                entity.Property(e => e.LasName).IsRequired().HasMaxLength(100);
                entity.HasMany(u => u.Followers)
                     .WithMany()
                     .UsingEntity(j => j.ToTable("UserFollows"));
            });

            modelBuilder.Entity<Tweet>(entity =>
            {
                entity.HasKey(t => t.TweetId);
                entity.Property(t => t.Message)
                      .IsRequired()
                      .HasMaxLength(280);
                entity.Property(t => t.CreatedAt)
                      .IsRequired();

                entity.HasOne(t => t.User)
                      .WithMany(u => u.Tweets)
                      .HasForeignKey(t => t.UserId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Aquí puedes añadir más configuraciones, como datos iniciales (seeding), etc.
            // Ejemplo de datos iniciales (seeding)
            modelBuilder.Entity<User>().HasData(
                new User {UserId = 1,  Name = "Pablo", LasName = "De Filippis" }
            );
        }
    }
}
