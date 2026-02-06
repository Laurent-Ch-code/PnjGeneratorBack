namespace pnj_generator.DTOs.Features
{
    public class TraitCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Effect { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? Prerequisites { get; set; }
        
        // ✅ Lien obligatoire avec l'univers
        public Guid UniverseId { get; set; }
    }
}
