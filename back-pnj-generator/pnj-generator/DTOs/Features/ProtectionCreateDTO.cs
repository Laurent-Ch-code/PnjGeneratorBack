namespace pnj_generator.DTOs.Features
{
    public class ProtectionCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string ArmorRating { get; set; } = string.Empty;
        public string Material { get; set; } = string.Empty;
        public string Weight { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        
        // ✅ Lien obligatoire avec l'univers
        public Guid UniverseId { get; set; }
    }
}
