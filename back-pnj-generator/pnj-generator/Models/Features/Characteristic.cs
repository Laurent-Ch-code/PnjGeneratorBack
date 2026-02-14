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

        [Required]
        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Type de dé utilisé pour cette caractéristique (ex: "D6", "D20", "D100")
        /// </summary>
        [Required]
        [MaxLength(10)]
        public string DiceType { get; set; } = string.Empty;

        /// <summary>
        /// Nombre minimum de dés à lancer (ex: 2 pour 2D6)
        /// </summary>
        [Required]
        public int MinDice { get; set; } = 1;

        /// <summary>
        /// Nombre maximum de dés à lancer
        /// Si null → jet fixe, égal à MinDice
        /// </summary>
        public int? MaxDice { get; set; } = null;

        /// <summary>
        /// Indique si cette caractéristique a ses propres règles de modificateurs
        /// Si false → les règles globales de l'univers s'appliquent (si elles existent)
        /// </summary>
        public bool HasModifiers { get; set; } = false;
    }
}