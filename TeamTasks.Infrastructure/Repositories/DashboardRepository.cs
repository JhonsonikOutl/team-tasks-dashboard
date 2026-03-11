using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.DTOs.Developers;
using TeamTasks.Application.DTOs.Projects;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Infrastructure.Options;

namespace TeamTasks.Infrastructure.Repositories
{
public class DashboardRepository : IDashboardRepository
    {
        private readonly string _connectionString;

        public DashboardRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<IEnumerable<DeveloperWorkloadDto>> GetDeveloperWorkloadAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<DeveloperWorkloadDto>("sp_get_developer_workload", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<ProjectHealthDto>> GetProjectHealthAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<ProjectHealthDto>("sp_get_project_health", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<DelayRiskDto>> GetDelayRiskAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<DelayRiskDto>("sp_get_delay_risk", commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
