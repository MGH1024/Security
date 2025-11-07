using Security.Application.Features.Auth.Commands.Login;

namespace Security.Test.Application.Features.Auth.Commands.Login;

public class LoginDtoBuilder
{
    private string? _email;
    private string? _password;

    public LoginDtoBuilder WithEmail(string? email)
    {
        _email = email;
        return this;
    }

    public LoginDtoBuilder WithPassword(string? password)
    {
        _password = password;
        return this;
    }

    public LoginCommandDto Build()
    {
        return new LoginCommandDto(_email, _password);
    }
}