using pnj_generator.Models;

namespace pnj_generator.DTOs
{
    public class WeaponsCreateDTO
    {
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int Capacity { get; set; } = 0;
        public int radius { get; set; } = 0;
        public WeaponFireMode weaponFireMode { get; set; } = WeaponFireMode.Single;
        // ✅ Lien obligatoire avec l’univers
        public Guid UniverseId { get; set; }
    }
}
