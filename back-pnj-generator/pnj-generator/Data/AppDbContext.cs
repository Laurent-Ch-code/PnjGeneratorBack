using Microsoft.EntityFrameworkCore;
using pnj_generator.Models;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace pnj_generator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Universe> Universes { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Universe>(entity =>
            {
                entity.ToTable("universes");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Era).HasMaxLength(100);
                entity.Property(u => u.DiceRule).HasMaxLength(50);
                // Description : on laisse libre (text)
            });
        }

    }
}
