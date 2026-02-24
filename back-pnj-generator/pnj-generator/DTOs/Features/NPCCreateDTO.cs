namespace pnj_generator.DTOs
{
    /// <summary>
    /// DTO pour créer/éditer un NPC manuellement
    /// Utilisé pour l'édition uniquement — la génération utilise le service
    /// </summary>
    public class NPCCreateDTO
    {
        public Guid UniverseId { get; set; }

        // Snapshots JSON (le front envoie déjà du JSON stringifié)
        public string IdentitySnapshot { get; set; } = string.Empty;
        public string CharacteristicsSnapshot { get; set; } = string.Empty;
        public string SkillsSnapshot { get; set; } = "[]";
        public string WeaponsSnapshot { get; set; } = "[]";
        public string ProtectionsSnapshot { get; set; } = "[]";
        public string EquipmentSnapshot { get; set; } = "[]";
        public string TraitsSnapshot { get; set; } = "[]";

        public int? HP { get; set; } = null;
        public string? PhysicalDescription { get; set; } = null;
        public string? GMNotes { get; set; } = null;
    }
}