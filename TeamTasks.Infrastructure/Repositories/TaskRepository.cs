using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.Interfaces;
using TeamTasks.Domain.Entities;
using TeamTasks.Infrastructure.Options;

namespace TeamTasks.Infrastructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly string _connectionString;

        public TaskRepository(IOptions<DatabaseOptions> options)
        {
            _connectionString = options.Value.ConnectionString;
        }

        public async Task<IEnumerable<TaskItem>> GetByProjectIdAsync(int projectId, int? statusId, int? assigneeId, int page, int pageSize)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TaskItem>(
                "sp_get_tasks_by_project",
                new { ProjectId = projectId, StatusId = statusId, AssigneeId = assigneeId, Page = page, PageSize = pageSize },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<TaskItem?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<TaskItem>(
                "sp_get_task_by_id",
                new { TaskId = id },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            using var connection = new SqlConnection(_connectionString);
            var id = await connection.QuerySingleAsync<int>(
                "sp_insert_task",
                new
                {
                    task.ProjectId,
                    task.Title,
                    task.Description,
                    task.AssigneeId,
                    task.StatusId,
                    task.PriorityId,
                    task.EstimatedComplexity,
                    task.DueDate
                },
                commandType: System.Data.CommandType.StoredProcedure
            );

            return (await GetByIdAsync(id))!;
        }

        public async Task UpdateStatusAsync(int id, int statusId, int? priorityId, int? estimatedComplexity)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "sp_update_task_status",
                new { TaskId = id, StatusId = statusId, PriorityId = priorityId, EstimatedComplexity = estimatedComplexity },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}
