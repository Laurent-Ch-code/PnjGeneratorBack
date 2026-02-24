using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models.Features
{
    /// <summary>
    /// Représente une protection (armure, bouclier, gilet pare-balles, etc.)
    /// </summary>
    public class Protection
    {
        [Key]
        public Guid Id { get; set; }

        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        [Required]
        public Guid UniverseId { get; set; }

        /// <summary>
        /// Nom de la protection (ex: "Gilet pare-balles léger", "Armure de plaques")
        /// </summary>
        [Required]
        [MaxLength(200)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type de protection (ex: "Gilet", "Armure complète", "Bouclier")
        /// </summary>
        [MaxLength(100)]
        public string Type { get; set; } = string.Empty;

        /// <summary>
        /// Niveau de protection / Valeur d'armure
        /// Format libre selon le système de jeu (ex: "1D6", "AC 15", "10 PA")
        /// </summary>
        [MaxLength(100)]
        public string ArmorRating { get; set; } = string.Empty;

        /// <summary>
        /// Matériaux de fabrication (ex: "Kevlar", "Acier", "Cuir renforcé")
        /// </summary>
        [MaxLength(200)]
        public string Material { get; set; } = string.Empty;

        /// <summary>
        /// Poids de la protection (format libre: "5kg", "10 lbs", "Léger")
        /// </summary>
        [MaxLength(50)]
        public string Weight { get; set; } = string.Empty;

        /// <summary>
        /// Description détaillée de la protection
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
