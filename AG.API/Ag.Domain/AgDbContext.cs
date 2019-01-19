using Ag.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Ag.Domain
{
    public class AgDbContext : DbContext
    {
        public AgDbContext(DbContextOptions<AgDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRelation>()
                .HasKey(e => new { e.FromId, e.ToId });

            modelBuilder.Entity<UserRelation>()
                .HasOne(e => e.UserFrom)
                .WithMany(e => e.RelatedTo)
                .HasForeignKey(e => e.FromId);

            modelBuilder.Entity<UserRelation>()
                .HasOne(e => e.UserTo)
                .WithMany(e => e.RelatedFrom)
                .HasForeignKey(e => e.ToId);
        }

        public DbSet<User> Users { get; set; }

        public DbSet<UserRelation> UserRelations { get; set; }

        public DbSet<WorkDay> WorkDays { get; set; }

        public DbSet<IncomeEntry> IncomeEntries { get; set; }

        public DbSet<IncomeChunk> IncomeChunks { get; set; }
    }
}
