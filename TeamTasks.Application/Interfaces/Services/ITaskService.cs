using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, int? statusId, int? assigneeId, int page, int pageSize);
        Task<TaskDto?> GetByIdAsync(int id);
        Task<TaskDto> CreateAsync(CreateTaskDto dto);
        Task UpdateStatusAsync(int id, int statusId, int? priorityId, int? estimatedComplexity);
    }
}
