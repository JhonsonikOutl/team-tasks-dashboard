using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;

namespace TeamTasks.Application.Services
{
    public class ProjectService : IProjectService
    {
        private readonly IProjectRepository _projectRepository;

        public ProjectService(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            return await _projectRepository.GetAllAsync();
        }

        public async Task<ProjectDto?> GetByIdAsync(int id)
        {
            var project = await _projectRepository.GetByIdAsync(id);
            if (project is null) return null;

            return new ProjectDto
            {
                ProjectId = project.ProjectId,
                Name = project.Name,
                ClientName = project.ClientName
            };
        }
    }
}
