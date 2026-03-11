using TeamTasks.Application.Interfaces.Services;
using TeamTasks.Application.Interfaces.Repositories;

namespace TeamTasks.Application.Services
{
    public class SeedService : ISeedService
    {
        private readonly ISeedRepository _seedRepository;

        public SeedService(ISeedRepository seedRepository)
        {
            _seedRepository = seedRepository;
        }

        public async Task SeedAsync()
        {
            if (await _seedRepository.HasDataAsync())
                throw new InvalidOperationException("La base de datos ya contiene información semilla. El seed no se ejecutará.");

            await _seedRepository.SeedReferenceDataAsync();
            await _seedRepository.SeedDevelopersAsync();
            await _seedRepository.SeedProjectsAsync();
            await _seedRepository.SeedTasksAsync();
        }
    }
}
