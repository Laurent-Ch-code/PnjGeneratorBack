using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features;
using pnj_generator.Models.Features;

namespace pnj_generator.Controllers.Features
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

        [HttpGet]
        public async Task<ActionResult<List<Characteristic>>> GetAllCharacteristics(Guid universeId)
        {
            var characteristics = await _db.Characteristics
                .Where(c => c.UniverseId == universeId)
                .ToListAsync();

            return Ok(characteristics);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Characteristic>> GetCharacteristicById(Guid universeId, Guid id)
        {
            var characteristic = await _db.Characteristics
                .FirstOrDefaultAsync(c => c.Id == id && c.UniverseId == universeId);

            if (characteristic is null)
                return NotFound(new { message = "Caractéristique introuvable" });

            return Ok(characteristic);
        }

        [HttpPost]
        public async Task<ActionResult<Characteristic>> CreateCharacteristic(Guid universeId, CharacteristicCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound(new { message = $"Univers {universeId} introuvable" });

            var characteristic = new Characteristic
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                Name = dto.Name,
                Description = dto.Description,
                GenerationType = dto.GenerationType,
                DiceType = dto.DiceType,
                MinDice = dto.MinDice,
                MaxDice = dto.MaxDice,
                MinValue = dto.MinValue,
                MaxValue = dto.MaxValue,
                HasModifiers = dto.HasModifiers
            };

            _db.Characteristics.Add(characteristic);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCharacteristicById),
                new { universeId, id = characteristic.Id },
                characteristic
            );
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateCharacteristic(Guid universeId, Guid id, CharacteristicCreateDTO dto)
        {
            var characteristic = await _db.Characteristics
                .FirstOrDefaultAsync(c => c.Id == id && c.UniverseId == universeId);

            if (characteristic is null)
                return NotFound(new { message = "Caractéristique introuvable dans cet univers" });

            characteristic.Name = dto.Name;
            characteristic.Description = dto.Description;
            characteristic.GenerationType = dto.GenerationType;
            characteristic.DiceType = dto.DiceType;
            characteristic.MinDice = dto.MinDice;
            characteristic.MaxDice = dto.MaxDice;
            characteristic.MinValue = dto.MinValue;
            characteristic.MaxValue = dto.MaxValue;
            characteristic.HasModifiers = dto.HasModifiers;

            await _db.SaveChangesAsync();
            return Ok(characteristic);
        }

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