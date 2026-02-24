using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs;
using pnj_generator.Interfaces.Services;
using pnj_generator.Models;
using pnj_generator.Services;

namespace pnj_generator.Controllers
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/npcs")]
    public class NPCsController : ControllerBase
    {
        private readonly AppDbContext _db;
        private readonly INPCGeneratorService _generatorService;

        public NPCsController(AppDbContext db, INPCGeneratorService generatorService)
        {
            _db = db;
            _generatorService = generatorService;
        }

        /// <summary>
        /// Récupère tous les NPCs d'un univers
        /// GET /api/universes/{universeId}/npcs
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<List<NPC>>> GetAll(Guid universeId)
        {
            var npcs = await _db.NPCs
                .Where(n => n.UniverseId == universeId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();

            return Ok(npcs);
        }

        /// <summary>
        /// Récupère un NPC spécifique
        /// GET /api/universes/{universeId}/npcs/{id}
        /// </summary>
        [HttpGet("{id:guid}")]
        public async Task<ActionResult<NPC>> GetById(Guid universeId, Guid id)
        {
            var npc = await _db.NPCs
                .FirstOrDefaultAsync(n => n.Id == id && n.UniverseId == universeId);

            if (npc == null)
                return NotFound(new { message = "NPC introuvable" });

            return Ok(npc);
        }

        /// <summary>
        /// Génère un nouveau NPC aléatoirement
        /// POST /api/universes/{universeId}/npcs/generate
        /// </summary>
        [HttpPost("generate")]
        public async Task<ActionResult<NPC>> Generate(Guid universeId)
        {
            try
            {
                var npc = await _generatorService.GenerateNPCAsync(universeId);

                _db.NPCs.Add(npc);
                await _db.SaveChangesAsync();

                return CreatedAtAction(nameof(GetById), new { universeId, id = npc.Id }, npc);
            }
            catch (ArgumentException ex)
            {
                return NotFound(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Erreur lors de la génération", error = ex.Message });
            }
        }

        /// <summary>
        /// Met à jour un NPC existant
        /// PUT /api/universes/{universeId}/npcs/{id}
        /// </summary>
        [HttpPut("{id:guid}")]
        public async Task<ActionResult<NPC>> Update(Guid universeId, Guid id, NPCCreateDTO dto)
        {
            var npc = await _db.NPCs
                .FirstOrDefaultAsync(n => n.Id == id && n.UniverseId == universeId);

            if (npc == null)
                return NotFound(new { message = "NPC introuvable" });

            // Mise à jour des snapshots
            npc.IdentitySnapshot = dto.IdentitySnapshot;
            npc.CharacteristicsSnapshot = dto.CharacteristicsSnapshot;
            npc.SkillsSnapshot = dto.SkillsSnapshot;
            npc.WeaponsSnapshot = dto.WeaponsSnapshot;
            npc.ProtectionsSnapshot = dto.ProtectionsSnapshot;
            npc.EquipmentSnapshot = dto.EquipmentSnapshot;
            npc.TraitsSnapshot = dto.TraitsSnapshot;
            npc.HP = dto.HP;
            npc.PhysicalDescription = dto.PhysicalDescription;
            npc.GMNotes = dto.GMNotes;

            await _db.SaveChangesAsync();

            return Ok(npc);
        }

        /// <summary>
        /// Supprime un NPC
        /// DELETE /api/universes/{universeId}/npcs/{id}
        /// </summary>
        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid universeId, Guid id)
        {
            var npc = await _db.NPCs
                .FirstOrDefaultAsync(n => n.Id == id && n.UniverseId == universeId);

            if (npc == null)
                return NotFound(new { message = "NPC introuvable" });

            _db.NPCs.Remove(npc);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}