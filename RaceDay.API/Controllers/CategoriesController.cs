
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RaceDay.API.Data;
using RaceDay.API.Models;

namespace RaceDay.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly RaceDayContext _context;

        public CategoriesController(RaceDayContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }
        [HttpPost]
        public async Task<ActionResult<Category>> CreateCategory(Category category)
        {
            _context.Categories.Add(category);

            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetCategory),
                new { id = category.CategoryID },
                category);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, Category category)
        {
            if (id != category.CategoryID)
            {
                return BadRequest();
            }

            var existingCategory = await _context.Categories.FindAsync(id);

            if (existingCategory == null)
            {
                return NotFound();
            }

            existingCategory.EventID = category.EventID;
            existingCategory.CategoryName = category.CategoryName;
            existingCategory.DistanceKM = category.DistanceKM;
            existingCategory.MaximumParticipants = category.MaximumParticipants;
            existingCategory.EntryFee = category.EntryFee;

            await _context.SaveChangesAsync();

            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

