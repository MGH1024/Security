using MGH.Core.Application.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.Login;

public record LoginCommand(LoginCommandDto LoginCommandDto) : ICommand<LoginCommandResponse>;