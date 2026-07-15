namespace TaskManagement.Application.DTOs.Task
{
    public class CreateTaskDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Domain.Enums.TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
    }
}
