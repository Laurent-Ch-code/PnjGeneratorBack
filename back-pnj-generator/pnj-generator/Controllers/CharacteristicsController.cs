using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs;
using pnj_generator.Models;

namespace pnj_generator.Controllers
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/characteristics")]
    public class CharacteristicsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CharacteristicsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Récupère toutes les caractéristiques d'un univers
        /// GET /api/universes/{universeId}/characteristics
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Characteristic>>> GetAllCharacteristics(Guid universeId)
        {
            var characteristics = await _db.Characteristics
                .Where(c => c.UniverseId == universeId)
                .ToListAsync();

            return Ok(characteristics);
        }

        /// <summary>
        /// Récupère une caractéristique spécifique
        /// GET /api/universes/{universeId}/characteristics/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Characteristic>> GetCharacteristicById(Guid universeId, Guid id)
        {
            var characteristic = await _db.Characteristics
                .FirstOrDefaultAsync(c => c.Id == id && c.UniverseId == universeId);

            if (characteristic is null)
                return NotFound(new { message = "Caractéristique introuvable" });

            return Ok(characteristic);
        }

        /// <summary>
        /// Crée une nouvelle caractéristique
        /// POST /api/universes/{universeId}/characteristics
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Characteristic>> CreateCharacteristic(Guid universeId, CharacteristicCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound(new { message = $"Univers {universeId} introuvable" });

            var characteristic = new Characteristic
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Value = dto.Value,
                Modifier = dto.Modifier,
                Description = dto.Description,
                UniverseId = universeId
            };

            _db.Characteristics.Add(characteristic);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCharacteristicById),
                new { universeId = universeId, id = characteristic.Id },
                characteristic
            );
        }

        /// <summary>
        /// Met à jour une caractéristique
        /// PUT /api/universes/{universeId}/characteristics/{id}
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCharacteristic(Guid universeId, Guid id, CharacteristicCreateDTO dto)
        {
            var characteristic = await _db.Characteristics
                .FirstOrDefaultAsync(c => c.Id == id && c.UniverseId == universeId);

            if (characteristic is null)
                return NotFound(new { message = "Caractéristique introuvable dans cet univers" });

            characteristic.Name = dto.Name;
            characteristic.Value = dto.Value;
            characteristic.Modifier = dto.Modifier;
            characteristic.Description = dto.Description;

            await _db.SaveChangesAsync();
            return Ok(characteristic);
        }

        /// <summary>
        /// Supprime une caractéristique
        /// DELETE /api/universes/{universeId}/characteristics/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteCharacteristic(Guid universeId, Guid id)
        {
            var characteristic = await _db.Characteristics
                .FirstOrDefaultAsync(c => c.Id == id && c.UniverseId == universeId);

            if (characteristic is null)
                return NotFound(new { message = "Caractéristique introuvable dans cet univers" });

            _db.Characteristics.Remove(characteristic);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
