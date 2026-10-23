using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventEnrollmentsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public EventEnrollmentsController(RaceDayContext context)
        {
            _context = context;
        }

        // GET: api/EventEnrollments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EventEnrollment>>> GetEnrollments()
        {
            return await _context.EventEnrollments.ToListAsync();
        }

        // GET: api/EventEnrollments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EventEnrollment>> GetEnrollment(int id)
        {
            var enrollment = await _context.EventEnrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            return enrollment;
        }
        // POST: api/EventEnrollments
[HttpPost]
public async Task<ActionResult<EventEnrollment>> CreateEnrollment(
    EventEnrollment enrollment)
        {
            enrollment.EnrollmentDate = DateTime.Now;

            _context.EventEnrollments.Add(enrollment);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetEnrollment),
                new { id = enrollment.EnrollmentID },
                enrollment);
        }
            
// PUT: api/EventEnrollments/5
[HttpPut("{id}")]
public async Task<IActionResult> UpdateEnrollment(
    int id, EventEnrollment enrollment)
        {
            if (id != enrollment.EnrollmentID)
            {
                return BadRequest();
            }

            var existingEnrollment =
                await _context.EventEnrollments.FindAsync(id);

            if (existingEnrollment == null)
            {
                return NotFound();
            }

            existingEnrollment.EventID = enrollment.EventID;
            existingEnrollment.ParticipantID = enrollment.ParticipantID;
            existingEnrollment.CategoryID = enrollment.CategoryID;
            existingEnrollment.Status = enrollment.Status;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
// DELETE: api/EventEnrollments/5
[HttpDelete("{id}")]
public async Task<IActionResult> DeleteEnrollment(int id)
        {
            var enrollment =
                await _context.EventEnrollments.FindAsync(id);

            if (enrollment == null)
            {
                return NotFound();
            }

            _context.EventEnrollments.Remove(enrollment);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

