using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Label : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string Color { get; set; } = null!;
        public int ProjectId { get; set; }
        public Project Project { get; set; } = null!;
        public ICollection<TaskLabel> TaskLabels { get; set; } = new List<TaskLabel>();
    }
}
