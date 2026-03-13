using Moq;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Services;

namespace TeamTasks.Tests.Services
{
    public class DeveloperServiceTests
    {
        private readonly Mock<IDeveloperRepository> _developerRepositoryMock;
        private readonly DeveloperService _sut;

        public DeveloperServiceTests()
        {
            _developerRepositoryMock = new Mock<IDeveloperRepository>();
            _sut = new DeveloperService(_developerRepositoryMock.Object);
        }

        [Fact]
        public async Task GetActiveAsync_ReturnsActiveDevelopersFromRepository()
        {
            var expected = new List<DeveloperDto>
            {
                new() { DeveloperId = 1, FullName = "Ana García" },
                new() { DeveloperId = 2, FullName = "Carlos López" }
            };
            _developerRepositoryMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(expected);

            var result = await _sut.GetActiveAsync();

            Assert.Equal(2, result.Count());
        }

        [Fact]
        public async Task GetActiveAsync_NoActiveDevelopers_ReturnsEmptyCollection()
        {
            _developerRepositoryMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(new List<DeveloperDto>());

            var result = await _sut.GetActiveAsync();

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetActiveAsync_OnlyReturnsDataProvidedByRepository()
        {
            var expected = new List<DeveloperDto>
            {
                new() { DeveloperId = 5, FullName = "Luis Martínez", Email = "luis@teamtasks.com" }
            };
            _developerRepositoryMock.Setup(r => r.GetActiveAsync()).ReturnsAsync(expected);

            var result = await _sut.GetActiveAsync();
            var single = result.Single();

            Assert.Equal(5, single.DeveloperId);
            Assert.Equal("Luis Martínez", single.FullName);
            Assert.Equal("luis@teamtasks.com", single.Email);
        }
    }
}
