using TeamTasks.Domain.Entities;

namespace TeamTasks.Application.Interfaces
{
    public interface IDeveloperRepository
    {
        Task<IEnumerable<Developer>> GetActiveAsync();
    }
}
