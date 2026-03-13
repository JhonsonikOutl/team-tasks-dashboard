using Moq;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Services;
using TeamTasks.Domain.Entities;

namespace TeamTasks.Tests.Services
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskRepository> _taskRepositoryMock;
        private readonly Mock<IProjectRepository> _projectRepositoryMock;
        private readonly Mock<IDeveloperRepository> _developerRepositoryMock;
        private readonly TaskService _sut;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _projectRepositoryMock = new Mock<IProjectRepository>();
            _developerRepositoryMock = new Mock<IDeveloperRepository>();

            _sut = new TaskService(
                _taskRepositoryMock.Object,
                _projectRepositoryMock.Object,
                _developerRepositoryMock.Object
            );
        }

        [Fact]
        public async Task GetByProjectIdAsync_ReturnsTasksFromRepository()
        {
            var expected = new List<TaskDto> { new() { TaskId = 1 }, new() { TaskId = 2 } };
            _taskRepositoryMock.Setup(r => r.GetByProjectIdAsync(1, null, null, 1, 10)).ReturnsAsync(expected);

            var result = await _sut.GetByProjectIdAsync(1, null, null, 1, 10);

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetByIdAsync_ExistingTask_ReturnsTaskDto()
        {
            var expected = new TaskDto { TaskId = 1, Title = "Tarea existente" };
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expected);

            var result = await _sut.GetByIdAsync(1);

            Assert.NotNull(result);
            Assert.Equal(expected.TaskId, result!.TaskId);
        }

        [Fact]
        public async Task GetByIdAsync_NonExistingTask_ReturnsNull()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskDto?)null);

            var result = await _sut.GetByIdAsync(99);

            Assert.Null(result);
        }

        [Fact]
        public async Task CreateAsync_ProjectNotFound_ThrowsArgumentException()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((ProjectDetailDto?)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(BuildCreateTaskDto()));
        }

        [Fact]
        public async Task CreateAsync_DeveloperNotFound_ThrowsArgumentException()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProjectDetailDto());
            _developerRepositoryMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(new List<DeveloperDto>());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(BuildCreateTaskDto(assigneeId: 99)));
        }

        [Fact]
        public async Task CreateAsync_ComplexityBelowRange_ThrowsArgumentException()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProjectDetailDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(BuildCreateTaskDto(estimatedComplexity: 0)));
        }

        [Fact]
        public async Task CreateAsync_ComplexityAboveRange_ThrowsArgumentException()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProjectDetailDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(BuildCreateTaskDto(estimatedComplexity: 6)));
        }

        [Fact]
        public async Task CreateAsync_DueDateInThePast_ThrowsArgumentException()
        {
            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProjectDetailDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.CreateAsync(BuildCreateTaskDto(dueDate: DateOnly.FromDateTime(DateTime.Today.AddDays(-1)))));
        }

        [Fact]
        public async Task CreateAsync_ValidData_ReturnsCreatedTaskDto()
        {
            var expected = new TaskDto { TaskId = 1, Title = "Tarea de prueba" };

            _projectRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new ProjectDetailDto());
            _taskRepositoryMock.Setup(r => r.CreateAsync(It.IsAny<TaskItem>())).ReturnsAsync(new TaskItem { TaskId = 1 });
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(expected);

            var result = await _sut.CreateAsync(BuildCreateTaskDto());

            Assert.NotNull(result);
            Assert.Equal(expected.TaskId, result.TaskId);
            Assert.Equal(expected.Title, result.Title);
        }

        [Fact]
        public async Task UpdateStatusAsync_TaskNotFound_ThrowsArgumentException()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync((TaskDto?)null);

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateStatusAsync(99, 1, null, null));
        }

        [Fact]
        public async Task UpdateStatusAsync_InvalidStatus_ThrowsArgumentException()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TaskDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateStatusAsync(1, 99, null, null));
        }

        [Fact]
        public async Task UpdateStatusAsync_InvalidPriority_ThrowsArgumentException()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TaskDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateStatusAsync(1, 1, 99, null));
        }

        [Fact]
        public async Task UpdateStatusAsync_ComplexityOutOfRange_ThrowsArgumentException()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TaskDto());

            await Assert.ThrowsAsync<ArgumentException>(() => _sut.UpdateStatusAsync(1, 1, null, 6));
        }

        [Fact]
        public async Task UpdateStatusAsync_ValidData_ExecutesWithoutError()
        {
            _taskRepositoryMock.Setup(r => r.GetByIdAsync(It.IsAny<int>())).ReturnsAsync(new TaskDto());
            _taskRepositoryMock.Setup(r => r.UpdateStatusAsync(It.IsAny<int>(), It.IsAny<int>(), It.IsAny<int?>(), It.IsAny<int?>())).Returns(Task.CompletedTask);

            var exception = await Record.ExceptionAsync(() => _sut.UpdateStatusAsync(1, 1, 1, 3));

            Assert.Null(exception);
        }

        private static CreateTaskDto BuildCreateTaskDto(
            int? assigneeId = null,
            int estimatedComplexity = 3,
            DateOnly? dueDate = null)
        {
            return new CreateTaskDto
            {
                ProjectId = 1,
                Title = "Tarea de prueba",
                Description = "Descripción de prueba",
                AssigneeId = assigneeId,
                StatusId = 1,
                PriorityId = 1,
                EstimatedComplexity = estimatedComplexity,
                DueDate = dueDate ?? DateOnly.FromDateTime(DateTime.Today.AddDays(7))
            };
        }
    }
}