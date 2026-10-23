
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventsController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Event>>> GetEvents()
        {
            return await _context.Events.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Event>> GetEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            return eventItem;
        }

        [HttpPost]
        public async Task<ActionResult<Event>> CreateEvent(Event eventItem)
        {
            _context.Events.Add(eventItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEvent),
                new { id = eventItem.EventID },
                eventItem);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEvent(int id, Event eventItem)
        {
            if (id != eventItem.EventID)
            {
                return BadRequest();
            }

            var existingEvent = await _context.Events.FindAsync(id);

            if (existingEvent == null)
            {
                return NotFound();
            }

            existingEvent.OrganiserID = eventItem.OrganiserID;
            existingEvent.EventName = eventItem.EventName;
            existingEvent.Description = eventItem.Description;
            existingEvent.EventDate = eventItem.EventDate;
            existingEvent.Location = eventItem.Location;

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEvent(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);

            if (eventItem == null)
            {
                return NotFound();
            }

            _context.Events.Remove(eventItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

