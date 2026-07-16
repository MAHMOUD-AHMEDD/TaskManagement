using FluentValidation;
using TaskManagement.Application.DTOs.Project;

namespace TaskManagement.Application.Validators.Project
{
    public class CreateProjectDtoValidator : AbstractValidator<CreateProjectDto>
    {
        public CreateProjectDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Project name is required.")
                .MaximumLength(100).WithMessage("Project name must not exceed 100 characters.");
            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Project description must not exceed 500 characters.");
        }
    }
}
