namespace TeamTasks.Application.Interfaces
{
    public interface IDashboardRepository
    {
        Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync();
        Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync();
        Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync();
    }
}
