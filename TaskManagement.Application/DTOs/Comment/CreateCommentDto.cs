namespace TaskManagement.Application.DTOs.Comment
{
    public class CreateCommentDto
    {
        public string Content { get; set; } = null!;
        public int TaskId { get; set; }
    }
}
