using pnj_generator.Models;

namespace pnj_generator.Interfaces.Services
{
    public interface INPCGeneratorService
    {
        Task<NPC> GenerateNPCAsync(Guid universeId);
    }
}