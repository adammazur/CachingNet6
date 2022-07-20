using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caching.Data;
using Caching.Models;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToysController : ControllerBase
    {
        private readonly CachingContext _context;

        public ToysController(CachingContext context)
        {
            _context = context;
        }

        // GET: api/Toys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Toy>>> GetToys()
        {
          if (_context.Toys == null)
          {
              return NotFound();
          }
            return await _context.Toys.ToListAsync();
        }
    }
}
