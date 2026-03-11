namespace TeamTasks.Application.DTOs.Tasks
{
    public class CreateTaskDto
    {
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssigneeId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int EstimatedComplexity { get; set; }
        public DateOnly DueDate { get; set; }
    }
}