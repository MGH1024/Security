using FluentValidation.TestHelper;
using Security.Application.Features.Auth.Commands.Login;

namespace Security.Test.Application.Features.Auth.Commands.Login;

public class LoginCommandDtoTest
{
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyEmail_WhenValidate_ThenWillInvalid(string? email)
    {
        var loginDto = new LoginDtoBuilder()
            .WithEmail(email)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(loginDto);
        result.ShouldHaveValidationErrorFor(x => x.Email);
    }
    
    [Theory]
    [InlineData("")]
    [InlineData(null)]
    public void GivenEmptyPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var loginDto = new LoginDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(loginDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
    
    
    [Theory]
    [InlineData("p")]
    [InlineData("pa")]
    [InlineData("pas")]
    public void GivenLessThanFourCharacterPassword_WhenValidate_ThenWillInvalid(string? password)
    {
        var loginDto = new LoginDtoBuilder()
            .WithPassword(password)
            .Build();

        var validator = new LoginCommandDtoValidator();
        var result = validator.TestValidate(loginDto);
        result.ShouldHaveValidationErrorFor(x => x.Password);
    }
}