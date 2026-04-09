using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime? DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public ICollection<TaskAssignment> TaskAssignments { get; set; } = new List<TaskAssignment>();
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
    }
}
