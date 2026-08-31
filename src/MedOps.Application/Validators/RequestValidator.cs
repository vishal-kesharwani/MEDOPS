namespace MedOps.Application.Validators;

using FluentValidation;
using MedOps.Application.DTOs;

public class CreateRequestValidator : AbstractValidator<CreateRequestDto>
{
    public CreateRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(5000);
        RuleFor(x => x.Priority).NotEmpty().Must(p => new[] { "Low", "Medium", "High", "Critical" }.Contains(p));
    }
}

public class UpdateRequestValidator : AbstractValidator<UpdateRequestDto>
{
    public UpdateRequestValidator()
    {
        RuleFor(x => x.Title).NotEmpty().MaximumLength(500);
        RuleFor(x => x.Description).MaximumLength(5000);
    }
}