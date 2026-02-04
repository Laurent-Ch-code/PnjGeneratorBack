using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models
{
    public class Equipment
    {
        [Key]
        public Guid Id { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;
        [Required]
        public Guid UniverseId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Type { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Bonus { get; set; } = string.Empty;
        public string Malus { get; set; } = string.Empty;
    }
}
