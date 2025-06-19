using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.Models;

public class PlayerRepository : IPlayerRepository
{
    private readonly AppDbContext _context;

    public PlayerRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Player>> GetPlayersAsync()
    {
        return await _context.Players.ToListAsync();
    }

    public async Task<Player> GetPlayerByIdAsync(int id)
    {
        return await _context.Players.FindAsync(id);
    }

    public async Task AddPlayerAsync(Player player)
    {
        _context.Players.Add(player);
        await _context.SaveChangesAsync();
    }

    public async Task UpdatePlayerAsync(Player player)
    {
        _context.Entry(player).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeletePlayerAsync(int id)
    {
        var player = await _context.Players.FindAsync(id);
        if (player != null)
        {
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> PlayerExistsAsync(int id)
    {
        return await _context.Players.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Player>> GetPlayersByNameAsync(string name)
    {
        return await _context.Players
            .Where(p => p.Name.Contains(name))
            .ToListAsync();
    }
}
