namespace TaskManagement.Domain.Entities
{
    public class TaskAssignment
    {
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public DateTime AssignedAt { get; set; } = DateTime.UtcNow;
        public User User { get; set; }
        public Task Task { get; set; }
    }
}
