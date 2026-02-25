using FluentValidation;
using TaskFlow.Api.DTOs;

namespace TaskFlow.Api.Validators;

public class CreateTaskValidator : AbstractValidator<CreateTaskDtos>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty().WithMessage("This is requird")
            .MaximumLength(200).WithMessage("This cannot exceed 200 characters");

        RuleFor(x => x.Description)
            .MaximumLength(1000).WithMessage("Description cannot exceed 1000 characters");
        
        RuleFor(x => x.DueDate)
            .GreaterThan(DateTime.Now).WithMessage("Due data must be in the future")
            .When(x => x.DueDate.HasValue);
    }
}