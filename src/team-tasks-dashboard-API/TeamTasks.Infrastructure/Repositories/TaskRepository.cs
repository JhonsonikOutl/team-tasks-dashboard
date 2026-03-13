using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Application.Interfaces.Repositories;
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

        public async Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, TaskFilterDto filter)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryAsync<TaskDto>(
                "sp_get_tasks_by_project",
                new { ProjectId = projectId, filter.StatusId, filter.AssigneeId, filter.Page, filter.PageSize },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            using var connection = new SqlConnection(_connectionString);
            return await connection.QueryFirstOrDefaultAsync<TaskDto>("sp_get_task_by_id", new { TaskId = id }, commandType: System.Data.CommandType.StoredProcedure);
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

            task.TaskId = id;
            return task;
        }

        public async Task UpdateStatusAsync(int id, UpdateTaskStatusDto filterUpdate)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.ExecuteAsync(
                "sp_update_task_status",
                new { TaskId = id, filterUpdate.StatusId, filterUpdate.PriorityId, filterUpdate.EstimatedComplexity },
                commandType: System.Data.CommandType.StoredProcedure
            );
        }
    }
}
