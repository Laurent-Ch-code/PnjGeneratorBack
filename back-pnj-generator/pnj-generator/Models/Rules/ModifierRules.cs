using pnj_generator.Models.Features;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models.Rules
{
    public class ModifierRules
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        // Null = règle globale à l'univers
        // Renseigné = règle spécifique à une caractéristique (prioritaire sur la règle globale)
        public Guid? CharacteristicId { get; set; } = null;
        [ForeignKey("CharacteristicId")]
        public Characteristic? Characteristic { get; set; } = null;

        [Required]
        public ModifierType Type { get; set; }

        // Utilisé si Type = RangeTable
        // Ex D&D : caract entre RangeMin et RangeMax → Modifier
        public int? RangeMin { get; set; } = null;
        public int? RangeMax { get; set; } = null;
        public int? Modifier { get; set; } = null;

        // Utilisé si Type = AvailableList
        // Ex ZCorps : valeur de modificateur disponible au choix
        public int? AvailableValue { get; set; } = null;
    }

    public enum ModifierType
    {
        RangeTable = 0, // D&D : palier min/max → modificateur
        AvailableList = 1  // ZCorps : liste de valeurs disponibles
    }
}