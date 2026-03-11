using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces
{
    public interface IProjectRepository
    {
        Task<IEnumerable<Project>> GetAllAsync();
        Task<Project?> GetByIdAsync(int id);
    }
}
