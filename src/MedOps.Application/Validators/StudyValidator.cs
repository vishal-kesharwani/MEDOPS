namespace MedOps.Application.Validators;

using FluentValidation;
using MedOps.Application.DTOs;

public class CreateStudyValidator : AbstractValidator<CreateStudyDto>
{
    public CreateStudyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}

public class UpdateStudyValidator : AbstractValidator<UpdateStudyDto>
{
    public UpdateStudyValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.StartDate).LessThanOrEqualTo(x => x.EndDate).When(x => x.StartDate.HasValue && x.EndDate.HasValue);
    }
}