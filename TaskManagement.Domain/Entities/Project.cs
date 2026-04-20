using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; } = null!;
        public string? Description { get; set; }
        public string OwnerId { get; set; } = null!;

        public User Owner { get; set; } = null!;
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
        public ICollection<ProjectMember> ProjectMembers { get; set; } = new List<ProjectMember>();
        public ICollection<Label> Labels { get; set; } = new List<Label>();
    }
}
