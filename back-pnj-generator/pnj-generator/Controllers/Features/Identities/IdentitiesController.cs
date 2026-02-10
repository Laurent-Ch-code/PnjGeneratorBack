using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using pnj_generator.Data;
using pnj_generator.Models.Features.Identities;

namespace pnj_generator.Controllers.Features.Identities
{
    [ApiController]
    [Route("api/universes/{universeId:guid}/identities")]
    public class IdentitiesController: ControllerBase
    {
        private readonly AppDbContext _db;

        public IdentitiesController(AppDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<ActionResult<List<Identity>>> GetIdentities(Guid universeId) => await _db.Identities.Where(x => x.UniverseId == universeId).ToListAsync();
    }
}
