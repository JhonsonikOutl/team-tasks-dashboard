using TeamTasks.Application.DTOs.Developers;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IDeveloperRepository
    {
        Task<IEnumerable<DeveloperDto>> GetActiveAsync();
    }
}
