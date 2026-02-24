using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models
{
    /// <summary>
    /// Représente un PNJ généré
    /// Tous les éléments sont sauvegardés en JSON snapshot pour éviter les dépendances FK
    /// </summary>
    public class NPC
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        /// <summary>
        /// Identité générée (snapshot JSON unique)
        /// Structure : { firstName, lastName, alias, age, gender, culture, specie, alignment, origin }
        /// </summary>
        [Required]
        public string IdentitySnapshot { get; set; } = string.Empty;

        /// <summary>
        /// Caractéristiques générées (snapshot JSON array)
        /// Structure : [{ name, diceType, value, modifier }, ...]
        /// Contient TOUTES les caractéristiques de l'univers
        /// </summary>
        [Required]
        public string CharacteristicsSnapshot { get; set; } = string.Empty;

        /// <summary>
        /// Compétences sélectionnées (snapshot JSON array)
        /// Structure : [{ id, name, bonus }, ...]
        /// </summary>
        public string SkillsSnapshot { get; set; } = "[]";

        /// <summary>
        /// Armes sélectionnées (snapshot JSON array)
        /// Structure : [{ id, name, type, damage, range, capacity, fireMode }, ...]
        /// </summary>
        public string WeaponsSnapshot { get; set; } = "[]";

        /// <summary>
        /// Protections sélectionnées (snapshot JSON array)
        /// Structure : [{ id, name, type, level, materials, weight }, ...]
        /// </summary>
        public string ProtectionsSnapshot { get; set; } = "[]";

        /// <summary>
        /// Équipements sélectionnés (snapshot JSON array)
        /// Structure : [{ id, name, type, bonusMalus }, ...]
        /// </summary>
        public string EquipmentSnapshot { get; set; } = "[]";

        /// <summary>
        /// Traits sélectionnés (snapshot JSON array)
        /// Structure : [{ id, name, effect }, ...]
        /// </summary>
        public string TraitsSnapshot { get; set; } = "[]";

        /// <summary>
        /// Points de vie (optionnel, rempli manuellement par le MJ)
        /// </summary>
        public int? HP { get; set; } = null;

        /// <summary>
        /// Description physique (optionnel, rempli par le MJ)
        /// En V2 : pourra être générée automatiquement avec IA
        /// </summary>
        public string? PhysicalDescription { get; set; } = null;

        /// <summary>
        /// Notes du MJ (optionnel)
        /// </summary>
        public string? GMNotes { get; set; } = null;

        /// <summary>
        /// Date de création du PNJ
        /// </summary>
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}