using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/developers")]
    public class DevelopersController : ControllerBase
    {
        private readonly IDeveloperService _developerService;

        public DevelopersController(IDeveloperService developerService)
        {
            _developerService = developerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetActive()
        {
            var developers = await _developerService.GetActiveAsync();
            return Ok(developers);
        }
    }
}