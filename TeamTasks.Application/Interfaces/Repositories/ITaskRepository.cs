using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, int? statusId, int? assigneeId, int page, int pageSize);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task UpdateStatusAsync(int id, int statusId, int? priorityId, int? estimatedComplexity);
    }
}
