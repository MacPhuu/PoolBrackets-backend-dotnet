using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Repositories;

namespace PoolBrackets_backend_dotnet.Controllers
{
    //[Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PlayerHistoryController : ControllerBase
    {
        private readonly IPlayerHistoryRepository _playerHistoryRepository;

        public PlayerHistoryController(IPlayerHistoryRepository playerHistoryRepository)
        {
            _playerHistoryRepository = playerHistoryRepository;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlayerHistory>>> GetAll()
        {
            var playerHistories = await _playerHistoryRepository.GetAllAsync();
            return Ok(playerHistories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlayerHistory>> GetById(int id)
        {
            var playerHistory = await _playerHistoryRepository.GetByIdAsync(id);
            if (playerHistory == null)
                return NotFound();

            return Ok(playerHistory);
        }

        [HttpPost]
        public async Task<ActionResult<PlayerHistory>> Add(PlayerHistoryDto playerHistory)
        {
            var newPlayerHistory = await _playerHistoryRepository.AddAsync(playerHistory);
            return CreatedAtAction(nameof(GetById), new { id = newPlayerHistory.Id }, newPlayerHistory);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<PlayerHistory>> Update(int id, PlayerHistoryDto playerHistory)
        {
            var updatedPlayerHistory = await _playerHistoryRepository.UpdateAsync(playerHistory);
            if (updatedPlayerHistory == null)
                return NotFound();

            return Ok(updatedPlayerHistory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _playerHistoryRepository.DeleteAsync(id);
            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
