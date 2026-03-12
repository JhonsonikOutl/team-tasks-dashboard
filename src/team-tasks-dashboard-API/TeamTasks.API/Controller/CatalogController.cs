using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class CatalogController : ControllerBase
    {
        private readonly ICatalogService _catalogService;

        public CatalogController(ICatalogService catalogService)
        {
            _catalogService = catalogService;
        }

        [HttpGet("task-statuses")]
        public async Task<IActionResult> GetTaskStatuses()
        {
            var result = await _catalogService.GetTaskStatusesAsync();
            return Ok(result);
        }

        [HttpGet("task-priorities")]
        public async Task<IActionResult> GetTaskPriorities()
        {
            var result = await _catalogService.GetTaskPrioritiesAsync();
            return Ok(result);
        }
    }
}