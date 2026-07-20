using AutoMapper;
using TaskManagement.Application.DTOs.Comment;
using TaskManagement.Application.Exceptions;
using TaskManagement.Application.Interfaces;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Application.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CommentService(IUnitOfWork unitOfWork, IMapper mapper)
        {

            _unitOfWork = unitOfWork;
            _mapper = mapper;

        }

        public async Task<IEnumerable<CommentDto>> GetCommentsAsync(int taskId)
        {
            var comments = await _unitOfWork.Comments.FindAllAsync(c => c.TaskId == taskId);
            return _mapper.Map<IEnumerable<CommentDto>>(comments);
        }

        public async Task<CommentDto?> GetCommentByIdAsync(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            return comment == null ? null : _mapper.Map<CommentDto>(comment);
        }

        public async Task<CommentDto> CreateCommentAsync(CreateCommentDto dto, string userId)
        {
            var comment = _mapper.Map<Domain.Entities.Comment>(dto);
            comment.UserId = userId;
            await _unitOfWork.Comments.AddAsync(comment);
            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<CommentDto>(comment);
        }

        public async Task UpdateCommentAsync(int id, UpdateCommentDto dto)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found.");
            }
            _mapper.Map(dto, comment);
            _unitOfWork.Comments.Update(comment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCommentAsync(int id)
        {
            var comment = await _unitOfWork.Comments.GetByIdAsync(id);
            if (comment == null)
            {
                throw new NotFoundException($"Comment with id {id} not found.");
            }
            _unitOfWork.Comments.Delete(comment);
            await _unitOfWork.SaveChangesAsync();
        }




    }
}
