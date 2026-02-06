using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models
{
    public class Weapons
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;
        [Required]
        public Guid UniverseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Damage { get; set; } = string.Empty;
        public string Range { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int? Capacity { get; set; } = null;
        public int? Radius { get; set; } = null;
        public WeaponFireMode WeaponFireMode { get; set; } = WeaponFireMode.Single;

    }

    public enum WeaponFireMode { 
        Single = 0,
        Burst = 1,
        Automatic = 2
    }
}
