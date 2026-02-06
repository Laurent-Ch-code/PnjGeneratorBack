using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs.Features;
using pnj_generator.Models.Features;

namespace pnj_generator.Controllers.Features
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/skills")]
    public class SkillsController : ControllerBase
    {
        private readonly AppDbContext _db;

        public SkillsController(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Récupère toutes les compétences d'un univers
        /// GET /api/universes/{universeId}/skills
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<Skill>>> GetAllSkills(Guid universeId)
        {
            var skills = await _db.Skills
                .Where(s => s.UniverseId == universeId)
                .ToListAsync();

            return Ok(skills);
        }

        /// <summary>
        /// Récupère une compétence spécifique
        /// GET /api/universes/{universeId}/skills/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Skill>> GetSkillById(Guid universeId, Guid id)
        {
            var skill = await _db.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UniverseId == universeId);

            if (skill is null)
                return NotFound(new { message = "Compétence introuvable" });

            return Ok(skill);
        }

        /// <summary>
        /// Crée une nouvelle compétence
        /// POST /api/universes/{universeId}/skills
        /// </summary>
        [HttpPost]
        public async Task<ActionResult<Skill>> CreateSkill(Guid universeId, SkillCreateDTO dto)
        {
            var universeExists = await _db.Universes.AnyAsync(u => u.Id == universeId);
            if (!universeExists)
                return NotFound(new { message = $"Univers {universeId} introuvable" });

            var skill = new Skill
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                RelatedCharacteristic = dto.RelatedCharacteristic,
                Bonus = dto.Bonus,
                Malus = dto.Malus,
                Prerequisites = dto.Prerequisites,
                UniverseId = universeId
            };

            _db.Skills.Add(skill);
            await _db.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetSkillById),
                new { universeId = universeId, id = skill.Id },
                skill
            );
        }

        /// <summary>
        /// Met à jour une compétence
        /// PUT /api/universes/{universeId}/skills/{id}
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<IActionResult> UpdateSkill(Guid universeId, Guid id, SkillCreateDTO dto)
        {
            var skill = await _db.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UniverseId == universeId);

            if (skill is null)
                return NotFound(new { message = "Compétence introuvable dans cet univers" });

            skill.Name = dto.Name;
            skill.Description = dto.Description;
            skill.RelatedCharacteristic = dto.RelatedCharacteristic;
            skill.Bonus = dto.Bonus;
            skill.Malus = dto.Malus;
            skill.Prerequisites = dto.Prerequisites;

            await _db.SaveChangesAsync();
            return Ok(skill);
        }

        /// <summary>
        /// Supprime une compétence
        /// DELETE /api/universes/{universeId}/skills/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteSkill(Guid universeId, Guid id)
        {
            var skill = await _db.Skills
                .FirstOrDefaultAsync(s => s.Id == id && s.UniverseId == universeId);

            if (skill is null)
                return NotFound(new { message = "Compétence introuvable dans cet univers" });

            _db.Skills.Remove(skill);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}
