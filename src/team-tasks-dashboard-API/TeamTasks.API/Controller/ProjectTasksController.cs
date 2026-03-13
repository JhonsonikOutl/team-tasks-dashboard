using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/projects/{projectId}/tasks")]
    public class ProjectTasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public ProjectTasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpGet]
        public async Task<IActionResult> GetByProject(int projectId, [FromQuery] TaskFilterDto filter)
        {
            var tasks = await _taskService.GetByProjectIdAsync(projectId, filter);
            return Ok(tasks);
        }
    }
}