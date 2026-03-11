namespace TeamTasks.Application.DTOs
{
    public class DelayRiskDto
    {
        public string? DeveloperName { get; set; }
        public int OpenTasksCount { get; set; }
        public double AvgDelayDays { get; set; }
        public DateOnly? NearestDueDate { get; set; }
        public DateOnly? LatestDueDate { get; set; }
        public DateOnly? PredictedCompletionDate { get; set; }
        public int HighRiskFlag { get; set; }
    }
}