using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using chess_tournament_manager_backend.Data;
using chess_tournament_manager_backend.Models;
using Microsoft.AspNetCore.Identity;

namespace chess_tournament_manager_backend.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public TournamentController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/Tournament
        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetTournaments()
        {
            return await _context.Tournament.ToListAsync();
        }

        // GET: api/Tournament/5
        [AllowAnonymous]
        [HttpGet("{id}")]
        public async Task<ActionResult<Tournament>> GetTournament(long id)
        {
            var Tournament = await _context.Tournament.FindAsync(id);

            if (Tournament == null)
            {
                return NotFound();
            }

            return Tournament;
        }

        // POST: api/Tournament
        [HttpPost]
        public async Task<ActionResult<Tournament>> PostTournament(Tournament Tournament)
        {
            if (ModelState.IsValid)
            {
                var ownerUserId = _userManager.GetUserId(User);
                Tournament.OwnerUserId = ownerUserId;

                Tournament.IsFinished = false;
                Tournament.IsStarted = false;
                Tournament.CreatedDate = DateTime.Now;

                _context.Add(Tournament);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetTournament), new { id = Tournament.Id }, Tournament);
            }
            return BadRequest(ModelState);
        }

        // PUT: api/Tournament/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(long id, Tournament Tournament)
        {
            if (id != Tournament.Id)
            {
                return BadRequest();
            }

            var userId = _userManager.GetUserId(User);
            if (Tournament.OwnerUserId != userId)
            {
                return Forbid();
            }

            _context.Entry(Tournament).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TournamentExists(id))
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

        // DELETE: api/Tournament/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(long id)
        {
            var Tournament = await _context.Tournament.FindAsync(id);
            if (Tournament == null)
            {
                return NotFound();
            }

            var userId = _userManager.GetUserId(User);
            if (Tournament.OwnerUserId != userId)
            {
                return Forbid();
            }

            _context.Tournament.Remove(Tournament);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool TournamentExists(long id)
        {
            return _context.Tournament.Any(e => e.Id == id);
        }

        // GET: api/Tournament/MyTournaments
        [HttpGet("MyTournaments")]
        public async Task<ActionResult<IEnumerable<Tournament>>> GetMyTournaments()
        {
            var userId = _userManager.GetUserId(User);

            var myTournaments = await _context.Tournament
                .Where(t => t.OwnerUserId == userId)
                .ToListAsync();

            return myTournaments;
        }

        // GET: api/Tournament/Players/5
        [HttpGet("Players/{id}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayers(int id)
        {
            var tournament = await _context.Tournament
                .FirstOrDefaultAsync(m => m.Id == id);

            if (tournament == null)
            {
                return NotFound();
            }

            var players = await _context.Player
                .Where(p => p.TournamentId == tournament.Id)
                .ToListAsync();

            return players;
        }
    }
}
