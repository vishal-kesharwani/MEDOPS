namespace MedOps.Application.Validators;

using FluentValidation;
using MedOps.Application.DTOs;

public class CreateSiteValidator : AbstractValidator<CreateSiteDto>
{
    public CreateSiteValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Address).NotNull();
        RuleFor(x => x.ContactInfo).NotNull();
        RuleFor(x => x.Address.Street).NotEmpty().MaximumLength(300);
        RuleFor(x => x.Address.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Address.ZipCode).NotEmpty().MaximumLength(20);
        RuleFor(x => x.ContactInfo.Email).EmailAddress().MaximumLength(256);
        RuleFor(x => x.ContactInfo.Phone).MaximumLength(20);
    }
}

public class UpdateSiteValidator : AbstractValidator<UpdateSiteDto>
{
    public UpdateSiteValidator()
    {
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty().MaximumLength(2000);
        RuleFor(x => x.Address.Street).MaximumLength(300);
        RuleFor(x => x.Address.City).MaximumLength(100);
        RuleFor(x => x.Address.State).MaximumLength(100);
        RuleFor(x => x.Address.Country).MaximumLength(100);
        RuleFor(x => x.Address.ZipCode).MaximumLength(20);
        RuleFor(x => x.ContactInfo.Email).EmailAddress().MaximumLength(256);
        RuleFor(x => x.ContactInfo.Phone).MaximumLength(20);
    }
}