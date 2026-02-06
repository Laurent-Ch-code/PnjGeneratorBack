using Microsoft.EntityFrameworkCore;
using pnj_generator.Models;
using pnj_generator.Models.Features;

namespace pnj_generator.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Universe> Universes { get; set; } = default!;
        public DbSet<Weapons> Weapons { get; set; } = default!;
        public DbSet<Equipment> Equipments { get; set; } = default!;
        public DbSet<Protection> Protections { get; set; }
        public DbSet<Characteristic> Characteristics { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<Trait> Traits { get; set; }

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
                entity.Property(w => w.WeaponFireMode).IsRequired();

                // Relation obligatoire : Weapon → Universe
                entity.HasOne(w => w.Universe)
                      .WithMany(u => u.Weapons) // Universe doit avoir ICollection<Weapon> Weapons
                      .HasForeignKey(w => w.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade); // si l'univers est supprimé, les armes aussi
            });

            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                // Relation obligatoire : Equipment → Universe
                entity.HasOne(e => e.Universe)
                      .WithMany(u => u.Equipments) // Universe doit avoir ICollection<Equipment> Equipments
                      .HasForeignKey(e => e.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade); // si l'univers est supprimé, les équipements aussi
            });
            // PROTECTION
            modelBuilder.Entity<Protection>(entity =>
            {
                entity.ToTable("protections");
                entity.HasKey(p => p.Id);

                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Type).HasMaxLength(100);
                entity.Property(p => p.ArmorRating).HasMaxLength(100);
                entity.Property(p => p.Material).HasMaxLength(200);
                entity.Property(p => p.Weight).HasMaxLength(50);

                // Relation obligatoire : Protection → Universe
                entity.HasOne(p => p.Universe)
                      .WithMany()
                      .HasForeignKey(p => p.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // CHARACTERISTIC
            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.ToTable("characteristics");
                entity.HasKey(c => c.Id);

                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.Value).IsRequired().HasMaxLength(50);
                entity.Property(c => c.Modifier).HasMaxLength(50);

                // Relation obligatoire : Characteristic → Universe
                entity.HasOne(c => c.Universe)
                      .WithMany()
                      .HasForeignKey(c => c.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // SKILL
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("skills");
                entity.HasKey(s => s.Id);

                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
                entity.Property(s => s.RelatedCharacteristic).HasMaxLength(100);
                entity.Property(s => s.Bonus).HasMaxLength(100);
                entity.Property(s => s.Malus).HasMaxLength(200);
                entity.Property(s => s.Prerequisites).HasMaxLength(300);

                // Relation obligatoire : Skill → Universe
                entity.HasOne(s => s.Universe)
                      .WithMany()
                      .HasForeignKey(s => s.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // TRAIT
            modelBuilder.Entity<Trait>(entity =>
            {
                entity.ToTable("traits");
                entity.HasKey(t => t.Id);

                entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Type).HasMaxLength(100);
                entity.Property(t => t.Effect).IsRequired().HasMaxLength(500);
                entity.Property(t => t.Prerequisites).HasMaxLength(300);

                // Relation obligatoire : Trait → Universe
                entity.HasOne(t => t.Universe)
                      .WithMany()
                      .HasForeignKey(t => t.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

        }
    }
}
