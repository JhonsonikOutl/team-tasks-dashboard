using TeamTasks.Application.DTOs;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.Application.Services
{
    public class DeveloperService : IDeveloperService
    {
        private readonly IDeveloperRepository _developerRepository;

        public DeveloperService(IDeveloperRepository developerRepository)
        {
            _developerRepository = developerRepository;
        }

        public async Task<IEnumerable<DeveloperDto>> GetActiveAsync()
        {
            return await _developerRepository.GetActiveAsync();
        }
    }
}
