using TeamTasks.Application.DTOs;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync();
        Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync();
        Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync();
    }
}
