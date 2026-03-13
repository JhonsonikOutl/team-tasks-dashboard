using TeamTasks.Application.DTOs.Catalog;

namespace TeamTasks.Application.Interfaces.Repositories
{
    public interface ICatalogRepository
    {
        Task<IEnumerable<CatalogItemDto>> GetTaskStatusesAsync();
        Task<IEnumerable<CatalogItemDto>> GetTaskPrioritiesAsync();
    }
}
