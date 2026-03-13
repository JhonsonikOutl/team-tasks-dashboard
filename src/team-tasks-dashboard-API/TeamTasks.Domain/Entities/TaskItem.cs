namespace TeamTasks.Domain.Entities
{
    public class TaskItem : BaseEntity
    {
        public int TaskId { get; set; }
        public int ProjectId { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int? AssigneeId { get; set; }
        public int StatusId { get; set; }
        public int PriorityId { get; set; }
        public int EstimatedComplexity { get; set; }
        public DateOnly DueDate { get; set; }
        public DateOnly? CompletionDate { get; set; }
    }
}
