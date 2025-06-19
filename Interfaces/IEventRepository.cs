using System.Collections.Generic;
using System.Threading.Tasks;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;

public interface IEventRepository
{
    Task<IEnumerable<Event>> GetEventsAsync();
    Task<Event> GetEventByIdAsync(int id);
    Task AddEventAsync(AddEventDto eventObj);
    Task UpdateEventAsync(Event eventObj);
    Task DeleteEventAsync(int id);
    Task<bool> EventExistsAsync(int id);
    Task<IEnumerable<Event>> GetEventsByNameAsync(string name);
}
