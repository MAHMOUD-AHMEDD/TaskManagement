using FluentValidation;
using TaskManagement.Application.DTOs.Comment;

namespace TaskManagement.Application.Validators.Comment
{
    public class UpdateCommentDtoValidator : AbstractValidator<UpdateCommentDto>
    {
        public UpdateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Comment content is required.")
                .MaximumLength(2000).WithMessage("Comment content must not exceed 2000 characters.")
                .MinimumLength(5).WithMessage("Comment content must be at least 5 characters long.");
        }
    }
}
