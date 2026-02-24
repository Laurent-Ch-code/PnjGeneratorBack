using pnj_generator.Models.Features;

namespace pnj_generator.DTOs.Features
{
    public class CharacteristicCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? DiceType { get; set; } = null;
        public CharacteristicGenerationType GenerationType { get; set; } = CharacteristicGenerationType.DiceCount;
        public int? MinDice { get; set; } = null;
        public int? MaxDice { get; set; } = null;
        public int? MinValue { get; set; } = null;
        public int? MaxValue { get; set; } = null;
        public bool HasModifiers { get; set; } = false;
        public Guid UniverseId { get; set; }
    }
}