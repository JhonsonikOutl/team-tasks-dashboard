using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface IProjectService
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDto?> GetByIdAsync(int id);
    }
}
