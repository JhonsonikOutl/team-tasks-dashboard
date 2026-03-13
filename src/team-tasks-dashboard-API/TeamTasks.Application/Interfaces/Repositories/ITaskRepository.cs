using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, TaskFilterDto filter);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task UpdateStatusAsync(int id, UpdateTaskStatusDto filterUpdate);
    }
}
