using TeamTasks.Application.DTOs.Developers;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface IDeveloperService
    {
        Task<IEnumerable<DeveloperDto>> GetActiveAsync();
    }
}
