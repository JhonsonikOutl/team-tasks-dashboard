using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync();
        Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync();
        Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync();
    }
}
