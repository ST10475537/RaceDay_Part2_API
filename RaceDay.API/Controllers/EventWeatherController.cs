using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventWeatherController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventWeatherController(RaceDayContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventWeather>>> GetEventWeather()
        {
            return await _context.EventWeather.ToListAsync();
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<EventWeather>> GetWeatherById(int id)
        {
            var weather = await _context.EventWeather.FindAsync(id);

            if (weather == null)
            {
                return NotFound();
            }

            return weather;
        }
        [HttpPost]
        public async Task<ActionResult<EventWeather>> CreateWeather(EventWeather weather)
        {
            weather.RetrievedAt = DateTime.Now;

            _context.EventWeather.Add(weather);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetWeatherById),
                new { id = weather.WeatherID },
                weather);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateWeather(int id, EventWeather weather)
        {
            if (id != weather.WeatherID)
            {
                return BadRequest();
            }

            var existingWeather = await _context.EventWeather.FindAsync(id);

            if (existingWeather == null)
            {
                return NotFound();
            }

            existingWeather.EventID = weather.EventID;
            existingWeather.ForecastDate = weather.ForecastDate;
            existingWeather.Temperature = weather.Temperature;
            existingWeather.WeatherCondition = weather.WeatherCondition;
            existingWeather.WindSpeed = weather.WindSpeed;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWeather(int id)
        {
            var weather = await _context.EventWeather.FindAsync(id);

            if (weather == null)
            {
                return NotFound();
            }

            _context.EventWeather.Remove(weather);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<EventWeather>>> GetWeatherByEvent(int eventId)
        {
            var weather = await _context.EventWeather
                .Where(w => w.EventID == eventId)
                .ToListAsync();

            return weather;
        }
    }
}