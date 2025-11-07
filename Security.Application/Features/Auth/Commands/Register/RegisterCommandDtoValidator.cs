using FluentValidation;

namespace Security.Application.Features.Auth.Commands.Register;

public class RegisterCommandDtoValidator : AbstractValidator<RegisterCommandDto>
{
    public RegisterCommandDtoValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.Password).NotEmpty().MinimumLength(4);
    }
}
