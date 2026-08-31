using FluentValidation.TestHelper;
using MedOps.Application.DTOs;
using MedOps.Application.Validators;

namespace MedOps.UnitTests.Application;

public class StudyValidatorTests
{
    private readonly CreateStudyValidator _createValidator = new();
    private readonly UpdateStudyValidator _updateValidator = new();

    [Fact]
    public async Task CreateValidator_ShouldPassWithValidInput()
    {
        var dto = new CreateStudyDto { Name = "Test Study", Description = "Description" };
        var result = await _createValidator.TestValidateAsync(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task CreateValidator_ShouldFailWithEmptyName()
    {
        var dto = new CreateStudyDto { Name = "", Description = "Description" };
        var result = await _createValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateValidator_ShouldFailWithNameTooLong()
    {
        var dto = new CreateStudyDto { Name = new string('A', 201), Description = "Description" };
        var result = await _createValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }

    [Fact]
    public async Task CreateValidator_ShouldFailWithEmptyDescription()
    {
        var dto = new CreateStudyDto { Name = "Test", Description = "" };
        var result = await _createValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.Description);
    }

    [Fact]
    public async Task UpdateValidator_ShouldPassWithValidInput()
    {
        var dto = new UpdateStudyDto { Name = "Updated Study", Description = "Updated Description" };
        var result = await _updateValidator.TestValidateAsync(dto);
        result.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public async Task UpdateValidator_ShouldFailWithEmptyName()
    {
        var dto = new UpdateStudyDto { Name = "", Description = "Description" };
        var result = await _updateValidator.TestValidateAsync(dto);
        result.ShouldHaveValidationErrorFor(x => x.Name);
    }
}
