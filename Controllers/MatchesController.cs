using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Repositories;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("matches")]
    public class MatchController : ControllerBase
    {
        private readonly IMatchRepository _matchRepository;

        public MatchController(IMatchRepository matchRepository)
        {
            _matchRepository = matchRepository;
        }

        // GET: api/match
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Match>>> GetAllMatches()
        {
            var matches = await _matchRepository.GetAllMatchesAsync();
            return Ok(matches);
        }

        // GET: api/match/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Match>> GetMatchById(int id)
        {
            var match = await _matchRepository.GetMatchByIdAsync(id);
            if (match == null)
            {
                return NotFound();
            }
            return Ok(match);
        }

        // POST: api/match
        [HttpPost]
        public async Task<ActionResult<Match>> AddMatch(Match match)
        {
            var createdMatch = await _matchRepository.AddMatchAsync(match);
            return CreatedAtAction(nameof(GetMatchById), new { id = createdMatch.Id }, createdMatch);
        }

        // PUT: api/match/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMatch(int id, Match match)
        {
            var updatedMatch = await _matchRepository.UpdateMatchAsync(id, match);
            if (updatedMatch == null)
            {
                return NotFound();
            }
            return NoContent();
        }

        // DELETE: api/match/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMatch(int id)
        {
            var success = await _matchRepository.DeleteMatchAsync(id);
            if (!success)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("by-event/{eventId}")]
        public async Task<IActionResult> GetMatchesByEventId(int eventId)
        {
            var matches = await _matchRepository.GetMatchesByEventIdAsync(eventId);
            if (matches == null || !matches.Any())
            {
                return NotFound();
            }
            return Ok(matches);
        }

        [HttpPut("match-finish/{id}")]
        public async Task<ActionResult<bool>> MatchFinish(int id)
        {
            return await _matchRepository.MatchFinishAsync(id);
        }

        [HttpPost("find-match")]
        public async Task<ActionResult<bool>> FindMatch(int playerId, int eventId)
        {
            return await _matchRepository.FindMatchAsync(playerId, eventId);
        }
        [HttpPut("update-match-point")]
        public async Task<IActionResult> UpdateMatchPoint(MatchPointUpdateDto match)
        {
            var updatedMatch = await _matchRepository.UpdateMatchPoint(match);
            if (updatedMatch == null)
            {
                return NotFound();
            }
            return NoContent();
        }
    }
}
