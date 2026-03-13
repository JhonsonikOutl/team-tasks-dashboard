using TeamTasks.Application.DTOs.Tasks;
using TeamTasks.Application.Interfaces.Repositories;
using TeamTasks.Application.Interfaces.Services;
using TeamTasks.Domain.Entities;
using Enums = TeamTasks.Domain.Enums;

namespace TeamTasks.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;
        private readonly IProjectRepository _projectRepository;
        private readonly IDeveloperRepository _developerRepository;

        public TaskService(ITaskRepository taskRepository, IProjectRepository projectRepository, IDeveloperRepository developerRepository)
        {
            _taskRepository = taskRepository;
            _projectRepository = projectRepository;
            _developerRepository = developerRepository;
        }

        public async Task<IEnumerable<TaskDto>> GetByProjectIdAsync(int projectId, TaskFilterDto filter)
        {
            return await _taskRepository.GetByProjectIdAsync(projectId, filter);
        }

        public async Task<TaskDto?> GetByIdAsync(int id)
        {
            return await _taskRepository.GetByIdAsync(id);
        }

        public async Task<TaskDto> CreateAsync(CreateTaskDto dto)
        {
            var project = await _projectRepository.GetByIdAsync(dto.ProjectId);
            if (project is null)
                throw new ArgumentException($"El proyecto con Id {dto.ProjectId} no existe.");

            if (dto.AssigneeId.HasValue)
            {
                var developers = await _developerRepository.GetActiveAsync();
                var developerExists = developers.Any(d => d.DeveloperId == dto.AssigneeId.Value);
                if (!developerExists)
                    throw new ArgumentException($"El desarrollador con Id {dto.AssigneeId} no existe o no esta activo.");
            }

            if (dto.EstimatedComplexity < 1 || dto.EstimatedComplexity > 5)
                throw new ArgumentException("La complejidad estimada debe estar entre 1 y 5.");

            if (dto.DueDate < DateOnly.FromDateTime(DateTime.Today))
                throw new ArgumentException("La fecha de vencimiento no puede ser en el pasado.");

            var task = new TaskItem
            {
                ProjectId = dto.ProjectId,
                Title = dto.Title,
                Description = dto.Description,
                AssigneeId = dto.AssigneeId,
                StatusId = dto.StatusId,
                PriorityId = dto.PriorityId,
                EstimatedComplexity = dto.EstimatedComplexity,
                DueDate = dto.DueDate
            };

            var created = await _taskRepository.CreateAsync(task);

            return (await _taskRepository.GetByIdAsync(created.TaskId))!;
        }

        public async Task UpdateStatusAsync(int id, UpdateTaskStatusDto filterUpdate)
        {
            var task = await _taskRepository.GetByIdAsync(id);
            if (task is null)
                throw new ArgumentException($"La tarea con Id {id} no existe.");

            if (!Enum.IsDefined(typeof(Enums.TaskStatus), filterUpdate.StatusId))
                throw new ArgumentException("El estado indicado no es valido.");

            if (filterUpdate.PriorityId.HasValue && !Enum.IsDefined(typeof(Enums.TaskPriority), filterUpdate.PriorityId!))
                throw new ArgumentException("La prioridad indicada no es valida.");

            if (filterUpdate.EstimatedComplexity.HasValue && (filterUpdate.EstimatedComplexity < 1 || filterUpdate.EstimatedComplexity > 5))
                throw new ArgumentException("La complejidad estimada debe estar entre 1 y 5.");

            await _taskRepository.UpdateStatusAsync(id, filterUpdate);
        }
    }
}
