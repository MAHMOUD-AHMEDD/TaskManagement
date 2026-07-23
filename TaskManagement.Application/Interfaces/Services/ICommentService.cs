using TaskManagement.Application.DTOs.Comment;
using TaskManagement.Application.DTOs.Common;

namespace TaskManagement.Application.Interfaces.Services;

public interface ICommentService
{
    Task<PagedResult<CommentDto>> GetCommentsAsync(int taskId, PaginationParams paginationParams);
    Task<CommentDto?> GetCommentByIdAsync(int id);
    Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, string userId);
    Task UpdateCommentAsync(int id, UpdateCommentDto dto);
    Task DeleteCommentAsync(int id);
}