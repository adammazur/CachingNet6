using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Caching.Data;
using Caching.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace Caching.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToysController : ControllerBase
    {
        private readonly IDistributedCache _distributedCache;
        private readonly CachingContext _context;

        public ToysController(CachingContext context, IDistributedCache distributedCache)
        {
            _context = context;
            _distributedCache = distributedCache;
        }

        // GET: api/Toys
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Toy>>> GetToys()
        {
            var cacheKey = "toys";

            // get list of toys from cache by key
            var cachedToys = await _distributedCache.GetAsync(cacheKey);
            
            List<Toy>? toys;
            
            if (cachedToys != null)
            {
                // convert byte array to string and deserialize
                var serialized = Encoding.UTF8.GetString(cachedToys);
                toys = JsonConvert.DeserializeObject<List<Toy>>(serialized);
            }
            else
            {
                // get toys from database
                toys = await _context.Toys.ToListAsync();
                
                // serialize data and convert to byte array
                var serialized = JsonConvert.SerializeObject(toys);
                cachedToys = Encoding.UTF8.GetBytes(serialized);
                
                // caching properties
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(2))
                    .SetSlidingExpiration(TimeSpan.FromSeconds(20));
                
                // storing data in distributed cache - Redis database
                await _distributedCache.SetAsync(cacheKey, cachedToys, options);
            }

            return Ok(toys);
        }
    }
}
