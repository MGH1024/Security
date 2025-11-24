using MGH.Core.Application.Buses.Commands;

namespace Security.Application.Features.Auth.Commands.RefreshToken;

public record RefreshTokenCommand(string RefreshToken) : ICommand<RefreshTokenResponse>;