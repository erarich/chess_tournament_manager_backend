using chess_tournament_manager_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace chess_tournament_manager_backend.Data
{

    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) :
            base(options) { }

        public DbSet<Tournament> Tournament { get; set; } = default!;
        public DbSet<Player> Player { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tournament>()
                .HasOne(t => t.OwnerUser)
                .WithMany()
                .HasForeignKey(t => t.OwnerUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Player>()
                .HasOne(p => p.Tournament)
                .WithMany()
                .HasForeignKey(p => p.TournamentId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
