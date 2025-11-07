using Security.Application.Features.Auth.Commands.Register;

namespace Security.Test.Application.Features.Auth.Commands.Register;

public class RegisterCommandDtoBuilder
{
    private string? _email;
    private string? _password;
    private string? _firstName;
    private string? _lastName;

    public RegisterCommandDtoBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public RegisterCommandDtoBuilder WithPassword(string? password)
    {
        _password = password;
        return this;
    }

    public RegisterCommandDtoBuilder WithFirstName(string? firstName)
    {
        _firstName = firstName;
        return this;
    }

    public RegisterCommandDtoBuilder WithLastName(string? lastname)
    {
        _lastName = lastname;
        return this;
    }

    public RegisterCommandDto Build()
    {
        return new RegisterCommandDto(_email, _password, _firstName, _lastName);
    }
}