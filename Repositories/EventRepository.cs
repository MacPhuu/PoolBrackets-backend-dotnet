using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.Data;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;

public class EventRepository : IEventRepository
{
    private readonly AppDbContext _context;

    public EventRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Event>> GetEventsAsync()
    {
        return await _context.Events.ToListAsync();
    }

    public async Task<Event> GetEventByIdAsync(int id)
    {
        return await _context.Events.FindAsync(id);
    }

    public async Task AddEventAsync(AddEventDto eventObj)
    {
        var newEvent = new Event
        {
            Name = eventObj.Name,
            Venue = eventObj.Venue,
            Location = eventObj.Location,
            Date = eventObj.Date,
            IsDisplayed = true,
            NumberOfPlayers = eventObj.NumberOfPlayers ?? 0,
            IsHappen = false,
            CreatedAt = DateTime.Now,
            UpdateAt = DateTime.Now,
        };
        _context.Events.Add(newEvent);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateEventAsync(Event eventObj)
    {
        _context.Entry(eventObj).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteEventAsync(int id)
    {
        var eventObj = await _context.Events.FindAsync(id);
        if (eventObj != null)
        {
            _context.Events.Remove(eventObj);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> EventExistsAsync(int id)
    {
        return await _context.Events.AnyAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Event>> GetEventsByNameAsync(string name)
    {
        return await _context.Events
            .Where(e => e.Name.Contains(name))
            .ToListAsync();
    }
}
