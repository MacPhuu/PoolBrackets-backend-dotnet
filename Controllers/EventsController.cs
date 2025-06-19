using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PoolBrackets_backend_dotnet.DTOs;
using PoolBrackets_backend_dotnet.Models;

namespace PoolBrackets_backend_dotnet.Controllers
{
    //[Authorize]
    [Route("events")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventRepository _eventRepository;

        public EventsController(IEventRepository eventRepository)
        {
            _eventRepository = eventRepository;
        }

        // GET: api/Events
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            var events = await _eventRepository.GetEventsAsync();
            return Ok(events);
        }

        // GET: api/Events/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Event>> GetEvent(int id)
        //{
        //    var eventObj = await _eventRepository.GetEventByIdAsync(id);

        //    if (eventObj == null)
        //    {
        //        return NotFound();
        //    }

        //    return Ok(eventObj);
        //}

        // PUT: api/Events/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEvent(int id, Event eventObj)
        {
            if (id != eventObj.Id)
            {
                return BadRequest();
            }

            try
            {
                await _eventRepository.UpdateEventAsync(eventObj);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _eventRepository.EventExistsAsync(id))
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

        // POST: api/Events
        [HttpPost]
        public async Task<ActionResult<AddEventDto>> PostEvent(AddEventDto eventObj)
        {
            await _eventRepository.AddEventAsync(eventObj);
            return Ok(eventObj);
        }

        // DELETE: api/Events/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventObj = await _eventRepository.GetEventByIdAsync(id);
            if (eventObj == null)
            {
                return NotFound();
            }

            await _eventRepository.DeleteEventAsync(id);

            return NoContent();
        }

        [HttpGet("{name}")]
        public async Task<ActionResult<IEnumerable<Event>>> GetEventsByName(string name)
        {
            var events = await _eventRepository.GetEventsByNameAsync(name);
            if (!events.Any())
            {
                return NotFound();
            }
            return Ok(events);
        }
    }
}
