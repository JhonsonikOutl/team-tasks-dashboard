using TeamTasks.Application.DTOs;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.DTOs.Projects;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync();
        Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync();
        Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync();
    }
}
