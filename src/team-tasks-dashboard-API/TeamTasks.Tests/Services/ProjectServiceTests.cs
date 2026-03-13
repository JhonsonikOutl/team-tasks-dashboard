using Moq;
using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Services;

namespace TeamTasks.Tests.Services
{
    public class ProjectServiceTests
    {
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly ProjectService _sut;

        public ProjectServiceTests()
        {
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _sut = new ProjectService(_projectRepositoryMock.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsAllProjectsFromRepository()
        {
            var expected = new List<ProjectDto>
            {
                new() { ProjectId = 1, Name = "Proyecto A" },
                new() { ProjectId = 2, Name = "Proyecto B" },
                new() { ProjectId = 3, Name = "Proyecto C" }
            };
            _projectRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(expected);

            var result = await _sut.GetAllAsync();

            Assert.Equal(3, result.Count());
        }

        [Fact]
        public async Task GetAllAsync_EmptyRepository_ReturnsEmptyCollection()
        {
            _projectRepositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<ProjectDto>());

            var result = await _sut.GetAllAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingProject_ReturnsMappedProjectDto()
        {
            var project = new ProjectDetailDto
            {
                ProjectId = 1,
                Name = "Proyecto ABC",
                ClientName = "Cliente ABC"
            };
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(project);

            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(project.ProjectId, result!.ProjectId);
            Assert.Equal(project.Name, result.Name);
            Assert.Equal(project.ClientName, result.ClientName);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingProject_ReturnsNull()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProjectDetailDto?)null);

            var result = await _sut.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task GetByIdAsync_ExistingProject_DoesNotReturnOtherProjects()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(new ProjectDetailDto { ProjectId = 1 });
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((ProjectDetailDto?)null);

            var result1 = await _sut.GetByIdAsync(1);
            var result2 = await _sut.GetByIdAsync(2);

            Assert.NotNull(result1);
            Assert.Null(result2);
        }
    }
}