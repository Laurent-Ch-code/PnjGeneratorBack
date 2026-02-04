using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models
{
    /// <summary>
    /// Représente un trait (Ambidextre, Vision nocturne, Berserker, etc.)
    /// </summary>
    public class Trait
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        [Required]
        public Guid UniverseId { get; set; }

        /// <summary>
        /// Nom du trait (ex: "Ambidextre", "Vision nocturne", "Berserker")
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type de trait (ex: "Avantage", "Désavantage", "Physique", "Mental")
        /// </summary>
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Effet mécanique du trait
        /// Ex: "+2 aux jets de Force", "Voit dans le noir", "Rage +1D6 dégâts"
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Effect { get; set; } = string.Empty;

        /// <summary>
        /// Description narrative du trait
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Pré-requis pour obtenir le trait
        /// Ex: "Force 15+", "Race Elfe", "Niveau 5"
        /// </summary>
        [MaxLength(300)]
        public string? Prerequisites { get; set; }
    }
}
