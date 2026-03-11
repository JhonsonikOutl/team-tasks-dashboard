namespace TeamTasks.Application.DTOs.Tasks
{
    public class UpdateTaskStatusDto
    {
        public int StatusId { get; set; }
        public int? PriorityId { get; set; }
        public int? EstimatedComplexity { get; set; }
    }
}