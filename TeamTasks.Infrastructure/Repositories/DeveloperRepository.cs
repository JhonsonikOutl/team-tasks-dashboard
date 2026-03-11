using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.DTOs;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Infrastructure.Options;

namespace TeamTasks.Infrastructure.Repositories
{
    public class DeveloperRepository : IDeveloperRepository
    {
        private readonly string _connectionString;

        public DeveloperRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<IEnumerable<DeveloperDto>> GetActiveAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<DeveloperDto>("sp_get_active_developers", commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}
