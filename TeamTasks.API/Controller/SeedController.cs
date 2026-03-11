using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/seed")]
    public class SeedController : ControllerBase
    {
        private readonly ISeedService _seedService;

        public SeedController(ISeedService seedService)
        {
            _seedService = seedService;
        }

        [HttpPost]
        public async Task<IActionResult> Seed()
        {
            await _seedService.SeedAsync();
            return Ok(new { message = "Seed ejecutado correctamente." });
        }
    }
}
