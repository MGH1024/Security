using MGH.Core.Application.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.RevokeToken;

public record RevokeTokenCommand(string Token) : ICommand<RevokedTokenResponse>;