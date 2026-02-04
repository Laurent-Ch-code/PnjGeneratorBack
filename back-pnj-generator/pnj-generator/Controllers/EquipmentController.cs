using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.Models;
using pnj_generator.DTOs;

namespace pnj_generator.Controllers
{
    [ApiController]
    [Route("api/equipments")]
    public class EquipmentController: ControllerBase
    {
        private readonly AppDbContext _db;

        public EquipmentController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Equipment>>> GetAllEquipment()
        {
            var equipments = await _db.Equipments.ToListAsync();
            // Implementation for retrieving all equipment
            return Ok(equipments);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Equipment>> GetEquipmentById(Guid id)
        {
            var equipment = await _db.Equipments.FindAsync(id);
            if (equipment is null) return NotFound();
            return Ok(equipment);
        }

        [HttpPost]
        public async Task<ActionResult<Equipment>> CreateEquipment(EquipmentCreateDTO dto)
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
            return CreatedAtAction(nameof(GetEquipmentById), new { id = equipment.Id }, equipment);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateEquipment(Guid id, EquipmentCreateDTO dto)
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
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEquipment(Guid id)
        {
            var equipment = await _db.Equipments.FindAsync(id);
            if (equipment is null) return NotFound();
            _db.Equipments.Remove(equipment);
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
