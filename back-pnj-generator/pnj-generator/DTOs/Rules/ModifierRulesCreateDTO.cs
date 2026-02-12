using pnj_generator.Models.Rules;

namespace pnj_generator.DTOs.Rules
{
    public class ModifierRulesCreateDTO
    {
        public Guid UniverseId { get; set; }

        // Null = règle globale, renseigné = règle spécifique à une caractéristique
        public Guid? CharacteristicId { get; set; } = null;

        public ModifierType Type { get; set; }

        // RangeTable
        public int? RangeMin { get; set; } = null;
        public int? RangeMax { get; set; } = null;
        public int? Modifier { get; set; } = null;

        // AvailableList
        public int? AvailableValue { get; set; } = null;
    }
}