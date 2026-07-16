using FluentValidation;
using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Validators.Task
{
    public class UpdateTaskDtoValidator : AbstractValidator<UpdateTaskDto>
    {
        public UpdateTaskDtoValidator()
        {

            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Task title is required.")
                .MaximumLength(100).WithMessage("Task title must not exceed 100 characters.")
                .MinimumLength(5).WithMessage("Task title must be at least 5 characters long.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Task description must not exceed 500 characters.")
                .MinimumLength(10).WithMessage("Task description must be at least 10 characters long.");
            RuleFor(x => x.DueDate)
                .GreaterThan(DateTime.Now).WithMessage("Due date must be in the future.")
                .When(x => x.DueDate.HasValue);
        }
    }
}
