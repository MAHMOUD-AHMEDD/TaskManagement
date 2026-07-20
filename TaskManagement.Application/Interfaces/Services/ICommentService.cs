using TaskManagement.Application.DTOs.Comment;

namespace TaskManagement.Application.Interfaces.Services;

public interface ICommentService
{
    Task<IEnumerable<CommentDto>> GetCommentsAsync(int taskId);
    Task<CommentDto?> GetCommentByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, string userId);
    Task UpdateCommentAsync(int id, UpdateCommentDto dto);
    Task DeleteCommentAsync(int id);
}