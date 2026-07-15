namespace TaskManagement.Application.DTOs.Task
{
    public class UpdateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public Domain.Enums.TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
    }
}
