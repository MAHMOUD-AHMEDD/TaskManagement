using FluentValidation;
using TaskManagement.Application.DTOs.Comment;

namespace TaskManagement.Application.Validators.Comment
{
    public class CreateCommentDtoValidator : AbstractValidator<CreateCommentDto>
    {
        public CreateCommentDtoValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty().WithMessage("Comment content is required.")
                .MaximumLength(2000).WithMessage("Comment content must not exceed 2000 characters.")
                .MinimumLength(5).WithMessage("Comment content must be at least 5 characters long.");
            RuleFor(x => x.TaskId)
                .GreaterThan(0).WithMessage("TaskId must be greater than 0.");
        }
    }
}