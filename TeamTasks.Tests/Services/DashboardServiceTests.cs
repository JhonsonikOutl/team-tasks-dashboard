using Moq;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Services;

namespace TeamTasks.Tests.Services
{
    public class DashboardServiceTests
    {
        private readonly Mock<IDashboardRepository> _dashboardRepositoryMock;
        private readonly DashboardService _sut;

        public DashboardServiceTests()
        {
            _dashboardRepositoryMock = new Mock<IDashboardRepository>();
            _sut = new DashboardService(_dashboardRepositoryMock.Object);
        }

        [Fact]
        public async Task GetDeveloperWorkloadAsync_ReturnsDeveloperWorkloadFromRepository()
        {
            var expected = new List<DeveloperWorkloadDto>
            {
                new() { DeveloperName = "Ana García", OpenTasksCount = 4, AverageEstimatedComplexity = 2.5 },
                new() { DeveloperName = "Carlos López", OpenTasksCount = 2, AverageEstimatedComplexity = 3.0 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetDeveloperWorkloadAsync()).ReturnsAsync(expected);

            var result = await _sut.GetDeveloperWorkloadAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetDeveloperWorkloadAsync_NoDevelopers_ReturnsEmptyCollection()
        {
            _dashboardRepositoryMock.Setup(r => r.GetDeveloperWorkloadAsync()).ReturnsAsync(new List<DeveloperWorkloadDto>());

            var result = await _sut.GetDeveloperWorkloadAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDeveloperWorkloadAsync_PreservesWorkloadValues()
        {
            var expected = new List<DeveloperWorkloadDto>
            {
                new() { DeveloperName = "Ana García", OpenTasksCount = 5, AverageEstimatedComplexity = 4.2 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetDeveloperWorkloadAsync()).ReturnsAsync(expected);

            var result = await _sut.GetDeveloperWorkloadAsync();
            var single = result.Single();

            Assert.Equal(5, single.OpenTasksCount);
            Assert.Equal(4.2, single.AverageEstimatedComplexity);
        }

        [Fact]
        public async Task GetProjectHealthAsync_ReturnsProjectHealthFromRepository()
        {
            var expected = new List<ProjectHealthDto>
            {
                new() { ProjectName = "Proyecto A", TotalTasks = 10, OpenTasks = 6, CompletedTasks = 4 },
                new() { ProjectName = "Proyecto B", TotalTasks = 5, OpenTasks = 0, CompletedTasks = 5 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetProjectHealthAsync()).ReturnsAsync(expected);

            var result = await _sut.GetProjectHealthAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetProjectHealthAsync_NoProjects_ReturnsEmptyCollection()
        {
            _dashboardRepositoryMock.Setup(r => r.GetProjectHealthAsync()).ReturnsAsync(new List<ProjectHealthDto>());

            var result = await _sut.GetProjectHealthAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetProjectHealthAsync_PreservesTaskCounts()
        {
            var expected = new List<ProjectHealthDto>
            {
                new() { ProjectName = "Proyecto A", TotalTasks = 8, OpenTasks = 5, CompletedTasks = 3 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetProjectHealthAsync()).ReturnsAsync(expected);

            var result = await _sut.GetProjectHealthAsync();
            var single = result.Single();

            Assert.Equal(8, single.TotalTasks);
            Assert.Equal(5, single.OpenTasks);
            Assert.Equal(3, single.CompletedTasks);
        }

        [Fact]
        public async Task GetDelayRiskAsync_ReturnsDelayRiskFromRepository()
        {
            var expected = new List<DelayRiskDto>
            {
                new() { DeveloperName = "Ana García", OpenTasksCount = 3, AvgDelayDays = 5.0, HighRiskFlag = 1 },
                new() { DeveloperName = "Carlos López", OpenTasksCount = 2, AvgDelayDays = 0.0, HighRiskFlag = 0 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetDelayRiskAsync()).ReturnsAsync(expected);

            var result = await _sut.GetDelayRiskAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetDelayRiskAsync_NoDevelopers_ReturnsEmptyCollection()
        {
            _dashboardRepositoryMock.Setup(r => r.GetDelayRiskAsync()).ReturnsAsync(new List<DelayRiskDto>());

            var result = await _sut.GetDelayRiskAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetDelayRiskAsync_HighRiskFlagIsPreservedFromRepository()
        {
            var expected = new List<DelayRiskDto>
            {
                new() { DeveloperName = "Ana García", HighRiskFlag = 1 },
                new() { DeveloperName = "Carlos López", HighRiskFlag = 0 }
            };
            _dashboardRepositoryMock.Setup(r => r.GetDelayRiskAsync()).ReturnsAsync(expected);

            var result = await _sut.GetDelayRiskAsync();

            Assert.Single(result, r => r.HighRiskFlag == 1);
            Assert.Single(result, r => r.HighRiskFlag == 0);
        }

        [Fact]
        public async Task GetDelayRiskAsync_PreservesDelayMetrics()
        {
            var expected = new List<DelayRiskDto>
            {
                new()
                {
                    DeveloperName = "Ana García",
                    OpenTasksCount = 4,
                    AvgDelayDays = 3.5,
                    NearestDueDate = new DateOnly(2026, 3, 15),
                    LatestDueDate = new DateOnly(2026, 3, 30),
                    PredictedCompletionDate = new DateOnly(2026, 4, 3),
                    HighRiskFlag = 1
                }
            };
            _dashboardRepositoryMock.Setup(r => r.GetDelayRiskAsync()).ReturnsAsync(expected);

            var result = await _sut.GetDelayRiskAsync();
            var single = result.Single();

            Assert.Equal(4, single.OpenTasksCount);
            Assert.Equal(3.5, single.AvgDelayDays);
            Assert.Equal(new DateOnly(2026, 4, 3), single.PredictedCompletionDate);
            Assert.Equal(1, single.HighRiskFlag);
        }
    }
}