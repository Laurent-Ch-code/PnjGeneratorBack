using pnj_generator.DTOs.Features.Identity;
using pnj_generator.Models.Features.Identities;

namespace pnj_generator.Interfaces.Services
{
    public interface IIdentityService
    {
        Task<Identity> CreateFullIdentityAsync(Guid universeId, IdentityCreateDTO identityDto);
        Task<Identity?> UpdateIdentityAsync(Guid identityId, IdentityCreateDTO identityDto);
    }
}
