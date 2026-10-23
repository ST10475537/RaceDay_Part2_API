using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventRoutesController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventRoutesController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventRoute>>> GetEventRoutes()
        {
            return await _context.EventRoutes.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EventRoute>> GetEventRoute(int id)
        {
            var route = await _context.EventRoutes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            return route;
        }
        [HttpPost]
        public async Task<ActionResult<EventRoute>> CreateEventRoute(EventRoute route)
        {
            _context.EventRoutes.Add(route);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEventRoute),
                new { id = route.RouteID },
                route);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEventRoute(int id, EventRoute route)
        {
            if (id != route.RouteID)
            {
                return BadRequest();
            }

            var existingRoute = await _context.EventRoutes.FindAsync(id);

            if (existingRoute == null)
            {
                return NotFound();
            }

            existingRoute.EventID = route.EventID;
            existingRoute.RouteName = route.RouteName;
            existingRoute.DistanceKM = route.DistanceKM;
            existingRoute.RouteDescription = route.RouteDescription;
            existingRoute.RouteMapURL = route.RouteMapURL;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEventRoute(int id)
        {
            var route = await _context.EventRoutes.FindAsync(id);

            if (route == null)
            {
                return NotFound();
            }

            _context.EventRoutes.Remove(route);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EventRoute>>> GetRoutesByEvent(int eventId)
        {
            var routes = await _context.EventRoutes
                .Where(r => r.EventID == eventId)
                .ToListAsync();

            return routes;
        }
    }
}