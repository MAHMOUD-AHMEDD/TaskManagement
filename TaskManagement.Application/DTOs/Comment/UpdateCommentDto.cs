namespace TaskManagement.Application.DTOs.Comment
{
    public class UpdateCommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
    }
}
