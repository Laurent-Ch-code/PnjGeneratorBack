using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs;
using pnj_generator.Models;

namespace pnj_generator.Controllers
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/weapons")]
    public class WeaponsController: ControllerBase
    {
        private readonly AppDbContext _db;

        public WeaponsController(AppDbContext db)
        {
            _db = db;
        }

        
        [HttpGet]
        public async Task<ActionResult<List<Weapons>>> GetAllWeapons(Guid universeId)
        {
            var weapons = await _db.Weapons
                .Where(w => w.UniverseId == universeId)
                .ToListAsync();
            return Ok(weapons);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Weapons>> GetWeaponById(Guid universeId,Guid id)
        {
            var weapon = await _db.Weapons.FindAsync(id);
            if (weapon is null) return NotFound();
            return Ok(weapon);
        }

        [HttpPost]
        public async Task<ActionResult<Weapons>> CreateWeapon(Guid universeId,WeaponsCreateDTO dto)
        {
            var weapon = new Weapons
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Damage = dto.Damage,
                Range = dto.Range,
                Description = dto.Description,
                Capacity = dto.Capacity,
                radius = dto.radius,
                weaponFireMode = dto.weaponFireMode,
                UniverseId = dto.UniverseId
            };

            _db.Weapons.Add(weapon);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetWeaponById), new { universeId = universeId, id = weapon.Id }, weapon);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateWeapon(Guid universeId,Guid id, WeaponsCreateDTO dto)
        {
            var weapon = await _db.Weapons.FindAsync(id);
            if (weapon is null) return NotFound();

            weapon.Name = dto.Name;
            weapon.Type = dto.Type;
            weapon.Damage = dto.Damage;
            weapon.Range = dto.Range;
            weapon.Description = dto.Description;
            weapon.Capacity = dto.Capacity;
            weapon.radius = dto.radius;
            weapon.weaponFireMode = dto.weaponFireMode;
            weapon.UniverseId = dto.UniverseId;

            await _db.SaveChangesAsync();
            return Ok(weapon);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteWeapon(Guid universeId, Guid id)
        {
            var weapon = await _db.Weapons
                .FirstOrDefaultAsync(w => w.Id == id && w.UniverseId == universeId);

            if (weapon is null)
                return NotFound();

            _db.Weapons.Remove(weapon);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
