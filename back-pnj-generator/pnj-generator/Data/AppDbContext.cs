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
        public DbSet<Weapons> Weapons { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Universe>(entity =>
            {
                entity.ToTable("universes");
                entity.HasKey(u => u.Id);

                entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Era).HasMaxLength(100);
                entity.Property(u => u.DiceRule).HasMaxLength(50);
            });

            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.ToTable("weapons");
                entity.HasKey(w => w.Id);

                entity.Property(w => w.Name).IsRequired().HasMaxLength(200);
                entity.Property(w => w.weaponFireMode).IsRequired();

                // Relation obligatoire : Weapon → Universe
                entity.HasOne(w => w.Universe)
                      .WithMany(u => u.Weapons) // Universe doit avoir ICollection<Weapon> Weapons
                      .HasForeignKey(w => w.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade); // si l'univers est supprimé, les armes aussi
            });
        }
    }
}
