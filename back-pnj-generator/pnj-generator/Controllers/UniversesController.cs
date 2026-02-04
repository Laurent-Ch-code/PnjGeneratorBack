using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.DTOs;
using pnj_generator.Models;

namespace PnjGenerator.Controllers;

[ApiController]
[Route("api/universes")]
public class UniversesController : ControllerBase
{
    private readonly AppDbContext _db;

    public UniversesController(AppDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<ActionResult<List<Universe>>> GetAll()
    {
        var universes = await _db.Universes
            .OrderBy(u => u.Name)
            .ToListAsync();

        return Ok(universes);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Universe>> GetById(Guid id)
    {
        var universe = await _db.Universes.FindAsync(id);
        if (universe is null) return NotFound();

        return Ok(universe);
    }

    [HttpPost]
    public async Task<ActionResult<Universe>> Create(UniverseCreateDTO dto)
    {
        var universe = new Universe
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Era = dto.Era,
            Description = dto.Description,
            DiceRule = dto.DiceRule
        };

        _db.Universes.Add(universe);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = universe.Id }, universe);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, UniverseCreateDTO dto)
    {
        var universe = await _db.Universes.FindAsync(id);
        if (universe is null) return NotFound();

        universe.Name = dto.Name;
        universe.Era = dto.Era;
        universe.Description = dto.Description;
        universe.DiceRule = dto.DiceRule;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var universe = await _db.Universes.FindAsync(id);
        if (universe is null) return NotFound();
        _db.Universes.Remove(universe);
        await _db.SaveChangesAsync();
        return NoContent();
    }
}
