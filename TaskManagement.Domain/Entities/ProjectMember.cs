using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class ProjectMember
    {
        public string UserId { get; set; }
        public int ProjectId { get; set; }
        public ProjectMemberRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; }
        public Project Project { get; set; }
    }
}
