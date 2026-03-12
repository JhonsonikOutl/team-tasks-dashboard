namespace TeamTasks.Application.DTOs.Projects
{
    public class ProjectDetailDto
    {
        public int ProjectId { get; set; }
        public string? Name { get; set; }
        public string? ClientName { get; set; }
        public string? Status { get; set; }
        public string? StatusDisplay { get; set; }
        public DateOnly StartDate { get; set; }
        public DateOnly? EndDate { get; set; }
    }
}
