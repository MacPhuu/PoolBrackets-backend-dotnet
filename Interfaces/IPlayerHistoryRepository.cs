using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;
using PoolBrackets_backend_dotnet.Models.Enums;

namespace PoolBrackets_backend_dotnet.Repositories
{
    public interface IPlayerHistoryRepository
    {
        Task<IEnumerable<PlayerHistory>> GetAllAsync();
        Task<PlayerHistory?> GetByIdAsync(int id);
        Task<PlayerHistory?> AddAsync(PlayerHistoryDto playerHistory);
        Task<PlayerHistory?> UpdateAsync(PlayerHistoryDto playerHistory);
        Task<bool> DeleteAsync(int id);
        Task<PlayerHistory?> GetByPlayerIdAndEventIdAsync(int playerId, int eventId);
        Task<PlayerHistory?> UpdatePlayerHistoryAsync(int playerId, int eventId, MatchBranchEnum e);
    }
}
