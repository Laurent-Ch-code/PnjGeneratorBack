namespace pnj_generator.DTOs.Features
{
    public class EquipmentCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Bonus { get; set; } = string.Empty;
        public string Malus { get; set; } = string.Empty;
        // ✅ Lien obligatoire avec l’univers
        public Guid UniverseId { get; set; }
    }
}
