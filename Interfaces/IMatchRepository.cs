using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public interface IMatchRepository
    {
        Task<IEnumerable<Match>> GetAllMatchesAsync();
        Task<Match?> GetMatchByIdAsync(int id);
        Task<Match> AddMatchAsync(Match match);
        Task<Match?> UpdateMatchAsync(int id, Match match);
        Task<bool> DeleteMatchAsync(int id);
        Task<List<MatchDto>> GetMatchesByEventIdAsync(int eventId);
        Task<bool> FindMatchAsync(int playerId, int eventId);
        Task<bool> MatchFinishAsync(int id);
        Task<MatchPointUpdateDto?> UpdateMatchPoint(MatchPointUpdateDto match);
    }
}
