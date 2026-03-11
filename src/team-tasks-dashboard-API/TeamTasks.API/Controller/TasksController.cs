using Microsoft.AspNetCore.Mvc;
using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.API.Controller
{
    [ApiController]
    [Route("api/tasks")]
    public class TasksController : ControllerBase
    {
        private readonly ITaskService _taskService;

        public TasksController(ITaskService taskService)
        {
            _taskService = taskService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTaskDto dto)
        {
            var created = await _taskService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.TaskId }, created);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var task = await _taskService.GetByIdAsync(id);
            if (task is null) return NotFound();
            return Ok(task);
        }

        [HttpPut("{id}/status")]
        public async Task<IActionResult> UpdateStatus(int id, [FromBody] UpdateTaskStatusDto dto)
        {
            await _taskService.UpdateStatusAsync(id, dto.StatusId, dto.PriorityId, dto.EstimatedComplexity);
            return NoContent();
        }
    }
}