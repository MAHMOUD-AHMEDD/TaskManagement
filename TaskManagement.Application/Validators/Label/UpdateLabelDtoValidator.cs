using FluentValidation;
using TaskManagement.Application.DTOs.Label;

namespace TaskManagement.Application.Validators.Label
{
    public class UpdateLabelDtoValidator : AbstractValidator<UpdateLabelDto>
    {
        public UpdateLabelDtoValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Label name is required.")
                .MaximumLength(50).WithMessage("Label name must not exceed 50 characters.")
                .MinimumLength(2).WithMessage("Label name must be at least 2 characters long.");
            RuleFor(x => x.Color)
                .NotEmpty().WithMessage("Label color is required.")
                .Matches("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$").WithMessage("Label color must be a valid hex color code.");

        }
    }
}