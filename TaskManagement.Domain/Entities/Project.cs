using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Project : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string OwnerId { get; set; }

        public User Owner { get; set; }
        public ICollection<Task> Tasks { get; set; }
        public ICollection<ProjectMember> ProjectMembers { get; set; }
        public ICollection<Label> Labels { get; set; }
    }
}
