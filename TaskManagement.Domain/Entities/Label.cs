using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Label : BaseEntity
    {
        public string Name { get; set; }
        public string Color { get; set; }
        public int ProjectId { get; set; }
        public Project Project { get; set; }
        public ICollection<TaskLabel> TaskLabels { get; set; } // ✅ add this

    }
}
