using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Rules;
using pnj_generator.Models.Rules;

namespace pnj_generator.Controllers.Rules
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/modifier-rules")]
    public class ModifierRulesController : ControllerBase
    {
        private readonly AppDbContext _db;

        public ModifierRulesController(AppDbContext db)
        {
            _db = db;
        }

        // Règles globales de l'univers (CharacteristicId = null)
        [HttpGet]
        public async Task<ActionResult<List<ModifierRules>>> GetByUniverse(Guid universeId)
        {
            var rules = await _db.ModifierRules
                .Where(r => r.UniverseId == universeId && r.CharacteristicId == null)
                .ToListAsync();

            return Ok(rules);
        }

        // Règles spécifiques à une caractéristique
        [HttpGet("characteristic/{characteristicId:guid}")]
        public async Task<ActionResult<List<ModifierRules>>> GetByCharacteristic(Guid universeId, Guid characteristicId)
        {
            var rules = await _db.ModifierRules
                .Where(r => r.UniverseId == universeId && r.CharacteristicId == characteristicId)
                .ToListAsync();

            return Ok(rules);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ModifierRules>> GetById(Guid universeId, Guid id)
        {
            var rule = await _db.ModifierRules
                .FirstOrDefaultAsync(r => r.Id == id && r.UniverseId == universeId);

            if (rule is null) return NotFound();

            return Ok(rule);
        }

        [HttpPost]
        public async Task<ActionResult<ModifierRules>> CreateModifierRule(Guid universeId, ModifierRulesCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists) return NotFound($"Universe with ID {universeId} not found.");

            var rule = new ModifierRules
            {
                Id = Guid.NewGuid(),
                UniverseId = universeId,
                CharacteristicId = dto.CharacteristicId,
                Type = dto.Type,
                RangeMin = dto.RangeMin,
                RangeMax = dto.RangeMax,
                Modifier = dto.Modifier,
                AvailableValue = dto.AvailableValue
            };

            _db.ModifierRules.Add(rule);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { universeId, id = rule.Id }, rule);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<ModifierRules>> UpdateModifierRule(Guid universeId, Guid id, ModifierRulesCreateDTO dto)
        {
            var rule = await _db.ModifierRules
                .FirstOrDefaultAsync(r => r.Id == id && r.UniverseId == universeId);

            if (rule is null) return NotFound();

            rule.Type = dto.Type;
            rule.RangeMin = dto.RangeMin;
            rule.RangeMax = dto.RangeMax;
            rule.Modifier = dto.Modifier;
            rule.AvailableValue = dto.AvailableValue;

            await _db.SaveChangesAsync();

            return Ok(rule);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteModifierRule(Guid universeId, Guid id)
        {
            var rule = await _db.ModifierRules
                .FirstOrDefaultAsync(r => r.Id == id && r.UniverseId == universeId);

            if (rule is null) return NotFound();

            _db.ModifierRules.Remove(rule);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}