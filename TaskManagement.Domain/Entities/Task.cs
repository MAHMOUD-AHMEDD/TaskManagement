using TaskManagement.Domain.Common;
using TaskManagement.Domain.Enums;
namespace TaskManagement.Domain.Entities
{
    public class Task : BaseEntity
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public Enums.TaskStatus Status { get; set; }
        public TaskPriority Priority { get; set; }
        public DateTime DueDate { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<TaskAssignment> TaskAssignments { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<TaskLabel> TaskLabels { get; set; }
    }
}
