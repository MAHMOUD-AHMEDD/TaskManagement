using TaskManagement.Domain.Common;

namespace TaskManagement.Domain.Entities
{
    public class Comment : BaseEntity
    {
        public string Content { get; set; }
        public string UserId { get; set; }
        public int TaskId { get; set; }
        public User User { get; set; }
        public Task Task { get; set; }

    }
}
