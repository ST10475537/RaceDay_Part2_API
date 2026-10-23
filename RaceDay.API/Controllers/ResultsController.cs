using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ResultsController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public ResultsController(RaceDayContext context)
        {
            _context = context;
        }

        // Get all results
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Result>>> GetResults()
        {
            return await _context.Results.ToListAsync();
        }

        // Get one result by ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Result>> GetResult(int id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }
        // Create a result
        [HttpPost]
        public async Task<ActionResult<Result>> CreateResult(Result result)
        {
            _context.Results.Add(result);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetResult),
                new { id = result.ResultID },
                result);
        }        
        // Update a result
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateResult(int id, Result result)
        {
            if (id != result.ResultID)
            {
                return BadRequest();
            }

            var existingResult = await _context.Results.FindAsync(id);

            if (existingResult == null)
            {
                return NotFound();
            }

            existingResult.EnrollmentID = result.EnrollmentID;
            existingResult.FinishPosition = result.FinishPosition;
            existingResult.FinishTime = result.FinishTime;
            existingResult.Pace = result.Pace;
            existingResult.ResultStatus = result.ResultStatus;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        
        // Delete a result
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteResult(int id)
        {
            var result = await _context.Results.FindAsync(id);

            if (result == null)
            {
                return NotFound();
            }

            _context.Results.Remove(result);
            await _context.SaveChangesAsync();

            return NoContent();
        }        
        // Get results for an event
        [HttpGet("event/{eventId}")]
        public async Task<ActionResult<IEnumerable<Result>>> GetResultsByEvent(int eventId)
        {
            var results = await _context.Results
                .Join(
                    _context.EventEnrollments,
                    result => result.EnrollmentID,
                    enrollment => enrollment.EnrollmentID,
                    (result, enrollment) => new { result, enrollment })
                .Where(x => x.enrollment.EventID == eventId)
                .Select(x => x.result)
                .ToListAsync();

            return results;
        }

    }
}
