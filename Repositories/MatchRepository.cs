using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Data;
using System.Collections.Generic;
using System.Threading.Tasks;
using PoolBrackets_backend_dotnet.DTOs;
using Microsoft.AspNetCore.Identity.Data;
using PoolBrackets_backend_dotnet.Models.Enums;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public class MatchRepository : IMatchRepository
    {
        private readonly AppDbContext _context;
        private readonly IEventRepository _eventRepository;
        private readonly IPlayerRepository _playerRepository;
        private readonly IPlayerHistoryRepository _playerHistoryRepository;

        public MatchRepository(AppDbContext context, IEventRepository eventRepository, IPlayerRepository playerRepository, IPlayerHistoryRepository playerHistoryRepository)
        {
            _context = context;
            _eventRepository = eventRepository;
            _playerRepository = playerRepository;
            _playerHistoryRepository = playerHistoryRepository;
        }

        public async Task<IEnumerable<Match>> GetAllMatchesAsync()
        {
            return await _context.Matches.ToListAsync();
        }

        public async Task<Match?> GetMatchByIdAsync(int id)
        {
            return await _context.Matches.FindAsync(id);
        }

        public async Task<Match> AddMatchAsync(Match match)
        {
            _context.Matches.Add(match);
            await _context.SaveChangesAsync();
            return match;
        }

        public async Task<Match?> UpdateMatchAsync(int id, Match match)
        {
            var existingMatch = await _context.Matches.FindAsync(id);
            if (existingMatch != null)
            {
                existingMatch.EventId = match.EventId;
                existingMatch.Table = match.Table;
                existingMatch.FirstPlayerId = match.FirstPlayerId;
                existingMatch.FirstPlayerPoint = match.FirstPlayerPoint;
                existingMatch.SecondPlayerId = match.SecondPlayerId;
                existingMatch.SecondPlayerPoint = match.SecondPlayerPoint;
                existingMatch.IsFinish = match.IsFinish;

                await _context.SaveChangesAsync();
            }
            return existingMatch;
        }

        public async Task<bool> DeleteMatchAsync(int id)
        {
            var match = await _context.Matches.FindAsync(id);
            if (match != null)
            {
                _context.Matches.Remove(match);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<List<MatchDto>> GetMatchesByEventIdAsync(int eventId)
        {
            return await _context.Matches
                .Where(m => m.EventId == eventId)
                .Include(m => m.SecondPlayer)
                .Select(m => new MatchDto
                {
                    Id = m.Id,
                    EventName = m.Event.Name,             
                    Table = m.Table,
                    FirstPlayerName = m.FirstPlayer != null ? m.FirstPlayer.Name : "Unknown",
                    FirstPlayerPoint = m.FirstPlayerPoint,
                    SecondPlayerName = m.SecondPlayer != null ? m.SecondPlayer.Name : "Unknown",
                    SecondPlayerPoint = m.SecondPlayerPoint,
                    IsStart = m.IsStart,
                    IsFinish = m.IsFinish,
                    Stage =m.Stage
                })
                .ToListAsync();
        }

        public async Task<bool> FindMatchAsync(int playerId, int eventId)
        {
            var playerHistory = await _playerHistoryRepository.GetByPlayerIdAndEventIdAsync(playerId, eventId);

            if(playerHistory == null)
            {
                var newHistory = new PlayerHistoryDto
                {
                    PlayerId = playerId,
                    EventId = eventId,
                    GroupStageTotal = 0,
                    GroupStageWin = 0,
                    KnockoutStageTotal = 0,
                    KnockoutStageWin = 0
                };
                await _playerHistoryRepository.AddAsync(newHistory);

                var availableMatch = await _context.Matches
                .Where(m => m.EventId == eventId && m.Stage == MatchStageEnum.GroupStage && m.Branch == null && m.SecondPlayer == null)
                .Include(m => m.SecondPlayer)
                .FirstOrDefaultAsync();

                if (availableMatch == null)
                {
                    var newMatch = new Match
                    {
                        EventId = eventId,
                        FirstPlayerId = playerId,
                        FirstPlayerPoint = 0,
                        SecondPlayerPoint = 0,
                        IsStart = false,
                        IsFinish =  false,
                        Branch = null,
                        Stage = MatchStageEnum.GroupStage,
                    };
                    _context.Matches.Add(newMatch);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    if (availableMatch.FirstPlayerId == playerId) return false;
                    availableMatch.SecondPlayerId = playerId;

                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            else
            {
                var curBranch = (MatchBranchEnum?)MatchBranchEnum.Win;
                var curStage = MatchStageEnum.GroupStage;
                if (playerHistory.GroupStageWin == 2)
                {
                    curBranch = null;
                    if (playerHistory.KnockoutStageWin != playerHistory.KnockoutStageTotal) return false;
                    else if (playerHistory.KnockoutStageTotal == 0) curStage = MatchStageEnum.RoundOf16;
                    else if (playerHistory.KnockoutStageTotal == 1) curStage = MatchStageEnum.QuarterFinal;
                    else if (playerHistory.KnockoutStageTotal == 2) curStage = MatchStageEnum.SemiFinal;
                    else if (playerHistory.KnockoutStageTotal == 3) curStage = MatchStageEnum.Final;
                    else return false;
                }
                else
                {
                    if (playerHistory.GroupStageWin +1 < playerHistory.GroupStageTotal) return false;
                    if (playerHistory.GroupStageTotal > playerHistory.GroupStageWin) curBranch = MatchBranchEnum.Lose;
                    else curBranch = MatchBranchEnum.Win;
                }
                var availableMatch = await _context.Matches
                .Where(m => m.EventId == eventId && m.Stage == curStage && m.Branch == curBranch && m.SecondPlayer == null)
                .Include(m => m.SecondPlayer)
                .FirstOrDefaultAsync();

                if (availableMatch == null)
                {
                    var newMatch = new Match
                    {
                        EventId = eventId,
                        FirstPlayerId = playerId,
                        FirstPlayerPoint = 0,
                        SecondPlayerPoint = 0,
                        IsStart = false,
                        IsFinish = false,
                        Branch = curBranch,
                        Stage = curStage,
                    };
                    _context.Matches.Add(newMatch);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    if (availableMatch.FirstPlayerId == playerId) return false;
                    availableMatch.SecondPlayerId = playerId;

                    await _context.SaveChangesAsync();
                    return true;
                }
            }

        }

        public async Task<bool> MatchFinishAsync(int id)
        {
            var match = await this.GetMatchByIdAsync(id);
            if(match == null || match.IsStart == false)
            {
                return false;
            }

            if(match.IsFinish == true)
            {
                return true;
            }

            match.IsFinish = true;

            var firstPlayerId = match.FirstPlayerId;
            var secondPlayerId = match.SecondPlayerId.Value;
            var eventid = match.EventId;

            if(match.FirstPlayerPoint > match.SecondPlayerPoint)
            {
                await _playerHistoryRepository.UpdatePlayerHistoryAsync(firstPlayerId, eventid, MatchBranchEnum.Win);
                await _playerHistoryRepository.UpdatePlayerHistoryAsync(secondPlayerId, eventid, MatchBranchEnum.Lose);
            }else
            {
                await _playerHistoryRepository.UpdatePlayerHistoryAsync(firstPlayerId, eventid, MatchBranchEnum.Lose);
                await _playerHistoryRepository.UpdatePlayerHistoryAsync(secondPlayerId, eventid, MatchBranchEnum.Win);
            }

            await this.FindMatchAsync(firstPlayerId, eventid);
            await this.FindMatchAsync(secondPlayerId, eventid);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<MatchPointUpdateDto?> UpdateMatchPoint(MatchPointUpdateDto match)
        {
            var existedMatch = await this.GetMatchByIdAsync(match.Id);
            if(existedMatch == null)
            {
                return null;
            }

            existedMatch.FirstPlayerPoint = match.FirstPlayerPoint;
            existedMatch.SecondPlayerPoint = match.SecondPlayerPoint;

            await _context.SaveChangesAsync();
            return new MatchPointUpdateDto
            {
                Id = existedMatch.Id,
                FirstPlayerPoint = existedMatch.FirstPlayerPoint,
                SecondPlayerPoint = existedMatch.SecondPlayerPoint,
            };
        }
    }
}
