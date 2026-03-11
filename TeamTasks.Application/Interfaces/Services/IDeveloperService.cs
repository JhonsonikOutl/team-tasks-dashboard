using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface IDeveloperService
    {
        Task<IEnumerable<DeveloperDto>> GetActiveAsync();
    }
}
