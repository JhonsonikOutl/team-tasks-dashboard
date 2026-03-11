using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.Interfaces;
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

        public async Task<IEnumerable<Project>> GetAllAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<Project>("sp_get_all_projects", commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<Project?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<Project>("sp_get_project_by_id", new { ProjectId = id }, commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
