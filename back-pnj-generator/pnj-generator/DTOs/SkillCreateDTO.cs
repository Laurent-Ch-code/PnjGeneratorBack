namespace pnj_generator.DTOs
{
    public class SkillCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? RelatedCharacteristic { get; set; }
        public string? Bonus { get; set; }
        public string? Malus { get; set; }
        public string? Prerequisites { get; set; }
        
        // ✅ Lien obligatoire avec l'univers
        public Guid UniverseId { get; set; }
    }
}
