using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models.Features
{
    /// <summary>
    /// Représente une caractéristique (Force, Dextérité, Intelligence, etc.)
    /// </summary>
    public class Characteristic
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        [Required]
        public Guid UniverseId { get; set; }

        /// <summary>
        /// Nom de la caractéristique (ex: "Force", "Dextérité", "Intelligence")
        /// </summary>
        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Valeur de la caractéristique
        /// Format libre selon le système (ex: "3D6", "8", "+2", "15")
        /// </summary>
        [Required]
        [MaxLength(50)]
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// Modificateur calculé (optionnel)
        /// Ex: D&D → Force 8 = Modificateur -1
        /// Ex: ZCorps → 3D6+2 devient 4D6
        /// </summary>
        [MaxLength(50)]
        public string? Modifier { get; set; }

        /// <summary>
        /// Description de la caractéristique
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
