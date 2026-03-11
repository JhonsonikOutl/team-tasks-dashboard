using TeamTasks.Application.DTOs;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.Application.Services
{
    public class DashboardService : IDashboardService
    {
        private readonly IDashboardRepository _dashboardRepository;

        public DashboardService(IDashboardRepository dashboardRepository)
        {
            _dashboardRepository = dashboardRepository;
        }

        public async Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync()
        {
            return await _dashboardRepository.GetDeveloperWorkloadAsync();
        }

        public async Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync()
        {
            return await _dashboardRepository.GetProjectHealthAsync();
        }

        public async Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync()
        {
            return await _dashboardRepository.GetDelayRiskAsync();
        }
    }
}
