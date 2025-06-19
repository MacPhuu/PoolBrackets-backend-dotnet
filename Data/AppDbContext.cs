using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<Player> Players { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Match> Matches { get; set; }
        public DbSet<PlayerHistory> PlayerHistories { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Match>()
                .HasOne(m => m.Event)
                .WithMany()  
                .HasForeignKey(m => m.EventId)
                .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Match>()
                .HasOne(m => m.FirstPlayer)
                .WithMany()
                .HasForeignKey(m => m.FirstPlayerId)
                .OnDelete(DeleteBehavior.Restrict); 

            modelBuilder.Entity<Match>()
                .HasOne(m => m.SecondPlayer)
                .WithMany()
                .HasForeignKey(m => m.SecondPlayerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<PlayerHistory>()
                .HasOne(ph => ph.Player)
                .WithMany()
                .HasForeignKey(ph => ph.PlayerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<PlayerHistory>()
                .HasOne(ph => ph.Event)
                .WithMany()
                .HasForeignKey(ph => ph.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
