using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.DTOs.Catalog;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Infrastructure.Options;

namespace TeamTasks.Infrastructure.Repositories
{
    public class CatalogRepository : ICatalogRepository
    {
        private readonly string _connectionString;

        public CatalogRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<IEnumerable<CatalogItemDto>> GetTaskStatusesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<CatalogItemDto>("sp_get_task_statuses", commandType: System.Data.CommandType.StoredProcedure);
        }

        public async Task<IEnumerable<CatalogItemDto>> GetTaskPrioritiesAsync()
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<CatalogItemDto>("sp_get_task_priorities", commandType: System.Data.CommandType.StoredProcedure);
        }
    }
}