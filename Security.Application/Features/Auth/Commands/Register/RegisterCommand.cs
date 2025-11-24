using MGH.Core.Application.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.Register;

public record RegisterCommand(RegisterCommandDto RegisterCommandDto)
    : ICommand<RegisterCommandResponse>;