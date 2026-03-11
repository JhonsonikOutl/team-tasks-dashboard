using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Domain.Entities;
using TeamTasks.Infrastructure.Options;

namespace TeamTasks.Infrastructure.Repositories
{
    public class ProjectRepository : IProjectRepository
    {
        private readonly string _connectionString;

        public ProjectRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<IEnumerable<ProjectDto>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<ProjectDto>("sp_get_all_projects", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Project>("sp_get_project_by_id", new { ProjectId = id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
