using TeamTasks.Application.DTOs.Tasks;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, TaskFilterDto filter);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task UpdateStatusAsync(int id, UpdateTaskStatusDto filterUpdate);
    }
}
