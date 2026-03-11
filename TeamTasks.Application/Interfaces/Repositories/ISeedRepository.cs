namespace TeamTasks.Infrastructure.Repositories
{
    public interface ISeedRepository
    {
        Task<bool> HasDataAsync();
        Task SeedReferenceDataAsync();
        Task SeedDevelopersAsync();
        Task SeedProjectsAsync();
        Task SeedTasksAsync();
    }
}