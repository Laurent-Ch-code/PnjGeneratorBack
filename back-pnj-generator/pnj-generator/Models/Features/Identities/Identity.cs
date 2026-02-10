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
        public Guid? CultureId { get; set; }
        [ForeignKey("CultureId")]
        public virtual Culture? Culture { get; set; }

        // Relation Specie (Optionnelle)
        public Guid? SpecieId { get; set; }
        [ForeignKey("SpecieId")]
        public virtual Specie? Specie { get; set; }

        // Relation Alignment (Optionnelle)
        public Guid? AlignmentId { get; set; }
        [ForeignKey("AlignmentId")]
        public virtual Alignment? Alignment { get; set; }

        // Relation Origin (Optionnelle)
        public Guid? OriginId { get; set; }
        [ForeignKey("OriginId")]
        public virtual Origin? Origin { get; set; }

        // Relation Name
        public Guid? NameId { get; set; }
        public FragmentIdentity? Name { get; set; }
        // Relation FirstName
        public Guid? FirstNameId { get; set; }
        public FragmentIdentity? FirstName { get; set; }
        // Relation Alias
        public Guid? AliasId { get; set; }
        public FragmentIdentity? Alias { get; set; }
        public Gender Gender { get; set; }
    }

    public class FragmentIdentity
    {
        [Key]
        public Guid Id { get; set; }
        public Guid UniverseId { get; set; }
        [ForeignKey("UniverseId")]
        public Universe Universe { get; set; } = null!;
        public string Value { get; set; }
    }

    public class AdditionalInformation
    {
        [Key]
        public Guid Id { get; set; }
        public string Name { get; set; }
        public Guid UniverseId { get; set; } 
        [ForeignKey("UnvierseId")]
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
