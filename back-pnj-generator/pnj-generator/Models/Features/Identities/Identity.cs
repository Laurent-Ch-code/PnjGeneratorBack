using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace pnj_generator.Models.Features.Identities
{
    public class Identity
    {
        [Key]
        public Guid Id { get; set; }

        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;

        // Relation Culture (Optionnelle)
        public Guid? CultureId { get; set; } = null;
        [ForeignKey("CultureId")]
        public virtual Culture? Culture { get; set; } = null;

        // Relation Specie (Optionnelle)
        public Guid? SpecieId { get; set; } = null;
        [ForeignKey("SpecieId")]
        public virtual Specie? Specie { get; set; } = null;

        // Relation Alignment (Optionnelle)
        public Guid? AlignmentId { get; set; } = null;
        [ForeignKey("AlignmentId")]
        public virtual Alignment? Alignment { get; set; } = null;

        // Relation Origin (Optionnelle)
        public Guid? OriginId { get; set; } = null;
        [ForeignKey("OriginId")]
        public virtual Origin? Origin { get; set; } = null;

        // Relation Name
        public Guid? NameId { get; set; } = null;
        public FragmentIdentity? Name { get; set; } = null;
        // Relation FirstName
        public Guid? FirstNameId { get; set; } = null;
        public FragmentIdentity? FirstName { get; set; } = null;
        // Relation Alias
        public Guid? AliasId { get; set; } = null;
        public FragmentIdentity? Alias { get; set; } = null;
        public Gender Gender { get; set; }
        public int? Age { get; set; } = null;
        public string? Description { get; set; } = null;
    }

    public class FragmentIdentity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;
        public string Value { get; set; }
        public Gender Gender { get; set; }
    }

    public class AdditionalInformation
    {
        [Key]
        public Guid Id { get; set; }
        public string Value { get; set; }
        public Guid UniverseId { get; set; } 
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;
    }

    public class Culture : AdditionalInformation { }

    public class Specie : AdditionalInformation { }

    public class  Alignment : AdditionalInformation { }
    public class Origin : AdditionalInformation { }

    public enum Gender
    {
        Male = 0,
        Female = 1,
        Neutral = 2
    }

    public enum AgeCategory
    {
        Child = 0,
        Teenage = 1,
        YoungAdult = 2,
        Adult = 3,
        MiddleAge = 4,
        Elder = 5
    }
}
