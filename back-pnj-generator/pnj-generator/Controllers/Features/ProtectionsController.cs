using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features;
using pnj_generator.Models.Features;

namespace pnj_generator.Controllers.Features
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/protections")]
    public class ProtectionsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ProtectionsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Récupère toutes les protections d'un univers
        /// GET /api/universes/{universeId}/protections
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Protection>>> GetAllProtections(Guid universeId)
        {
            var protections = await _db.Protections
                .Where(p => p.UniverseId == universeId)
                .ToListAsync();

            return Ok(protections);
        }

        /// <summary>
        /// Récupère une protection spécifique
        /// GET /api/universes/{universeId}/protections/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Protection>> GetProtectionById(Guid universeId, Guid id)
        {
            var protection = await _db.Protections
                .FirstOrDefaultAsync(p => p.Id == id && p.UniverseId == universeId);

            if (protection is null)
                return NotFound(new { message = "Protection introuvable" });

            return Ok(protection);
        }

        /// <summary>
        /// Crée une nouvelle protection
        /// POST /api/universes/{universeId}/protections
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Protection>> CreateProtection(Guid universeId, ProtectionCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound(new { message = $"Univers {universeId} introuvable" });

            var protection = new Protection
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                ArmorRating = dto.ArmorRating,
                Material = dto.Material,
                Weight = dto.Weight,
                Description = dto.Description,
                UniverseId = universeId
            };

            _db.Protections.Add(protection);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetProtectionById),
                new { universeId = universeId, id = protection.Id },
                protection
            );
        }

        /// <summary>
        /// Met à jour une protection
        /// PUT /api/universes/{universeId}/protections/{id}
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateProtection(Guid universeId, Guid id, ProtectionCreateDTO dto)
        {
            var protection = await _db.Protections
                .FirstOrDefaultAsync(p => p.Id == id && p.UniverseId == universeId);

            if (protection is null)
                return NotFound(new { message = "Protection introuvable dans cet univers" });

            protection.Name = dto.Name;
            protection.Type = dto.Type;
            protection.ArmorRating = dto.ArmorRating;
            protection.Material = dto.Material;
            protection.Weight = dto.Weight;
            protection.Description = dto.Description;

            await _db.SaveChangesAsync();
            return Ok(protection);
        }

        /// <summary>
        /// Supprime une protection
        /// DELETE /api/universes/{universeId}/protections/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteProtection(Guid universeId, Guid id)
        {
            var protection = await _db.Protections
                .FirstOrDefaultAsync(p => p.Id == id && p.UniverseId == universeId);

            if (protection is null)
                return NotFound(new { message = "Protection introuvable dans cet univers" });

            _db.Protections.Remove(protection);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
