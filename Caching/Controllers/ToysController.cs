using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caching.Data;
using Caching.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToysController : ControllerBase
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CachingContext _context;

        public ToysController(CachingContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // GET: api/Toys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Toy>>> GetToys()
        {
            var cacheKey = "toys";

            // get value from memory cache
            if (!_memoryCache.TryGetValue(cacheKey, out List<Toy> toys))
            {
                toys = await _context.Toys.ToListAsync();
                var cacheExpiryOptions = new MemoryCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddMinutes(5),
                    Priority = CacheItemPriority.High,
                    SlidingExpiration = TimeSpan.FromMinutes(2)
                };
                
                // set cache value by key 'toys'
                _memoryCache.Set(cacheKey, toys, cacheExpiryOptions);
            }

            return Ok(toys);
        }
    }
}
