using TeamTasks.Application.DTOs.Projects;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<ProjectDetailDto?> GetByIdAsync(int id);
    }
}
