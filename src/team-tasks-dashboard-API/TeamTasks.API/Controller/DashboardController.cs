using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/dashboard")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet("developer-workload")]
        public async Task<IActionResult> GetDeveloperWorkload()
        {
            var result = await _dashboardService.GetDeveloperWorkloadAsync();
            return Ok(result);
        }

        [HttpGet("project-health")]
        public async Task<IActionResult> GetProjectHealth()
        {
            var result = await _dashboardService.GetProjectHealthAsync();
            return Ok(result);
        }

        [HttpGet("developer-delay-risk")]
        public async Task<IActionResult> GetDelayRisk()
        {
            var result = await _dashboardService.GetDelayRiskAsync();
            return Ok(result);
        }
    }
}