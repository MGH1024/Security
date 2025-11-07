namespace Security.Application.Features.Auth.Commands.Login;

public record LoginCommandResponse(
    string Token,
    DateTime TokenExpiry,
    string RefreshToken,
    DateTime RefreshTokenExpiry,
    bool IsSuccess);