using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features;
using pnj_generator.Models.Features;

namespace pnj_generator.Controllers.Features
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/equipments")]
    public class EquipmentController: ControllerBase
    {
        private readonly AppDbContext _db;

        public EquipmentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipment>>> GetAllEquipment(Guid universeId)
        {
            var equipments = await _db.Equipments
                .Where(w => w.UniverseId == universeId)
                .ToListAsync();
            // Implementation for retrieving all equipment
            return Ok(equipments);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Equipment>> GetEquipmentById(Guid universeId,Guid id)
        {
            var equipment = await _db.Equipments.FindAsync(id);
            if (equipment is null) return NotFound();
            return Ok(equipment);
        }

        [HttpPost]
        public async Task<ActionResult<Equipment>> CreateEquipment(Guid universeId,EquipmentCreateDTO dto)
        {
            var equipment = new Equipment
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Type = dto.Type,
                Description = dto.Description,
                Bonus = dto.Bonus,
                Malus = dto.Malus,
                UniverseId = dto.UniverseId
            };
            _db.Equipments.Add(equipment);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetEquipmentById), new { universeId = universeId, id = equipment.Id }, equipment);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEquipment(Guid universeId, Guid id, EquipmentCreateDTO dto)
        {
            var equipment = await _db.Equipments.FindAsync(id);
            if (equipment is null) return NotFound();

            equipment.Name = dto.Name;
            equipment.Type = dto.Type;
            equipment.Description = dto.Description;
            equipment.Bonus = dto.Bonus;
            equipment.Malus = dto.Malus;
            equipment.UniverseId = dto.UniverseId;
            await _db.SaveChangesAsync();
            return Ok(equipment);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEquipment(Guid universeId, Guid id)
        {
            var equipment = await _db.Equipments
                .FirstOrDefaultAsync(e => e.Id == id && e.UniverseId == universeId);

            if (equipment is null)
                return NotFound();

            _db.Equipments.Remove(equipment);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
