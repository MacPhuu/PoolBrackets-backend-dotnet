using System.Collections.Generic;
using System.Threading.Tasks;
using PoolBrackets_backend_dotnet.Models;

public interface IPlayerRepository
{
    Task<IEnumerable<Player>> GetPlayersAsync();
    Task<Player> GetPlayerByIdAsync(int id);
    Task AddPlayerAsync(Player player);
    Task UpdatePlayerAsync(Player player);
    Task DeletePlayerAsync(int id);
    Task<bool> PlayerExistsAsync(int id);
    Task<IEnumerable<Player>> GetPlayersByNameAsync(string name);
}
