using TeamTasks.Application.DTOs.Catalog;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.Application.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly ICatalogRepository _catalogRepository;

        public CatalogService(ICatalogRepository catalogRepository)
        {
            _catalogRepository = catalogRepository;
        }

        public async Task<IEnumerable<CatalogItemDto>> GetTaskStatusesAsync()
        {
            return await _catalogRepository.GetTaskStatusesAsync();
        }

        public async Task<IEnumerable<CatalogItemDto>> GetTaskPrioritiesAsync()
        {
            return await _catalogRepository.GetTaskPrioritiesAsync();
        }
    }
}