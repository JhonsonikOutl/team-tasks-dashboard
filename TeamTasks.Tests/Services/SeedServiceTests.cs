using Moq;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Services;

namespace TeamTasks.Tests.Services
{
    public class SeedServiceTests
    {
        private readonly Mock<ISeedRepository> _seedRepositoryMock;
        private readonly SeedService _seedService;

        public SeedServiceTests()
        {
            _seedRepositoryMock = new Mock<ISeedRepository>();
            _seedService = new SeedService(_seedRepositoryMock.Object);
        }

        [Fact]
        public async Task SeedAsync_AlreadyHasData_ThrowsInvalidOperationException()
        {
            _seedRepositoryMock.Setup(r => r.HasDataAsync()).ReturnsAsync(true);

            await Assert.ThrowsAsync<InvalidOperationException>(() => _seedService.SeedAsync());
        }

        [Fact]
        public async Task SeedAsync_NoData_ExecutesAllStepsInOrder()
        {
            var callOrder = new List<string>();

            _seedRepositoryMock.Setup(r => r.HasDataAsync()).ReturnsAsync(false);
            _seedRepositoryMock.Setup(r => r.SeedReferenceDataAsync()).Callback(() => callOrder.Add("reference")).Returns(Task.CompletedTask);
            _seedRepositoryMock.Setup(r => r.SeedDevelopersAsync()).Callback(() => callOrder.Add("developers")).Returns(Task.CompletedTask);
            _seedRepositoryMock.Setup(r => r.SeedProjectsAsync()).Callback(() => callOrder.Add("projects")).Returns(Task.CompletedTask);
            _seedRepositoryMock.Setup(r => r.SeedTasksAsync()).Callback(() => callOrder.Add("tasks")).Returns(Task.CompletedTask);

            await _seedService.SeedAsync();

            Assert.Equal(new[] { "reference", "developers", "projects", "tasks" }, callOrder);
        }
    }
}
