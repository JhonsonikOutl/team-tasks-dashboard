using TeamTasks.Application.DTOs.Catalog;

namespace TeamTasks.Application.Interfaces.Services
{
    public interface ICatalogService
    {
        Task<IEnumerable<CatalogItemDto>> GetTaskStatusesAsync();
        Task<IEnumerable<CatalogItemDto>> GetTaskPrioritiesAsync();
    }
}