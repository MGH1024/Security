using FluentValidation.TestHelper;
using Security.Application.Features.Auth.Commands.Register;

namespace Security.Test.Application.Features.Auth.Commands.Register;

public class RegisterCommandDtoTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("m.com")]
    public void GivenNonTrueEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyFirstName_WhenValidate_ThenWillInvalid(string? firstName)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithFirstName(firstName)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData("d")]
    public void GivenFirstNameWithLowerThanTwoCharacter_WhenValidate_ThenWillInvalid(string? firstName)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithFirstName(firstName)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.FirstName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyLastName_WhenValidate_ThenWillInvalid(string? lastName)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithLastName(lastName)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Theory]
    [InlineData("d")]
    public void GivenLastNameWithLowerThanTwoCharacter_WhenValidate_ThenWillInvalid(string? lastName)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithLastName(lastName)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.LastName);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    [Theory]
    [InlineData("dww")]
    public void GivenLastNameWithLowerThanFourCharacter_WhenValidate_ThenWillInvalid(string? password)
    {
        var registerCommandDto = new RegisterCommandDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new RegisterCommandDtoValidator();
        var result = validator.TestValidate(registerCommandDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}