namespace TeamTasks.Domain.Entities
{
    public class Developer : BaseEntity
    {
        public int DeveloperId { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsActive { get; set; }
    }
}
