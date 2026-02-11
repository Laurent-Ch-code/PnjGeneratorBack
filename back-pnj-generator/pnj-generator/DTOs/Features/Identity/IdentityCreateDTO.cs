using pnj_generator.Models.Features.Identities;

namespace pnj_generator.DTOs.Features.Identity
{
    public class IdentityCreateDTO
    {
        public Guid UniverseId { get; set; }
        public FragmentIdentityDTO? Name { get; set; } = null;
        public FragmentIdentityDTO? FirstName { get; set; } = null;
        public FragmentIdentityDTO? Alias { get; set; } = null;
        public Gender Gender { get; set; }
        public AdditionnalInformationDTO? Culture { get; set; } = null;
        public AdditionnalInformationDTO? Alignment { get; set; } = null;
        public AdditionnalInformationDTO? Specie { get; set; } = null;
        public AdditionnalInformationDTO? Origin { get; set; } = null;

    }

    public class AdditionnalInformationDTO
    {
        public string Value { get; set; }
        public Guid UniverseId { get; set; }
        public Gender Gender { get; set; }
    }

    public class FragmentIdentityDTO
    {
        public Guid UniverseId { get; set; }
        public string Value { get; set; }
    }
}
