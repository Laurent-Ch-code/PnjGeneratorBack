using pnj_generator.Models.Features;

namespace pnj_generator.DTOs.Features
{
    public class WeaponsCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        public int Radius { get; set; } = 0;
        public WeaponFireMode WeaponFireMode { get; set; } = WeaponFireMode.Single;
        // ✅ Lien obligatoire avec l’univers
        public Guid UniverseId { get; set; }
    }
}
