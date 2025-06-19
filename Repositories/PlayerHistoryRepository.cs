using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Data;
using Microsoft.VisualBasic;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models.Enums;
using PoolBrackets_backend_dotnet.Controllers;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public class PlayerHistoryRepository : IPlayerHistoryRepository
    {
        private readonly AppDbContext _context;
        private readonly IEventRepository _eventRepository;
        private readonly IPlayerRepository _playerRepository;

        public PlayerHistoryRepository(AppDbContext context, IEventRepository eventRepository, IPlayerRepository playerRepository)
        {
            _context = context;
            _eventRepository = eventRepository;
            _playerRepository = playerRepository;
        }

        public async Task<IEnumerable<PlayerHistory>> GetAllAsync()
        {
            return await _context.PlayerHistories
                .Include(ph => ph.Player)
                .Include(ph => ph.Event)
                .ToListAsync();
        }

        public async Task<PlayerHistory?> GetByIdAsync(int id)
        {
            return await _context.PlayerHistories
                .Include(ph => ph.Player)
                .Include(ph => ph.Event)
                .FirstOrDefaultAsync(ph => ph.Id == id);
        }

        public async Task<PlayerHistory?> AddAsync(PlayerHistoryDto playerHistoryDto)
        {
            var player = await _playerRepository.GetPlayerByIdAsync(playerHistoryDto.PlayerId);
            var eventItem = await _eventRepository.GetEventByIdAsync(playerHistoryDto.EventId);

            if (player == null || eventItem == null)
            {
                return null; 
            }

            var playerHistory = new PlayerHistory
            {
                PlayerId = playerHistoryDto.PlayerId,
                EventId = playerHistoryDto.EventId,
                GroupStageTotal = playerHistoryDto.GroupStageTotal,
                GroupStageWin = playerHistoryDto.GroupStageWin,
                KnockoutStageTotal = playerHistoryDto.KnockoutStageTotal,
                KnockoutStageWin = playerHistoryDto.KnockoutStageWin
            };

            _context.PlayerHistories.Add(playerHistory);
            await _context.SaveChangesAsync();
            return playerHistory;
        }

        public async Task<PlayerHistory?> UpdateAsync(PlayerHistoryDto playerHistoryDto)
        {
            var existingPlayerHistory = await this.GetByPlayerIdAndEventIdAsync(playerHistoryDto.PlayerId,playerHistoryDto.EventId);
            if (existingPlayerHistory == null) {
                await this.AddAsync(playerHistoryDto);
            }

            var player = await _playerRepository.GetPlayerByIdAsync(playerHistoryDto.PlayerId);
            var eventItem = await _eventRepository.GetEventByIdAsync(playerHistoryDto.EventId);

            if (player == null || eventItem == null)
            {
                return null;
            }

            existingPlayerHistory.PlayerId = playerHistoryDto.PlayerId;
            existingPlayerHistory.EventId = playerHistoryDto.EventId;
            existingPlayerHistory.GroupStageTotal = playerHistoryDto.GroupStageTotal;
            existingPlayerHistory.GroupStageWin = playerHistoryDto.GroupStageWin;
            existingPlayerHistory.KnockoutStageTotal = playerHistoryDto.KnockoutStageTotal;
            existingPlayerHistory.KnockoutStageWin = playerHistoryDto.KnockoutStageWin;

            await _context.SaveChangesAsync();
            return existingPlayerHistory;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var playerHistory = await _context.PlayerHistories.FindAsync(id);
            if (playerHistory == null) return false;

            _context.PlayerHistories.Remove(playerHistory);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<PlayerHistory?> GetByPlayerIdAndEventIdAsync(int playerId, int eventId)
        {
            return await _context.PlayerHistories
                .Include(ph => ph.Player)
                .Include(ph => ph.Event)
                .FirstOrDefaultAsync(ph => ph.PlayerId == playerId && ph.EventId == eventId);
        }

        public async Task<PlayerHistory?> UpdatePlayerHistoryAsync(int playerId, int eventId, MatchBranchEnum e)
        {
            var playerHistory = await this.GetByPlayerIdAndEventIdAsync(playerId, eventId);
            if(playerHistory == null)
            {
                var newHistory = new PlayerHistory
                {
                    PlayerId = playerId,
                    EventId = eventId,
                    GroupStageTotal = 1,
                    GroupStageWin = e == MatchBranchEnum.Win?1:0,
                    KnockoutStageTotal = 0,
                    KnockoutStageWin = 0
                };

                await _context.SaveChangesAsync();
                return newHistory;
            }

            if(playerHistory.GroupStageWin<2)
            {
                playerHistory.GroupStageTotal += 1;
                playerHistory.GroupStageWin += e == MatchBranchEnum.Win ? 1 : 0;

                await _context.SaveChangesAsync();
                return playerHistory;
            }

            playerHistory.KnockoutStageTotal += 1;
            playerHistory.KnockoutStageWin += e == MatchBranchEnum.Win ? 1 : 0;

            await _context.SaveChangesAsync();
            return playerHistory;
        } 
    }
}
