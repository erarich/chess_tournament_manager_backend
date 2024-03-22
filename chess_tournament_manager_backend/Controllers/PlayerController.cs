using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using chess_tournament_manager_backend.Data;
using chess_tournament_manager_backend.Models;

namespace chess_tournament_manager_backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PlayerController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Player
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers()
        {
            return await _context.Player.ToListAsync();
        }

        // GET: api/Player/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Player>> GetPlayer(long id)
        {
            var Player = await _context.Player.FindAsync(id);

            if (Player == null)
            {
                return NotFound();
            }

            return Player;
        }

        // POST: api/Player
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player Player)
        {
            _context.Player.Add(Player);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetPlayer), new { id = Player.Id }, Player);
        }

        // PUT: api/Player/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(long id, Player Player)
        {
            if (id != Player.Id)
            {
                return BadRequest();
            }

            _context.Entry(Player).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PlayerExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Player/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(long id)
        {
            var Player = await _context.Player.FindAsync(id);
            if (Player == null)
            {
                return NotFound();
            }

            _context.Player.Remove(Player);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool PlayerExists(long id)
        {
            return _context.Player.Any(e => e.Id == id);
        }

    }
}
