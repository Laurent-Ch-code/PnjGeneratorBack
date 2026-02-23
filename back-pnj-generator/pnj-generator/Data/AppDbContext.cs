using Microsoft.EntityFrameworkCore;
using pnj_generator.Models;
using pnj_generator.Models.Features;
using pnj_generator.Models.Features.Identities;
using pnj_generator.Models.Rules;

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
        public DbSet<Identity> Identities { get; set; }
        public DbSet<FragmentIdentity> FragmentIdentities { get; set; }

        // Tables dédiées pour chaque type d'info additionnelle
        public DbSet<Culture> Cultures { get; set; }
        public DbSet<Specie> Species { get; set; }
        public DbSet<Alignment> Alignments { get; set; }
        public DbSet<Origin> Origins { get; set; }
        public DbSet<ModifierRules> ModifierRules { get; set; }
        public DbSet<NPC> NPCs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // -------------------------
            // UNIVERSE
            // -------------------------
            modelBuilder.Entity<Universe>(entity =>
            {
                entity.ToTable("universes");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(200);
                entity.Property(u => u.Era).HasMaxLength(100);
                entity.Property(u => u.DiceRule).HasMaxLength(50);
            });

            // -------------------------
            // MODIFIER RULES
            // -------------------------
            modelBuilder.Entity<ModifierRules>(entity => {
                entity.ToTable("modifierRules");
                entity.HasOne(r => r.Universe)
                      .WithMany()
                      .HasForeignKey(r => r.UniverseId)
                      .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(r => r.Characteristic)
                      .WithMany()
                      .HasForeignKey(r => r.CharacteristicId)
                      .OnDelete(DeleteBehavior.SetNull);
            });


            // -------------------------
            // WEAPONS
            // -------------------------
            modelBuilder.Entity<Weapons>(entity =>
            {
                entity.ToTable("weapons");
                entity.HasKey(w => w.Id);

                entity.Property(w => w.Name).IsRequired().HasMaxLength(200);
                entity.Property(w => w.WeaponFireMode).IsRequired();

                // Relation obligatoire : Weapon → Universe
                entity.HasOne(w => w.Universe)
                      .WithMany()
                      .HasForeignKey(w => w.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade); // si l'univers est supprimé, les armes aussi
            });

            // -------------------------
            // EQUIPMENT
            // -------------------------
            modelBuilder.Entity<Equipment>(entity =>
            {
                entity.ToTable("equipments");
                entity.HasKey(e => e.Id);
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);

                entity.HasOne(e => e.Universe)
                      .WithMany()
                      .HasForeignKey(e => e.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // PROTECTION
            // -------------------------
            modelBuilder.Entity<Protection>(entity =>
            {
                entity.ToTable("protections");
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(200);
                entity.Property(p => p.Type).HasMaxLength(100);
                entity.Property(p => p.ArmorRating).HasMaxLength(100);
                entity.Property(p => p.Material).HasMaxLength(200);
                entity.Property(p => p.Weight).HasMaxLength(50);

                entity.HasOne(p => p.Universe)
                      .WithMany()
                      .HasForeignKey(p => p.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // CHARACTERISTIC
            // -------------------------
            modelBuilder.Entity<Characteristic>(entity =>
            {
                entity.ToTable("characteristics");
                entity.HasKey(c => c.Id);
                entity.Property(c => c.Name).IsRequired().HasMaxLength(100);
                entity.Property(c => c.DiceType).IsRequired().HasMaxLength(10);
                entity.Property(c => c.MinDice);
                entity.Property(c => c.MaxDice);
                entity.Property(c => c.HasModifiers).IsRequired();

                entity.HasOne(c => c.Universe)
                      .WithMany()
                      .HasForeignKey(c => c.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // SKILL
            // -------------------------
            modelBuilder.Entity<Skill>(entity =>
            {
                entity.ToTable("skills");
                entity.HasKey(s => s.Id);
                entity.Property(s => s.Name).IsRequired().HasMaxLength(200);
                entity.Property(s => s.RelatedCharacteristic).HasMaxLength(100);
                entity.Property(s => s.Bonus).HasMaxLength(100);
                entity.Property(s => s.Malus).HasMaxLength(200);
                entity.Property(s => s.Prerequisites).HasMaxLength(300);

                entity.HasOne(s => s.Universe)
                      .WithMany()
                      .HasForeignKey(s => s.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // TRAIT
            // -------------------------
            modelBuilder.Entity<Trait>(entity =>
            {
                entity.ToTable("traits");
                entity.HasKey(t => t.Id);
                entity.Property(t => t.Name).IsRequired().HasMaxLength(200);
                entity.Property(t => t.Type).HasMaxLength(100);
                entity.Property(t => t.Effect).IsRequired().HasMaxLength(500);
                entity.Property(t => t.Prerequisites).HasMaxLength(300);

                entity.HasOne(t => t.Universe)
                      .WithMany()
                      .HasForeignKey(t => t.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // -------------------------
            // FRAGMENT IDENTITY
            // Un fragment = une valeur textuelle (ex: "Jean", "Dupont", "Shadow")
            // réutilisable par plusieurs identités, dans plusieurs rôles (prénom, nom, alias)
            // Le rôle n'est PAS stocké dans le fragment — c'est la FK dans Identity qui dit le rôle
            // -------------------------
            modelBuilder.Entity<FragmentIdentity>(entity =>
            {
                entity.ToTable("fragmentIdentities");
                entity.HasKey(f => f.Id);
                entity.Property(f => f.Value).IsRequired().HasMaxLength(200);
                entity.Property(f => f.Gender).IsRequired();

                entity.HasOne(f => f.Universe)
                      .WithMany()
                      .HasForeignKey(f => f.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict); // Pas de cascade : le fragment peut être partagé
            });

            // -------------------------
            // ADDITIONAL INFORMATION (base — Table Per Type)
            // Culture, Specie, Alignment, Origin héritent tous d'AdditionalInformation
            // Chaque type a sa propre table, mais la logique commune est définie ici
            // -------------------------
            modelBuilder.Entity<AdditionalInformation>(entity =>
            {
                // Pas de ToTable ici — c'est TPT (Table Per Type), chaque enfant a sa table
                entity.HasKey(a => a.Id);
                entity.ToTable("additionalInformations");
                entity.Property(a => a.Value).IsRequired().HasMaxLength(200);

                entity.HasOne(a => a.Universe)
                      .WithMany()
                      .HasForeignKey(a => a.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Cascade);
            });

            // Chaque type dérivé a sa propre table — EF Core gère l'héritage TPT
            modelBuilder.Entity<Culture>().ToTable("cultures");
            modelBuilder.Entity<Specie>().ToTable("species");
            modelBuilder.Entity<Alignment>().ToTable("alignments");
            modelBuilder.Entity<Origin>().ToTable("origins");

            // -------------------------
            // IDENTITY
            // Le lien avec les FragmentIdentities se fait via les FK NameId, FirstNameId, AliasId
            // "Jean" peut être le prénom d'une identité et le nom d'une autre — c'est la FK qui dit le rôle
            // -------------------------
            modelBuilder.Entity<Identity>(entity =>
            {
                entity.ToTable("identities");
                entity.HasKey(i => i.Id);

                entity.HasOne(i => i.Universe)
                      .WithMany()
                      .HasForeignKey(i => i.UniverseId)
                      .IsRequired()
                      .OnDelete(DeleteBehavior.Restrict);

                // Fragments — on utilise les vraies propriétés FK du modèle (NameId, FirstNameId, AliasId)
                // Pas de DeleteBehavior.Cascade : si on supprime un fragment, on ne veut pas perdre l'identité
                entity.HasOne(i => i.Name)
                      .WithMany()
                      .HasForeignKey(i => i.NameId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.FirstName)
                      .WithMany()
                      .HasForeignKey(i => i.FirstNameId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Alias)
                      .WithMany()
                      .HasForeignKey(i => i.AliasId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                // Infos additionnelles — relations optionnelles vers les tables dédiées
                entity.HasOne(i => i.Culture)
                      .WithMany()
                      .HasForeignKey(i => i.CultureId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Specie)
                      .WithMany()
                      .HasForeignKey(i => i.SpecieId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Alignment)
                      .WithMany()
                      .HasForeignKey(i => i.AlignmentId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(i => i.Origin)
                      .WithMany()
                      .HasForeignKey(i => i.OriginId)
                      .IsRequired(false)
                      .OnDelete(DeleteBehavior.Restrict);
            });
            
            // -------------------------
            // NPC
            // Un NPC = une identité + des traits + des skills + caracteristics + équipements + armes + armures
            // -------------------------
            modelBuilder.Entity<NPC>(entity =>
            {
                entity.ToTable("npcs");
                entity.HasOne(n => n.Universe)
                      .WithMany()
                      .HasForeignKey(n => n.UniverseId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}