using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IProjectRepository
    {
        Task<IEnumerable<ProjectDto>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
    }
}
