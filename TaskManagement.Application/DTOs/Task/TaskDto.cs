using TaskManagement.Domain.Enums;

namespace TaskManagement.Application.DTOs.Task
{
    public class TaskDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Domain.Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public string ProjectName { get; set; } = null!;
        public List<string> AssignedUserNames { get; set; } = new();
        public List<string> Labels { get; set; } = new();
    }
}
