using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Comment;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CommentsController : ControllerBase
    {

        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        [HttpGet("{taskId}")]
        public async Task<IActionResult> GetComments(int taskId)
        {
            var comments = await _commentService.GetCommentsAsync(taskId);
            return Ok(comments);
        }

        [HttpGet("comment/{id}")]
        public async Task<IActionResult> GetCommentById(int id)
        {
            var comment = await _commentService.GetCommentByIdAsync(id);
            if (comment == null)
            {
                return NotFound();
            }
            return Ok(comment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDto dto)
        {
            var userId = User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userId))
            {
                return Unauthorized("User ID could not be retrieved from the token.");
            }
            var comment = await _commentService.CreateCommentAsync(dto, userId);
            return CreatedAtAction(nameof(GetCommentById), new { id = comment.Id }, comment);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int id, [FromBody] UpdateCommentDto dto)
        {
            await _commentService.UpdateCommentAsync(id, dto);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            await _commentService.DeleteCommentAsync(id);
            return NoContent();
        }


    }
}
