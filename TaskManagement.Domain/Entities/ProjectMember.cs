using TaskManagement.Domain.Enums;

namespace TaskManagement.Domain.Entities
{
    public class ProjectMember
    {
        public string UserId { get; set; } = null!;
        public int ProjectId { get; set; }
        public ProjectMemberRole Role { get; set; }
        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public User User { get; set; } = null!;
        public Project Project { get; set; } = null!;
    }
}
