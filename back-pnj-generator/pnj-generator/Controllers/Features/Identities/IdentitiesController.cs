using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features.Identity;
using pnj_generator.Interfaces.Services;
using pnj_generator.Models.Features.Identities;
using pnj_generator.Services;

namespace pnj_generator.Controllers.Features.Identities
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/identities")]
    public class IdentitiesController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly IIdentityService _identityService;

        public IdentitiesController(AppDbContext db, IIdentityService identityService)
        {
            _db = db;
            _identityService = identityService;
        }

        [HttpGet]
        public async Task<ActionResult<List<Identity>>> GetIdentities(Guid universeId)
        {
            var identities = await _db.Identities
                .Where(i => i.UniverseId == universeId)
                .Include(i => i.FirstName)
                .Include(i => i.Name)
                .Include(i => i.Alias)
                .Include(i => i.Culture)
                .Include(i => i.Specie)
                .Include(i => i.Alignment)
                .Include(i => i.Origin)
                .ToListAsync();
            return Ok(identities);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Identity>> GetIdentityById(Guid universeId, Guid id)
        {
            var identity = await _db.Identities
                .Where(i => i.UniverseId == universeId && i.Id == id)
                .Include(i => i.FirstName)
                .Include(i => i.Name)
                .Include(i => i.Alias)
                .Include(i => i.Culture)
                .Include(i => i.Specie)
                .Include(i => i.Alignment)
                .Include(i => i.Origin)
                .FirstOrDefaultAsync();
            return Ok(identity);
        }

        [HttpPost]
        public async Task<ActionResult<Identity>> CreateIdentity(Guid universeId, IdentityCreateDTO identityDTO)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound($"Universe with id {universeId} does not exist");

            var newIdentity = await _identityService.CreateFullIdentityAsync(universeId, identityDTO);

            return CreatedAtAction(nameof(GetIdentityById), new { universeId, id = newIdentity.Id }, newIdentity);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Identity>> UpdateIdentity(Guid id, IdentityCreateDTO identityDto)
        {
            // On appelle le service
            var updatedIdentity = await _identityService.UpdateIdentityAsync(id, identityDto);

            if (updatedIdentity == null)
            {
                return NotFound($"Identity with ID {id} not found.");
            }

            return Ok(updatedIdentity);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult> DeleteIdentity(Guid universeId, Guid id)
        {
            var identity = await _db.Identities
                .FirstOrDefaultAsync(i => i.Id == id && i.UniverseId == universeId);

            if (identity == null)
                return NotFound($"Identity with ID {id} not found.");

            // Supprime uniquement la ligne Identity
            // Les FragmentIdentity et AdditionalInformation restent en BDD
            // pour être réutilisés par d'autres identités ou futures générations
            _db.Identities.Remove(identity);
            await _db.SaveChangesAsync();

            return NoContent();
        }

        // Suppression complète — identity + tous ses fragments/infos s'ils ne sont plus utilisés ailleurs
        [HttpDelete("{id:guid}/full")]
        public async Task<ActionResult> DeleteIdentityFull(Guid universeId, Guid id)
        {
            var identity = await _db.Identities
                .Include(i => i.FirstName)
                .Include(i => i.Name)
                .Include(i => i.Alias)
                .Include(i => i.Culture)
                .Include(i => i.Specie)
                .Include(i => i.Alignment)
                .Include(i => i.Origin)
                .FirstOrDefaultAsync(i => i.Id == id && i.UniverseId == universeId);

            if (identity == null)
                return NotFound($"Identity with ID {id} not found.");

            // Récupère les entités liées avant de supprimer l'identité
            var fragments = new[] { identity.FirstName, identity.Name, identity.Alias }
                .Where(f => f != null).ToList();

            var additionalInfos = new AdditionalInformation?[] { identity.Culture, identity.Specie, identity.Alignment, identity.Origin }
                .Where(a => a != null).ToList();

            // Supprime l'identité d'abord (libère les FK)
            _db.Identities.Remove(identity);
            await _db.SaveChangesAsync();

            // Supprime les fragments seulement s'ils ne sont plus référencés
            foreach (var fragment in fragments)
            {
                var isUsed = await _db.Identities.AnyAsync(i =>
                    i.FirstNameId == fragment!.Id ||
                    i.NameId == fragment!.Id ||
                    i.AliasId == fragment!.Id);

                if (!isUsed) _db.FragmentIdentities.Remove(fragment!);
            }

            // Supprime les infos additionnelles seulement si plus référencées
            foreach (var info in additionalInfos)
            {
                var isUsed = await _db.Identities.AnyAsync(i =>
                    i.CultureId == info!.Id ||
                    i.SpecieId == info!.Id ||
                    i.AlignmentId == info!.Id ||
                    i.OriginId == info!.Id);

                if (!isUsed) _db.Set<AdditionalInformation>().Remove(info!);
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
