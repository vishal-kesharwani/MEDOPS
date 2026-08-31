namespace MedOps.Application.Validators;

using FluentValidation;
using MedOps.Application.DTOs;

public class CreateTaskValidator : AbstractValidator<CreateTaskDto>
{
    public CreateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(5000);
        RuleFor(x => x.AssignedTo).NotEmpty();
        RuleFor(x => x.Priority).NotEmpty().Must(p => new[] { "Low", "Medium", "High", "Critical" }.Contains(p));
        RuleFor(x => x.DueDate).GreaterThan(DateOnly.FromDateTime(DateTime.Today)).When(x => x.DueDate.HasValue);
    }
}

public class UpdateTaskValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTaskValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Priority).Must(p => new[] { "Low", "Medium", "High", "Critical" }.Contains(p));
    }
}