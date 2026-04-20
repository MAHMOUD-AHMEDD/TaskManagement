using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public int TaskId { get; set; }
        public User User { get; set; } = null!;
        public Task Task { get; set; } = null!;
    }
}
