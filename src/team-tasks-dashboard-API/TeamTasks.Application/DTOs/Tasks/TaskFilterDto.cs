namespace TeamTasks.Application.DTOs.Tasks
{
    public class TaskFilterDto
    {
        public int? StatusId { get; set; }
        public int? AssigneeId { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}