namespace TeamTasks.Application.DTOs.Tasks
{
    public class TaskDto
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssigneeId { get; set; }
        public string? AssigneeName { get; set; }
        public string? Status { get; set; }
        public string? Priority { get; set; }
        public int EstimatedComplexity { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? CompletionDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
