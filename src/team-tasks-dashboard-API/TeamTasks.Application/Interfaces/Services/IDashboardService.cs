using TeamTasks.Application.DTOs;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.DTOs.Projects;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface IDashboardService
    {
        Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync();
        Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync();
        Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync();
    }
}
