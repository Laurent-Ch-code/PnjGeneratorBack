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
        /// Type de génération de la caractéristique
        /// DiceCount (0) : nombre de dés variable (ex: ZCorps 1D6 à 8D6)
        /// FixedValue (1) : valeur fixe générée (ex: DnD 3 à 18)
        /// </summary>
        [Required]
        public CharacteristicGenerationType GenerationType { get; set; } = CharacteristicGenerationType.DiceCount;

        /// <summary>
        /// Type de dé utilisé pour cette caractéristique (ex: "D6", "D20", "D100")
        /// </summary>
        [MaxLength(10)]
        public string? DiceType { get; set; }

        /// <summary>
        /// Nombre minimum de dés à lancer (ex: 2 pour 2D6)
        /// </summary>
        public int? MinDice { get; set; }

        /// <summary>
        /// Nombre maximum de dés à lancer
        /// Si null → jet fixe, égal à MinDice
        /// </summary>
        public int? MaxDice { get; set; }

        /// <summary>
        /// Valeur minimum générée
        /// Utilisé uniquement si GenerationType = FixedValue
        /// </summary>
        public int? MinValue { get; set; }

        /// <summary>
        /// Valeur maximum générée
        /// Utilisé uniquement si GenerationType = FixedValue
        /// </summary>
        public int? MaxValue { get; set; }

        /// <summary>
        /// Indique si cette caractéristique a ses propres règles de modificateurs
        /// Si false → les règles globales de l'univers s'appliquent (si elles existent)
        /// </summary>
        public bool HasModifiers { get; set; } = false;
    }

    /// <summary>
    /// Type de génération d'une caractéristique
    /// </summary>
    public enum CharacteristicGenerationType
    {
        /// <summary>
        /// Nombre de dés variable (ex: ZCorps 1D6 à 8D6)
        /// </summary>
        DiceCount = 0,

        /// <summary>
        /// Valeur fixe générée (ex: DnD 3 à 18)
        /// </summary>
        FixedValue = 1
    }
}