using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Controllers
{
    [Authorize]
    [Route("players")]
    [ApiController]
    public class PlayersController : ControllerBase
    {
        private readonly IPlayerRepository _playerRepository;

        public PlayersController(IPlayerRepository playerRepository)
        {
            _playerRepository = playerRepository;
        }

        // GET: api/Players
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayer()
        {
            var players = await _playerRepository.GetPlayersAsync();
            return Ok(players);
        }

        // GET: api/Players/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Player>> GetPlayer(int id)
        //{
        //    var player = await _playerRepository.GetPlayerByIdAsync(id);

        //    if (player == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(player);
        //}

        // PUT: api/Players/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPlayer(int id, Player player)
        {
            if (id != player.Id)
            {
                return BadRequest();
            }

            try
            {
                await _playerRepository.UpdatePlayerAsync(player);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _playerRepository.PlayerExistsAsync(id))
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

        // POST: api/Players
        [HttpPost]
        public async Task<ActionResult<Player>> PostPlayer(Player player)
        {
            await _playerRepository.AddPlayerAsync(player);
            return CreatedAtAction("GetPlayer", new { id = player.Id }, player);
        }

        // DELETE: api/Players/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlayer(int id)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(id);
            if (player == null)
            {
                return NotFound();
            }

            await _playerRepository.DeletePlayerAsync(id);

            return NoContent();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Player>>> GetPlayersByName(string name)
        {
            var players = await _playerRepository.GetPlayersByNameAsync(name);
            if (!players.Any())
            {
                return NotFound();
            }
            return Ok(players);
        }

    }
}
