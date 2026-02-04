using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs;
using pnj_generator.Models;

namespace pnj_generator.Controllers
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/traits")]
    public class TraitsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public TraitsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Récupère tous les traits d'un univers
        /// GET /api/universes/{universeId}/traits
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Trait>>> GetAllTraits(Guid universeId)
        {
            var traits = await _db.Traits
                .Where(t => t.UniverseId == universeId)
                .ToListAsync();

            return Ok(traits);
        }

        /// <summary>
        /// Récupère un trait spécifique
        /// GET /api/universes/{universeId}/traits/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Trait>> GetTraitById(Guid universeId, Guid id)
        {
            var trait = await _db.Traits
                .FirstOrDefaultAsync(t => t.Id == id && t.UniverseId == universeId);

            if (trait is null)
                return NotFound(new { message = "Trait introuvable" });

            return Ok(trait);
        }

        /// <summary>
        /// Crée un nouveau trait
        /// POST /api/universes/{universeId}/traits
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Trait>> CreateTrait(Guid universeId, TraitCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound(new { message = $"Univers {universeId} introuvable" });

            var trait = new Trait
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Effect = dto.Effect,
                Description = dto.Description,
                Prerequisites = dto.Prerequisites,
                UniverseId = universeId
            };

            _db.Traits.Add(trait);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetTraitById),
                new { universeId = universeId, id = trait.Id },
                trait
            );
        }

        /// <summary>
        /// Met à jour un trait
        /// PUT /api/universes/{universeId}/traits/{id}
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateTrait(Guid universeId, Guid id, TraitCreateDTO dto)
        {
            var trait = await _db.Traits
                .FirstOrDefaultAsync(t => t.Id == id && t.UniverseId == universeId);

            if (trait is null)
                return NotFound(new { message = "Trait introuvable dans cet univers" });

            trait.Name = dto.Name;
            trait.Type = dto.Type;
            trait.Effect = dto.Effect;
            trait.Description = dto.Description;
            trait.Prerequisites = dto.Prerequisites;

            await _db.SaveChangesAsync();
            return Ok(trait);
        }

        /// <summary>
        /// Supprime un trait
        /// DELETE /api/universes/{universeId}/traits/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteTrait(Guid universeId, Guid id)
        {
            var trait = await _db.Traits
                .FirstOrDefaultAsync(t => t.Id == id && t.UniverseId == universeId);

            if (trait is null)
                return NotFound(new { message = "Trait introuvable dans cet univers" });

            _db.Traits.Remove(trait);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
