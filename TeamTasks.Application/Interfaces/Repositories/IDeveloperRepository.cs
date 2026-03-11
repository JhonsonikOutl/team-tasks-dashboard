using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IDeveloperRepository
    {
        Task<IEnumerable<DeveloperDto>> GetActiveAsync();
    }
}
