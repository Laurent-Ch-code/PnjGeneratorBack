namespace pnj_generator.DTOs
{
    public class CharacteristicCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string? Modifier { get; set; }
        public string Description { get; set; } = string.Empty;
        
        // ✅ Lien obligatoire avec l'univers
        public Guid UniverseId { get; set; }
    }
}
