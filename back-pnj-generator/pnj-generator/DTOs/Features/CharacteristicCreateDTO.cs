namespace pnj_generator.DTOs.Features
{
    public class CharacteristicCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string DiceType { get; set; } = string.Empty;
        public int MinDice { get; set; } = 1;
        public int? MaxDice { get; set; } = null;
        public bool HasModifiers { get; set; } = false;
        public Guid UniverseId { get; set; }
    }
}