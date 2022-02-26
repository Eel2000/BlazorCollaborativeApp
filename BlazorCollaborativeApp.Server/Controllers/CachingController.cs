using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BlazorCollaborativeApp.Shared.Services.Intefaces;
using BlazorCollaborativeApp.Shared.Models;

namespace BlazorCollaborativeApp.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CachingController : ControllerBase
    {
        private readonly ICachingService _cachingService;
        private readonly IHttpContextAccessor _contextAccessor;

        public CachingController(ICachingService cachingService, IHttpContextAccessor contextAccessor)
        {
            _cachingService = cachingService;
            _contextAccessor = contextAccessor;
        }

        [HttpPost("cache-data")]
        public async Task<IActionResult> CacheAsync([FromBody] Sheet sheet)
        {
            await _cachingService.SetAsync(sheet.Id, sheet);
            return Ok(sheet.Id);
        }

        [HttpGet("get-cached-data/{sId}")]
        public async Task<IActionResult> GetCachedDataAsync(string sId)
            => Ok(await _cachingService.GetAsync<Sheet>(sId));

        [HttpPost("cache-list")]
        public async Task<IActionResult> CacheListAsync([FromBody] IEnumerable<Sheet> sheets)
        {
            await _cachingService.SetAsync(HttpContext.Session.Id, sheets);
            return Ok(HttpContext.Session.Id);
        }

        [HttpGet("get-cached-list/{sId}")]
        public async Task<IActionResult> GetCachedListAsync(string sId)
            => Ok(await _cachingService.GetListAsync<Sheet>(sId));

        [HttpDelete("remove-from-cache/{sId}")]
        public async Task<IActionResult> RemoveFromCacheAsync(string sId)
        {
            await _cachingService.DeleteAsync(sId);
            return Ok();
        }

        [HttpPut("update-cached-data")]
        public async Task<IActionResult> UpdateCachedDataAsync([FromBody] Sheet sheet)
        {
            await _cachingService.DeleteAsync(sheet.Id);
            await _cachingService.SetAsync(sheet.Id, sheet);
            return Ok();
        }
    }
}
