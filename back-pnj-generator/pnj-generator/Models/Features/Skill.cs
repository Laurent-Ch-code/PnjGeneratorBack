using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models.Features
{
    /// <summary>
    /// Représente une compétence (Tir, Discrétion, Artisanat, etc.)
    /// </summary>
    public class Skill
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        [Required]
        public Guid UniverseId { get; set; }

        /// <summary>
        /// Nom de la compétence (ex: "Tir", "Discrétion", "Artisanat")
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Description de la compétence
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Caractéristique liée (optionnel)
        /// Ex: "Dextérité" pour Tir, "Force" pour Escalade
        /// </summary>
        [MaxLength(100)]
        public string? RelatedCharacteristic { get; set; }

        /// <summary>
        /// Bonus apporté par la compétence (format libre)
        /// Ex: "+2", "1D6", "Avantage"
        /// </summary>
        [MaxLength(100)]
        public string? Bonus { get; set; }

        /// <summary>
        /// Malus ou contrainte (format libre)
        /// Ex: "-1 en armure lourde", "Fatigue après usage"
        /// </summary>
        [MaxLength(200)]
        public string? Malus { get; set; }

        /// <summary>
        /// Pré-requis pour obtenir la compétence
        /// Ex: "Force 12+", "Niveau 3", "Trait Agile"
        /// </summary>
        [MaxLength(300)]
        public string? Prerequisites { get; set; }
    }
}
