namespace TeamTasks.Application.DTOs.Developers
{
    public class DeveloperWorkloadDto
    {
        public int DeveloperId { get; set; }
        public string? DeveloperName { get; set; }
        public int OpenTasksCount { get; set; }
        public double AverageEstimatedComplexity { get; set; }
    }
}