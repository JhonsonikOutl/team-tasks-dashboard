using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces
{
    public interface ITaskRepository
    {
        Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId, int? statusId, int? assigneeId, int page, int pageSize);
        Task<TaskItem?> GetByIdAsync(int id);
        Task<TaskItem> CreateAsync(TaskItem task);
        Task UpdateStatusAsync(int id, int statusId, int? priorityId, int? estimatedComplexity);
    }
}
