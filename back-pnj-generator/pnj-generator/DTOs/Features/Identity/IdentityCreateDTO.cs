using pnj_generator.Models.Features.Identities;

namespace pnj_generator.DTOs.Features.Identity
{
    public class IdentityCreateDTO
    {
        public Guid UniverseId { get; set; }
        public FragmentIdentity? Name { get; set; }
        public FragmentIdentity? FirstName { get; set; }
        public FragmentIdentity? Alias { get; set; }
        public Gender Gender { get; set; }
        public AdditionalInformation? Culture { get; set; }
        public AdditionalInformation? Alignment { get; set; }
        public AdditionalInformation? Specie { get; set; }
        public AdditionalInformation? Origin { get; set; }

    }

    public class AdditionnalInformation
    {
        public string Name { get; set; }
        public Guid UniverseId { get; set; }
    }

    public class FragmentIdentity
    {
        public Guid UniverseId { get; set; }
        public string Value { get; set; }
    }
}
